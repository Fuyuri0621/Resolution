using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class MapDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IScrollHandler
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 0.1f;     // 滾輪縮放速度
    [SerializeField] private float minZoom = 0.5f;       // 最小縮放
    [SerializeField] private float maxZoom = 3f;         // 最大縮放

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Vector2 lastMousePos;
    private bool isDragging = false;

#if UNITY_STANDALONE || UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);
#endif

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (transform.parent != null)
            parentRect = transform.parent.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;



        // 隱藏滑鼠
        Cursor.visible = false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out lastMousePos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // 恢復滑鼠可見並把滑鼠移回開始拖動的位置
        Cursor.visible = true;

#if UNITY_STANDALONE || UNITY_EDITOR

        SetCursorPos(Screen.width / 2, Screen.height / 2);
#endif
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            Vector2 delta = localPoint - lastMousePos;
            rectTransform.anchoredPosition += delta;
            ClampToParent();
            lastMousePos = localPoint;
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        float scale = rectTransform.localScale.x + eventData.scrollDelta.y * zoomSpeed;
        scale = Mathf.Clamp(scale, minZoom, maxZoom);
        rectTransform.localScale = new Vector3(scale, scale, 1);
        ClampToParent();
    }

    private void ClampToParent()
    {
        if (parentRect == null) return;

        Vector2 size = rectTransform.rect.size * rectTransform.localScale.x;
        Vector2 parentSize = parentRect.rect.size;

        Vector2 minPos = (parentSize - size) / 2;
        Vector2 maxPos = -minPos;

        Vector2 pos = rectTransform.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, minPos.x, maxPos.x);
        pos.y = Mathf.Clamp(pos.y, minPos.y, maxPos.y);
        rectTransform.anchoredPosition = pos;
    }

    private void OnDisable()
    {
        // 防止遊戲中途停用造成滑鼠消失
        Cursor.visible = true;
    }
}
