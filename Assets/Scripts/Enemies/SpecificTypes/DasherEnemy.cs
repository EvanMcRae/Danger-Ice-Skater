using UnityEngine;

namespace Enemies.SpecificTypes {
    public class DasherEnemy : Follower {

        public float dashTimerMin;
        public float dashTimerMax;
        public float dashDelay;

        public float dashForce;
        public new void Start() {
            base.Start();
            dashDelay = Random.Range(dashTimerMin, dashTimerMax);
        }

        public new void Update() {
            dashDelay -= Time.deltaTime;
            if (dashDelay <= 0) {
                moveTowardsPlayer = false;
            }

            if (dashDelay <= -1) {
                Dash();
                moveTowardsPlayer = true;
                dashDelay = Random.Range(dashTimerMin, dashTimerMax);
            }
        }

        public void Dash() {
            Vector3 vectorDiff = player.transform.position - transform.position;
            vectorDiff.Normalize();
            
            rb.AddForce(vectorDiff * dashForce, ForceMode.Impulse);
        }


    }
}