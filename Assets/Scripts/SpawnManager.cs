using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Spawn area (course board is on XY, Z = 0)")]
    [SerializeField] private float minX = -3.75f;
    [SerializeField] private float maxX = 3.75f;
    [SerializeField] private float minY = -3.75f;
    [SerializeField] private float maxY = 3.75f;
    [SerializeField] private float spawnZ = 0f;

    /// <summary>Hidden clone used for Instantiate; survives when the original Hierarchy reference is destroyed.</summary>
    private GameObject spawnTemplate;

    private Quaternion spawnRotation;

    private void Start ()
    {
        if (foodPrefab == null)
        {
            Debug.LogWarning("SpawnManager: assign Food Prefab in the Inspector.");
            return;
        }

        if (foodPrefab.scene.IsValid())
        {
            Debug.LogWarning(
                "SpawnManager: Food Prefab points at a scene object. Spawning still works (a hidden copy is kept). For a cleaner setup, assign the prefab from the Project window (blue cube), not the Hierarchy.");
        }

        spawnTemplate = Instantiate(foodPrefab, transform);
        spawnTemplate.name = "_SpawnTemplate (do not delete)";
        spawnTemplate.SetActive(false);
        spawnRotation = spawnTemplate.transform.rotation;

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop ()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (GameManager.Instance != null && !GameManager.Instance.IsGameActive)
                continue;
            if (spawnTemplate == null)
            {
                Debug.LogWarning("SpawnManager: spawn template missing. Check Spawn Manager object and Food Prefab.");
                continue;
            }
            Vector3 pos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), spawnZ);
            GameObject instance = Instantiate(spawnTemplate, pos, spawnRotation);
            instance.SetActive(true);
        }
    }
}
