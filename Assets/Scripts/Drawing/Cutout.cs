using Game;
using UnityEngine;

public class Cutout : MonoBehaviour
{
    public bool m_isDead = false;

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.paused) return;
        
        if (transform.position.y < GameController.KILL_HEIGHT && !m_isDead)
        {
            m_isDead = true;
            Destroy(gameObject);
        }
    }
}
