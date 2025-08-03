using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.SpecificTypes {
    public class RandomDirectionTraveler : Enemy {
        public GameObject player;
        
        public bool rotateTowardsPlayer;
        public bool move; //Check this to make enemy go away from player
        public bool pushPlayerOnCollision;
        public float pushForce;

        public Vector3 dir;

        public float moveForce;

        public float directionChangeTime;
        private float directionChangeTimer;


        public new void Start() {
            base.Start();
            player = GameObject.FindWithTag("Player");
            if (!player) Debug.Log("Player could not be found!");
        }

        public new void Update() {
            if (PauseManager.ShouldNotRun()) return;
            
            base.Update();
            
            directionChangeTimer -= Time.deltaTime;
            if (directionChangeTimer <= 0) {
                directionChangeTimer = directionChangeTime; 
                RandomDir();
            }
            if (rotateTowardsPlayer) {
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.up);
            }
        }

        public void RandomDir() {
            dir = new Vector3(Random.value * 2 - 1, 0, Random.value * 2 - 1).normalized;
            if (Physics.Raycast(transform.position, dir, 2, 0)) {
                dir *= -1;
            }
        }

        public void FixedUpdate() {
            if (PauseManager.ShouldNotRun()) return;
            
            if (move) {
                rb.AddForce(dir * moveForce, ForceMode.Force);
            }
        }

        public void OnCollisionEnter(Collision collision) {
            RandomDir();
            
            if (pushPlayerOnCollision && collision.gameObject.CompareTag("Player")) {
                Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

                Vector3 playerDir = collision.transform.position - transform.position;
                Vector3 fixedDir = new Vector3(playerDir.x, 0, playerDir.z);
                fixedDir.Normalize();
                
                playerRb.AddForce(fixedDir * pushForce, ForceMode.Impulse);
                rb.AddForce(fixedDir * -1 * pushForce, ForceMode.Impulse);
            }
        }
    }
}