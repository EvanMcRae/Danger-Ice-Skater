using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("what and why");
        collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
        collision.gameObject.GetComponent<Rigidbody>().excludeLayers = 1 << 6;
    }

    private void OnTriggerEnter(Collider collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
        //collision.gameObject.GetComponent<Collider>().excludeLayers = LayerMask.NameToLayer("Hole");
    }
}
