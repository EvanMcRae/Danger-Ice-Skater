using UnityEngine;

namespace Enemies
{
    public class TrapEnemy : Follower
    {
        public float idleTime;
        public float placingRadius;
        public float idleForce;
        public float idleCooldown;
        [SerializeField] public GameObject trap;
        public GameObject top, bottom, left, right, center;
        float topBound, bottomBound, leftBound, rightBound, centerBound;

        float idleDelay;
        public float cooldownDelay;
        Vector3 targetSpot;
        public Vector3 targetSpot2;

        bool placingTrap;
        bool arrivedAtTarget = false;
        bool notMoving = false;
        
        public new void Start()
        {
            base.Start();
            idleDelay = idleTime;
            placingTrap = false;
            cooldownDelay = idleCooldown;
            targetSpot2 = GenerateTarget(placingRadius / 2);

            topBound = top.transform.position.z; bottomBound = bottom.transform.position.z;
            leftBound = left.transform.position.x; rightBound = right.transform.position.x;
            centerBound = center.transform.position.x;
        }
        public new void Update()
        {
            if (PauseManager.paused) return;

            if (!m_waiting && !m_isDead)
            {
                base.Update();
                if (idleDelay <= 0f && !placingTrap)
                {
                    placingTrap = true;
                    targetSpot = GenerateTarget(placingRadius);
                    idleDelay = idleTime;
                }

                if (placingTrap)
                {
                    anim.Play("placeTrap");
                    DropTrap();
                }
                else Idle();
            }
            else if (!m_isDead) anim.Play("idle");
        }

        //Seeks a target spot to drop a trap
        public void DropTrap()
        {
            //Pick a random spot within a radius 
            if (!arrivedAtTarget)
            {
                Vector3 vectorDiff = targetSpot - transform.position;
                vectorDiff.Normalize();
                rb.AddForce(vectorDiff * moveForce, ForceMode.Force);
                if (DistanceFromTarget(targetSpot) <= .5f)
                {
                    arrivedAtTarget = true;
                }
            }
            else
            {
                //Drop trap once movement stops
                if(Mathf.Abs(rb.linearVelocity.x) <= .05f && Mathf.Abs(rb.linearVelocity.z) <= .05f)
                {
                    Vector3 spot = transform.position;

                    if (spot.x < centerBound) spot.x += 3f;
                    else spot.x -= 3f;
                    Instantiate(trap, spot, Quaternion.identity);

                    placingTrap = false;
                    arrivedAtTarget = false;
                    cooldownDelay = 0f;
                    notMoving = true;
                }
            }
        }

        //Skates around aimlessly
        //Adds a little impulse in a random direction for idleDelay/idleTime seconds
        public void Idle()
        {
            //Move in a direction
            if (cooldownDelay > 0f && !notMoving)
            {
                anim.Play("skate");
                Vector3 vectorDiff = targetSpot2 - transform.position;
                vectorDiff.Normalize();
                rb.AddForce(vectorDiff * idleForce, ForceMode.Impulse);
                cooldownDelay -= Time.deltaTime;
            }
            else notMoving = true;

            //Standing still
            if (notMoving)
            {
                cooldownDelay += Time.deltaTime;
                anim.Play("idle");
            }

            //Ready to move again (resetting variables)
            if ((cooldownDelay >= 3) && notMoving)
            {
                notMoving = false;
                targetSpot2 = GenerateTarget(placingRadius / 2);
                cooldownDelay = idleCooldown;
            }

            idleDelay -= Time.deltaTime;
        }

        //Random X, Z coordinate within a given radius
        public Vector3 GenerateTarget(float radius)
        {
            float randX = Random.Range(transform.position.x - radius,
                                           transform.position.x + radius);
            float randZ = Random.Range(transform.position.z - radius,
                                       transform.position.z + radius);

            if (randX >= rightBound) randX = rightBound - 3;
            if (randX <= leftBound) randX = leftBound + 3;
            if (randZ >= bottomBound) randZ = bottomBound + 5;
            if (randZ <= topBound) randZ = topBound - 5;

            return new Vector3(randX, transform.position.y, randZ);
        }
    }
}
