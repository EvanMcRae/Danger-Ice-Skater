using UnityEngine;

namespace Enemies.SpecificTypes {
    public class ProjectileEnemy : Enemy {
        
        [Header("Projectile Enemy Settings")]
        public float projectileDelayMin;
        public float projectileDelayMax;
        
        public float projectileTimer;

        public GameObject projectileSpawnPosition;
        public GameObject puck;

        public float forceScalar;
        
        public void Start() {
            projectileTimer = projectileDelayMax;
        }
        public void Update() {
            projectileTimer -= Time.deltaTime;
            if (projectileTimer <= 0) {
                Shoot(Vector3.forward);
                projectileTimer = Random.Range(projectileDelayMin, projectileDelayMax);
            }
        }

        public void Shoot(Vector3 direction) {
            float len = direction.sqrMagnitude;
            if (len > 1.01f || len < 0.99f) direction.Normalize();

            GameObject puckInstance = Instantiate(puck, projectileSpawnPosition.transform.position, Quaternion.identity);
            
            
            
            puckInstance.GetComponent<Rigidbody>().AddForce(direction * forceScalar);

        }
        
        
        
        public override void Behavior() {
            //throw new System.NotImplementedException();
        }
    }
}