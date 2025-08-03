using UnityEngine;

namespace Enemies
{
    public class CircularEnemy : Follower
    {
        public float dashForce;
        public float minCircleForce;
        public float rangeTime;
        public float dashTime;
        public float targetRange;
        public bool clockwise;

        bool isAttacking = false;
        float rangeDelay;
        float dashDelay;
        float circleForce;

        public new void Start()
        {
            fallSFX = "SharkFall";
            base.Start();
            moveTowardsPlayer = true;
            rotateTowardsPlayer = true;
            rangeDelay = rangeTime; dashDelay = dashTime;
            circleForce = minCircleForce;
            AkUnitySoundEngine.PostEvent("SharkSpawn", gameObject);
        }
        public new void Update()
        {
            if (PauseManager.ShouldNotRun()) return;

            if (!m_waiting && !m_isDead)
            {
                base.Update();

                //If in range, circle player
                if ((DistanceFromTarget(player.transform.position) <= targetRange) && !isAttacking)
                {
                    CirclePlayer();
                    rangeDelay -= Time.deltaTime;

                    //If in range for this long, attack player
                    if (rangeDelay <= 0)
                    {
                        isAttacking = true;
                        rangeDelay = rangeTime;
                    }
                    else if (rangeDelay <= 1f)
                    {
                        circleForce = circleForce / 2; //Slows down circular force
                    }
                }
                //Out of range
                else
                {
                    circleForce = minCircleForce;
                    rangeDelay = rangeTime;
                }

                if (isAttacking)
                    AttackPlayer();
            }
        }

        public void CirclePlayer()
        {
            //Add force in the direction perpendicular to the movement direction
            //If it always tries to face the player, just always add force to the right or left
            if (clockwise) rb.AddForce(transform.right * -circleForce, ForceMode.Force);
            else rb.AddForce(transform.right * circleForce, ForceMode.Force);
        }

        public void AttackPlayer()
        {
            //Dash + Dash cooldown
            if (dashDelay >= dashTime - .01f)
            {
                anim.Play("dash");
                anim.SetTrigger("dashing");
                Vector3 vectorDiff = player.transform.position - transform.position;
                vectorDiff.Normalize();

                rb.AddForce(vectorDiff * dashForce, ForceMode.Impulse);
            }
            else if (dashDelay >= dashTime - 3f) moveTowardsPlayer = false;

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
