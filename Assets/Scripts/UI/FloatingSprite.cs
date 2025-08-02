using UnityEngine;
using UnityEngine.UI;

public class FloatingSprite : MonoBehaviour
{
    public float lifetime = 1.2f;
    public float floatSpeed = 50f;
    public float wiggleAmount = 25f;
    public float fadeDuration = 1f;

    public Vector3 worldStartPos;
    public RectTransform canvasRect;
    
    
    private Image image;
    private RectTransform rectTransform;
    private float timeElapsed;
    private Vector3 startPosition;
    private float wiggleFrequency;

    void Awake() {
        canvasRect = transform.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>();
        image = GetComponent<Image>();
        //rectTransform = GetComponent<RectTransform>();
        //startPosition = rectTransform.anchoredPosition;
        wiggleFrequency = Random.Range(2f, 6f); // Random wiggle speed
    }

    void Start()
    {
        // Set random tint color
        image.color = new Color(Random.value, Random.value, Random.value, 1f);
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;

    }

    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldStartPos);


        bool valid = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null,
            out Vector2 anchoredPos);
        if (!valid) {
            return;
        }
        
        
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / lifetime;

        // Move up with wiggle
        float yOffset = floatSpeed * t;
        float xOffset = Mathf.Sin(t * Mathf.PI * wiggleFrequency) * wiggleAmount;

        rectTransform.anchoredPosition = anchoredPos + new Vector2(xOffset, yOffset);

        // Fade out
        float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
        Color c = image.color;
        image.color = new Color(c.r, c.g, c.b, alpha);

        // Destroy after lifetime
        if (timeElapsed >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
