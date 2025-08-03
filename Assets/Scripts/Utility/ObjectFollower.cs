using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    public Transform target;
    
    // Update is called once per frame
    void Update()
    {
        if (PauseManager.ShouldNotRun()) return;

        if (target.TryGetComponent<PlayerController>(out var player))
        {
            if (PlayerController.fallThroughHole && player.transform.position.y < -1.5f) return;
        }
        transform.position = target.transform.position;
    }
}
