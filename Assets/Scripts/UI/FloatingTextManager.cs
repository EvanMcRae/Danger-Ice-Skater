using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject floatingSpritePrefab; 
    public Canvas canvas;

    public void ShowFloatingSprite(Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Make instance under canvas
        GameObject instance = Instantiate(floatingSpritePrefab, canvas.transform);

        // Convert screen point to canvas local point
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform instanceRect = instance.GetComponent<RectTransform>();

        Vector2 anchoredPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, null, out anchoredPos))
        {
            instanceRect.anchoredPosition = anchoredPos;
        }
    }


}
