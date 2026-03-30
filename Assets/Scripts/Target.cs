using UnityEngine;

public class Target : MonoBehaviour
{
    private bool handled;

    public void HandleClick ()
    {
        if (handled || !isActiveAndEnabled)
            return;
        handled = true;

        if (GameManager.Instance != null)
            GameManager.Instance.AddScoreForHit();
        Destroy(gameObject);
    }
}
