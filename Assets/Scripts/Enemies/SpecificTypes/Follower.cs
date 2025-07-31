using UnityEngine;

namespace Enemies {
    public class Follower : Enemy {
        
        public GameObject player;
        public Rigidbody rb;
        
        public bool rotateTowardsPlayer;
        public bool moveTowardsPlayer; //Check this to make enemy go towards player

        public float moveForce;


        public new void Start() {
            base.Start();
            player = GameObject.FindWithTag("Player");
            if (!player) Debug.Log("Player could not be found!");
            
            rb = GetComponent<Rigidbody>();
        }

        public void Update() {
            //base.Update();
            if (rotateTowardsPlayer) {
                transform.LookAt(player.transform);
            }
        }

        public void FixedUpdate() {
            Vector3 vectorDiff = player.transform.position - transform.position;
            vectorDiff.Normalize();
            if (moveTowardsPlayer) {
                rb.AddForce(vectorDiff * moveForce, ForceMode.Force);
            }
        }
        
    }
}