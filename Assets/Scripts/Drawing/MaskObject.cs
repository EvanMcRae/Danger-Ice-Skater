using UnityEngine;

public class MaskObject : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = 3002;
    }
}
