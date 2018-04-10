using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler {

    public RectTransform draggableObject;
    public RectTransform boundary;

    private Vector2 startMouse;
    private Vector2 startPanel;

    public void OnBeginDrag(PointerEventData eventData) {
        startMouse = eventData.position - eventData.delta;
        startPanel = (Vector2)draggableObject.localPosition;
    }

    public void OnDrag(PointerEventData eventData) {
        // TODO doesn't support not being horizontally centered
        // TODO seems to allow items to be a little too high or a little too low
        Vector2 pos = startPanel + (eventData.position - startMouse) / GetComponentInParent<Canvas>().scaleFactor;
        float width = (draggableObject.rect.width * draggableObject.localScale.x) / 2f;
        float height = (boundary.rect.height - draggableObject.rect.height * draggableObject.localScale.y) / 2f;
        
        if (pos.x - boundary.rect.x < width) {
            pos.x = boundary.rect.x + width;
        } else if (boundary.rect.x + boundary.rect.width - pos.x < width) {
            pos.x = boundary.rect.x + boundary.rect.width - width;
        }
        if (pos.y > -height) {
            pos.y = -height;
        } else if (-boundary.rect.height / 2f > pos.y) {
            pos.y = -boundary.rect.height / 2f;
        }

        draggableObject.localPosition = pos;
    }
}
