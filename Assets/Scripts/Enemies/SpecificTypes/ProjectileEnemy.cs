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

        public bool nearPlayer = false;

        public new void Start() {
            base.Start();
            projectileTimer = projectileDelayMax;
            //fallSFX = "HockeyFall";
        }
        public new void Update() {
            if (PauseManager.ShouldNotRun()) return;
            
            if (canMove && !m_isDead && !m_waiting)
            {
                //CheckIfCloseToWall(top.transform.position.z, bottom.transform.position.z, left.transform.position.x, right.transform.position.x);
                base.Update();

                //Check if too close to player
                if (DistanceFromTarget(player.transform.position) <= .5f)
                {
                    nearPlayer = true;
                }
                else nearPlayer = false;

                if (nearPlayer) anim.SetTrigger("hittingPlayer");

                projectileTimer -= Time.deltaTime;
                if (projectileTimer <= 0)
                {
                    Vector3 diff = player.transform.position - transform.position;
                    diff.Normalize();
                    Shoot(diff);
                    projectileTimer = Random.Range(projectileDelayMin, projectileDelayMax);
                }
                else
                {
                    //anim.Play("idle");
                    //print("debug");
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