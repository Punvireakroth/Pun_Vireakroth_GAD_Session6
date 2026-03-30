using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TargetInput : MonoBehaviour
{
    private void Update ()
    {
        if (!TryGetPrimaryClickScreenPosition(out Vector2 screenPos))
            return;

        Camera cam = Camera.main;
        if (cam == null)
            return;

        Ray ray = cam.ScreenPointToRay(screenPos);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Collide))
            return;

        Collider col = hit.collider;
        if (col == null)
            return;

        Target target = col.GetComponentInParent<Target>();
        if (target == null)
            target = col.GetComponentInChildren<Target>();
        if (target == null)
            return;

        target.HandleClick();
    }

    private static bool TryGetPrimaryClickScreenPosition (out Vector2 screenPos)
    {
        screenPos = default;
        if (Mouse.current != null)
        {
            if (!Mouse.current.leftButton.wasPressedThisFrame)
                return false;
            screenPos = Mouse.current.position.ReadValue();
            return true;
        }
        if (Pen.current != null && Pen.current.press.wasPressedThisFrame)
        {
            screenPos = Pen.current.position.ReadValue();
            return true;
        }
        if (Touchscreen.current != null)
        {
            TouchControl primary = Touchscreen.current.primaryTouch;
            if (!primary.press.wasPressedThisFrame)
                return false;
            screenPos = primary.position.ReadValue();
            return true;
        }
        return false;
    }
}
