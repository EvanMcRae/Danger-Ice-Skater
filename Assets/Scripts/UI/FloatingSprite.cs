using UnityEngine;
using UnityEngine.UI;

public class FloatingSprite : MonoBehaviour
{
    public float lifetime = 1.2f;
    public float floatSpeed = 50f;
    public float wiggleAmount = 25f;
    public float fadeDuration = 1f;

    private Image image;
    private RectTransform rectTransform;
    private float timeElapsed;
    private Vector3 startPosition;
    private float wiggleFrequency;

    void Awake()
    {
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
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / lifetime;

        // Move up with wiggle
        float yOffset = floatSpeed * t;
        float xOffset = Mathf.Sin(t * Mathf.PI * wiggleFrequency) * wiggleAmount;

        rectTransform.anchoredPosition = startPosition + new Vector3(xOffset, yOffset, 0f);

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
