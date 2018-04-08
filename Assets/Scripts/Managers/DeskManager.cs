using UnityEngine;

public class DeskManager : MonoBehaviour {

    public GameObject blackout;

    [Header("Rect Transform")]
    public Vector2 anchorMin;
    public Vector2 anchorMax;
    public Vector2 anchoredPosition;
    public Vector2 sizeDelta;
    public Vector2 localScale;

    private RectTransform open;
    private Vector2 savedAnchorMax;
    private Vector2 savedAnchorMin;
    private Vector2 savedAnchoredPosition;
    private Vector2 savedSizeDelta;
    private Vector2 savedLocalScale;

    public void Toggle(RectTransform rect) {
        if (open == rect) Close();
        else Inspect(rect);
    }

    public void Inspect(RectTransform rect) {
        if (open != null)
            Close();

        // Save current values
        savedAnchorMax = rect.anchorMax;
        savedAnchorMin = rect.anchorMin;
        savedAnchoredPosition = rect.anchoredPosition;
        savedSizeDelta = rect.sizeDelta;
        savedLocalScale = rect.localScale;
        open = rect;

        // Apply inspector values
        rect.anchorMax = anchorMax;
        rect.anchorMin = anchorMin;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;
        rect.localScale = localScale;

        blackout.SetActive(true);
    }

    public void Close() {
        open.anchorMin = savedAnchorMin;
        open.anchorMax = savedAnchorMax;
        open.anchoredPosition = savedAnchoredPosition;
        open.sizeDelta = savedSizeDelta;
        open.localScale = savedLocalScale;

        open = null;
        blackout.SetActive(false);
    }
}
