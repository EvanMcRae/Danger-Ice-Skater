using Unity.VisualScripting;
using UnityEngine;

namespace Enemies.SpecificTypes {
    public class ProjectileEnemy : Follower {
        
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
            base.Update();
            projectileTimer -= Time.deltaTime;
            if (projectileTimer <= 0) {
                Vector3 diff = player.transform.position - transform.position;
                diff.Normalize();
                Shoot(diff);
                projectileTimer = Random.Range(projectileDelayMin, projectileDelayMax);
            }
        }

        public void Shoot(Vector3 direction) {
            if (!canMove) return;
            float len = direction.sqrMagnitude;
            if (len > 1.01f || len < 0.99f) direction.Normalize();

            GameObject puckInstance = Instantiate(puck, projectileSpawnPosition.transform.position, Quaternion.identity);
            
            puckInstance.GetComponent<Rigidbody>().AddForce(direction * forceScalar, ForceMode.Impulse);

        }
        
    }
}