using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    public Transform target;
    
    // Update is called once per frame
    void Update()
    {
        if (PauseManager.ShouldNotRun()) return;

        Vector3 targetPos = target.transform.position;
        if (target.TryGetComponent<PlayerController>(out var player))
        {
            if (PlayerController.fallThroughHole && player.transform.position.y < -1.5f)
            {
                targetPos.y = transform.position.y;
            }
        }
        transform.position = targetPos;
    }
}
