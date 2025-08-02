using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject floatingSpritePrefab;
    public static FloatingTextManager instance;
    public Canvas canvas;

    public void Start()
    {
        instance = this;
    }

    public void ShowFloatingSprite(Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Make instance under canvas
        GameObject instance = Instantiate(floatingSpritePrefab, canvas.transform);

        // Convert screen point to canvas local point
        Debug.Log(canvas);

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
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
