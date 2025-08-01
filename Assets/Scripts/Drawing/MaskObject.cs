using UnityEngine;

public class MaskObject : MonoBehaviour
{
    public int level; // 0 = cutout, 1 = plane, 2 = refill

    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = 3002 + level * 2;
    }
}
