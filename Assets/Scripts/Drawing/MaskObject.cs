using UnityEngine;

public class MaskObject : MonoBehaviour
{
    public int level; // 0 = cutout, 1 = refill, 2 = plane

    void Start()
    {
        GetComponent<Renderer>().material.renderQueue = 3004 + level * 4;
    }
}
