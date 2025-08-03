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
        
        public new void Start() {
            base.Start();
            projectileTimer = projectileDelayMax;
        }
        public new void Update() {
            if (PauseManager.ShouldNotRun()) return;

            if (!m_waiting && !m_isDead)
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
            else if (!m_isDead)
            {
                anim.Play("idle");
                print("HELP!!!");
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