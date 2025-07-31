using UnityEngine;

namespace Enemies
{
    public class TrapEnemy : Follower
    {
        public float idleTime;
        public float placingRadius;
        [SerializeField] public GameObject trap;
        float idleDelay;
        Vector3 targetSpot;
        bool placingTrap;
        bool arrivedAtTarget = false;
        
        public new void Start()
        {
            base.Start();
            idleDelay = idleTime;
            placingTrap = false;
        }
        public new void Update()
        {
            base.Update();
            if (idleDelay <= 0f && !placingTrap)
            {
                placingTrap = true;
                float randX = Random.Range(transform.position.x - placingRadius,
                                           transform.position.x + placingRadius);
                float randZ = Random.Range(transform.position.z - placingRadius,
                                           transform.position.z + placingRadius);
                targetSpot = new Vector3(randX, transform.position.y, randZ);
                idleDelay = idleTime;
            }
            
            if (placingTrap) DropTrap();
            else Idle();
        }

        public void DropTrap()
        {
            //Pick a random spot within a radius around the enemy to drop a trap
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
                if(Mathf.Abs(rb.linearVelocity.x) <= .05f && Mathf.Abs(rb.linearVelocity.z) <= .05f)
                {
                    Instantiate(trap, transform.position, Quaternion.identity);
                    placingTrap = false;
                    arrivedAtTarget = false;
                }
            }
        }

        public void Idle()
        {
            //Pick a random unoccupied space to idle
            idleDelay -= Time.deltaTime;
        }
    }
}
