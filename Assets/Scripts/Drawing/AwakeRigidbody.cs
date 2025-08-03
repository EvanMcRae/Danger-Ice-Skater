using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AwakeRigidbody : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0;
    }
}
