using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject floatingSpritePrefab;
    public static FloatingTextManager instance;

    public void Start()
    {
        instance = this;
    }

    public void ShowFloatingSprite(Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Make instance under canvas
        GameObject instance = Instantiate(floatingSpritePrefab, transform);

        // Convert screen point to canvas local point
        RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();
        RectTransform instanceRect = instance.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, null, out Vector2 anchoredPos))
        {
            instanceRect.anchoredPosition = anchoredPos;
        }
    }

    public void OnDestroy()
    {
        instance = null;
    }
}
