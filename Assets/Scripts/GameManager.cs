using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scoring")]
    [SerializeField] private int pointsPerHit = 1;

    [Header("Phase 7 — Timer")]
    [SerializeField] private float roundDurationSeconds = 60f;

    [Header("Phase 8 — UI (optional until wired)")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Phase 9 — Restart")]
    [Tooltip("Optional: drag your Restart button here, or wire Button OnClick to BeginRound in the Inspector.")]
    [SerializeField] private Button restartButton;

    public bool IsGameActive { get; private set; }

    public int Score { get; private set; }

    public float TimeRemaining { get; private set; }

    private void Awake ()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("GameManager: more than one in scene. Using the first.");
            return;
        }
        Instance = this;
    }

    private void Start ()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(BeginRound);
        BeginRound();
    }

    private void OnDestroy ()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Update ()
    {
        if (!IsGameActive)
            return;

        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            EndRound();
        }

        RefreshTimerUi();
    }

    public int PointsPerHit => pointsPerHit;

    /// <summary>Phase 9: clears spawned food, resets timer/score, hides game over. Hook a UI Button here or assign Restart Button below.</summary>
    public void BeginRound ()
    {
        ClearSpawnedTargets();
        IsGameActive = true;
        TimeRemaining = roundDurationSeconds;
        Score = 0;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        RefreshScoreUi();
        RefreshTimerUi();
    }

    private static void ClearSpawnedTargets ()
    {
        Target[] targets = Object.FindObjectsByType<Target>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Target t in targets)
        {
            if (t != null)
                Object.Destroy(t.gameObject);
        }
    }

    private void EndRound ()
    {
        IsGameActive = false;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        ShowRestartUi();
        RefreshTimerUi();
    }

    /// <summary>
    /// If you disabled the Restart button in the Inspector (or turned off Interactable), it would stay hidden or dead when the panel opens. We fix that when the round ends.
    /// </summary>
    private void ShowRestartUi ()
    {
        if (restartButton == null)
            return;
        restartButton.gameObject.SetActive(true);
        restartButton.interactable = true;
    }

    public void AddScore (int amount)
    {
        if (!IsGameActive)
            return;
        Score += amount;
        RefreshScoreUi();
    }

    public void AddScoreForHit ()
    {
        AddScore(pointsPerHit);
    }

    public void ResetScore ()
    {
        Score = 0;
        RefreshScoreUi();
    }

    private void RefreshScoreUi ()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {Score}";
    }

    private void RefreshTimerUi ()
    {
        if (timerText != null)
        {
            int whole = Mathf.Max(0, Mathf.CeilToInt(TimeRemaining));
            timerText.text = $"Time: {whole}";
        }
    }
}
