using Unity.VisualScripting;
using UnityEngine;

namespace Enemies.SpecificTypes {
    public class ProjectileEnemy : RandomDirectionTraveler {
        
        [Header("Projectile Enemy Settings")]
        public float projectileDelayMin;
        public float projectileDelayMax;
        
        public float projectileTimer;
        
        public GameObject projectileSpawnPosition;
        public GameObject puck;

        public float forceScalar;

        public GameObject top, bottom, left, right;

        public new void Start() {
            base.Start();
            projectileTimer = projectileDelayMax;
        }
        public new void Update() {
            if (PauseManager.paused) return;

            if (!m_waiting && !m_isDead)
            {
                CheckIfCloseToWall(top.transform.position.z, bottom.transform.position.z, left.transform.position.x, right.transform.position.x);
                if (!movingAwayFromWall)
                {
                    base.Update();
                    projectileTimer -= Time.deltaTime;
                    if (projectileTimer <= 0)
                    {
                        Vector3 diff = player.transform.position - transform.position;
                        diff.Normalize();
                        Shoot(diff);
                        projectileTimer = Random.Range(projectileDelayMin, projectileDelayMax);
                    }
                    else anim.Play("idle");
                }
                else
                {
                    anim.Play("skate");
                    MoveAwayFromWall();
                }
            }
            else if (!m_isDead)
            {
                anim.Play("idle");
            }
        }

        public void Shoot(Vector3 direction) {
            if (!canMove) return;
            anim.Play("throwPuck");
            float len = direction.sqrMagnitude;
            if (len > 1.01f || len < 0.99f) direction.Normalize();

            GameObject puckInstance = Instantiate(puck, projectileSpawnPosition.transform.position, Quaternion.identity);
            
            puckInstance.GetComponent<Rigidbody>().AddForce(direction * forceScalar, ForceMode.Impulse);
        }
    }
}