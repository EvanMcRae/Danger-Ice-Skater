using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        Bounds myBounds = gameObject.GetComponent<Collider>().bounds;
        Bounds otherBounds = other.collider.bounds;

        Vector3 min = otherBounds.min;
        min.y = myBounds.min.y;
        Vector3 max = otherBounds.max;
        max.y = myBounds.max.y;

        if (myBounds.Contains(min) && myBounds.Contains(max))
        {
            other.gameObject.GetComponent<Rigidbody>().excludeLayers = 1 << 6 | 1 << 7; // ignore ice and hole
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }
}
