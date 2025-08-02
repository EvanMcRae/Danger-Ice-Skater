using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    public Transform target;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
    }
}
