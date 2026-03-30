using UnityEngine;
using UnityEngine.InputSystem;

public class TargetInput : MonoBehaviour
{
    private void Update ()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Camera cam = Camera.main;
        if (cam == null)
            return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Collide))
            return;

        Collider col = hit.collider;
        if (col == null)
            return;

        Target target = col.GetComponentInParent<Target>();
        if (target == null)
            return;

        target.HandleClick();
    }
}
