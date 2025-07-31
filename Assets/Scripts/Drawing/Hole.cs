using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Bounds myBounds = gameObject.GetComponent<MeshCollider>().bounds;
        Bounds otherBounds = other.gameObject.GetComponent<Collider>().bounds;

        Vector3 min = otherBounds.min;
        min.y = myBounds.min.y;
        Vector3 max = otherBounds.max;
        max.y = myBounds.max.y;

        if (myBounds.Contains(min) && myBounds.Contains(max))
        {
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
