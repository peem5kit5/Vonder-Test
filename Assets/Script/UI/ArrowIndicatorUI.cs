using UnityEngine;

public class ArrowIndicatorUI : MonoBehaviour
{
    [SerializeField] private RectTransform arrowRect;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float edgePadding = 50f;
    [SerializeField] private Vector2 targetSnapOffset = new Vector2(0f, 60f);

    private Transform target;

    private void Update()
    {
        UpdateArrow();
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private void UpdateArrow()
    {
        if (target == null)
        {
            arrowRect.gameObject.SetActive(false);
            return;
        }

        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        if (screenPos.z < 0f)
            screenPos = new Vector3(-screenPos.x, -screenPos.y, 0f);

        arrowRect.gameObject.SetActive(true);

        bool isOnScreen = screenPos.x > 0f && screenPos.x < Screen.width
                       && screenPos.y > 0f && screenPos.y < Screen.height;

        Vector2 targetPos2D = new Vector2(screenPos.x, screenPos.y);
        Vector2 arrowPos;

        if (isOnScreen)
        {
            arrowPos = targetPos2D + targetSnapOffset;
        }
        else
        {
            arrowPos = ClampToScreenEdge(screenPos);
        }

        arrowRect.position = arrowPos;

        Vector2 dir = targetPos2D - arrowPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        arrowRect.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private Vector2 ClampToScreenEdge(Vector3 screenPos)
    {
        Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 dir = (new Vector2(screenPos.x, screenPos.y) - center).normalized;

        float halfW = center.x - edgePadding;
        float halfH = center.y - edgePadding;

        float scaleX = Mathf.Abs(dir.x) > 0.001f ? halfW / Mathf.Abs(dir.x) : float.MaxValue;
        float scaleY = Mathf.Abs(dir.y) > 0.001f ? halfH / Mathf.Abs(dir.y) : float.MaxValue;

        return center + dir * Mathf.Min(scaleX, scaleY);
    }
}
