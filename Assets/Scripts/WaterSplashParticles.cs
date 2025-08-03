using UnityEngine;

public class WaterSplashParticles : MonoBehaviour
{
    float lifetime = 5;
    float startTime = 0;

    public static GameObject splashGameobj;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime + startTime < Time.time)
            Destroy(gameObject);
    }

    public static void CreateSplashParticles(float xPos, float zPos)
    {
        Instantiate(splashGameobj, new Vector3(xPos, -3.28f, zPos), new Quaternion());
    }
}
