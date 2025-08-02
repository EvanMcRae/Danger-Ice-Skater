using UnityEngine;

public class Cutout : MonoBehaviour
{
    public float m_killHeight = -10f;
    public bool m_isDead = false;

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.paused) return;
        
        if (transform.position.y < m_killHeight && !m_isDead)
        {
            m_isDead = true;
            Destroy(gameObject);
        }
    }
}
