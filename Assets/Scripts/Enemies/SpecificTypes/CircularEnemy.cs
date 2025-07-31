using UnityEngine;

namespace Enemies
{
    public class CircularEnemy : Follower
    {
        public float dashForce; //Min: 5f
        public float rangeTime;
        public float dashTime;
        public float targetRange;
        bool isAttacking = false;
        float rangeDelay;
        float dashDelay;
        public new void Start()
        {
            base.Start();
            moveTowardsPlayer = true;
            rangeDelay = rangeTime; dashDelay = dashTime;
        }
        public new void Update()
        {
            //If in range, circle player
            if ((DistanceFromPlayer() <= targetRange) && !isAttacking) {
                CirclePlayer();
                rangeDelay -= Time.deltaTime;

                //If in range for this long, attack player
                if(rangeDelay <= 0)
                {
                    isAttacking = true;
                    rangeDelay = rangeTime;
                }
            }
            //Out of range
            else
            {
                rangeDelay = rangeTime;
            }

            if (isAttacking)
                AttackPlayer();
        }
        
        public void CirclePlayer()
        {

        }

        public void AttackPlayer()
        {
            //Dash + Dash cooldown
            if (dashDelay >= dashTime - .01f) { 
                Vector3 vectorDiff = player.transform.position - transform.position;
                vectorDiff.Normalize();

                rb.AddForce(vectorDiff * dashForce, ForceMode.Impulse);
            }
            else if(dashDelay >= dashTime - 3f) moveTowardsPlayer = false;

            //Reset variables
            if (dashDelay <= 0f)
            {
                isAttacking = false;
                dashDelay = dashTime;
                moveTowardsPlayer = true;
            }
            else dashDelay -= Time.deltaTime;
        }
    }
}
