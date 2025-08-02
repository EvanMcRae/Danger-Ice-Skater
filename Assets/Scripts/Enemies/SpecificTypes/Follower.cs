using UnityEngine;

namespace Enemies {
    public class Follower : Enemy {
        
        public GameObject player;
        
        public bool rotateTowardsPlayer;
        public bool moveTowardsPlayer; //Check this to make enemy go towards player
        public bool pushPlayerOnCollision;
        public float pushForce;

        public float moveForce;


        public new void Start() {
            base.Start();
            player = GameObject.FindWithTag("Player");
            if (!player) Debug.Log("Player could not be found!");
        }

        public new void Update() {
            if (PauseManager.paused) return;

            base.Update();
            if (rotateTowardsPlayer) {
                transform.LookAt(player.transform);
            }
        }

        public void FixedUpdate() {
            if (PauseManager.paused) return;

            Vector3 vectorDiff = player.transform.position - transform.position;
            vectorDiff.Normalize();
            if (moveTowardsPlayer) {
                rb.AddForce(vectorDiff * moveForce, ForceMode.Force);
            }
        }

        public void OnCollisionEnter(Collision collision) {
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