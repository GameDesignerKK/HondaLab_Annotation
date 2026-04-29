using UnityEngine;
using UnityEngine.EventSystems;

public class AVP_DraggableWorldUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Drag Settings")]
    public Transform objectToMove;
    public Camera eventCamera;

    [Tooltip("如果为 true，只在 UI 原本所在的平面上移动。推荐开启。")]
    public bool lockToStartPlane = true;

    private Plane dragPlane;
    private Vector3 offset;
    private bool isDragging;

    private void Awake()
    {
        if (objectToMove == null)
            objectToMove = transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        Camera cam = GetEventCamera(eventData);
        if (cam == null)
        {
            Debug.LogWarning("[AVP_DraggableWorldUI] No event camera found.");
            return;
        }

        // 用 UI 当前朝向创建一个拖拽平面
        // transform.forward 是 UI 面板的法线方向
        dragPlane = new Plane(objectToMove.forward, objectToMove.position);

        if (TryGetWorldPointOnPlane(eventData.position, cam, out Vector3 worldPoint))
        {
            offset = objectToMove.position - worldPoint;
        }
        else
        {
            offset = Vector3.zero;
        }

        Debug.Log("[AVP_DraggableWorldUI] Begin Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        Camera cam = GetEventCamera(eventData);
        if (cam == null)
            return;

        if (TryGetWorldPointOnPlane(eventData.position, cam, out Vector3 worldPoint))
        {
            objectToMove.position = worldPoint + offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        Debug.Log("[AVP_DraggableWorldUI] End Drag");
    }

    private Camera GetEventCamera(PointerEventData eventData)
    {
        if (eventCamera != null)
            return eventCamera;

        if (eventData.pressEventCamera != null)
            return eventData.pressEventCamera;

        if (eventData.enterEventCamera != null)
            return eventData.enterEventCamera;

        return Camera.main;
    }

    private bool TryGetWorldPointOnPlane(Vector2 screenPosition, Camera cam, out Vector3 worldPoint)
    {
        Ray ray = cam.ScreenPointToRay(screenPosition);

        if (dragPlane.Raycast(ray, out float distance))
        {
            worldPoint = ray.GetPoint(distance);
            return true;
        }

        worldPoint = Vector3.zero;
        return false;
    }
}
