using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public FloatingSprite floatingSpritePrefab;
    public static FloatingTextManager instance;

    public void Start()
    {
        instance = this;
    }

    public void ShowFloatingSprite(Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Make instance under canvas
        FloatingSprite inst = Instantiate(floatingSpritePrefab, transform);

        // Convert screen point to canvas local point
        RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();
        RectTransform instanceRect = inst.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, null, out Vector2 anchoredPos)) {
            
            inst.worldStartPos = worldPosition;
            instanceRect.anchoredPosition = anchoredPos;
            
        }
    }

    public void OnDestroy()
    {
        instance = null;
    }
}
