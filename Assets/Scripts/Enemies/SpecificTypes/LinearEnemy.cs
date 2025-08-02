using UnityEngine;

namespace Enemies
{
    public class LinearEnemy : Follower
    {
        public new void Start()
        {
            base.Start();
            //moveTowardsPlayer = true;
        }
        public new void Update()
        {
            if (PauseManager.paused) return;

            if (m_waiting && !m_isDead) anim.SetBool("Idle", true);
            else if (!m_isDead)
            {
                base.Update();
                anim.SetBool("Idle", false);
            }
        }
    }
}
