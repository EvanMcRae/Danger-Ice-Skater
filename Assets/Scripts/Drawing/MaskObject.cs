using UnityEngine;

public class MaskObject : MonoBehaviour
{
    public bool lower;

    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = 3002 + (lower ? 2 : 0);
    }
}
