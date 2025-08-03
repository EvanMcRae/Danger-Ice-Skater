using Enemies;
using UnityEngine;

namespace Waves {
    public class WaveSpawnPoint : MonoBehaviour {
        
        public GameObject spawnPoint;
        public GameObject relativeDirection;
        public bool dualGate;
        public GameObject gate1, gate2;

        public MeshRenderer pointRenderer;
        public MeshRenderer directionRenderer;
        
        public Enemy thisEnemy;

        public float forceScalar;

        public void Start() {
            pointRenderer.enabled = false;
            directionRenderer.enabled = false;
        }

        public void SpawnEnemyAt(Enemy e) {

            thisEnemy = Instantiate(e, spawnPoint.transform.position, Quaternion.identity);
            thisEnemy.transform.LookAt(relativeDirection.transform);
            thisEnemy.SetCanMove(false);
            thisEnemy.rb.detectCollisions = false;
        }

        public void SendEnemy() {
            Vector3 dir = relativeDirection.transform.position - spawnPoint.transform.position;
            dir.Normalize();
            thisEnemy.SetCanMove(true);
            thisEnemy.rb.AddForce(dir * forceScalar, ForceMode.Impulse);
            thisEnemy.rb.detectCollisions = true;
            thisEnemy.EnemySent();
            thisEnemy = null;

            //Open gate animation
            gate1.GetComponent<gateOpen>().opened = true;
            if (dualGate) gate2.GetComponent<gateOpen>().opened = true;
        }

    }
}