using Enemies.SpecificTypes;
using UnityEngine;

namespace Enemies
{
    public class TrapEnemy : RandomDirectionTraveler
    {
        public float idleTime;
        public float placingRadius;
        public float idleForce;
        public float idleCooldown;
        [SerializeField] public GameObject trap;

        private float trapTimer;
        public float trapDelay;

        public GameObject top, bottom, left, right;

        /*
        float idleDelay;
        public float cooldownDelay;

        bool placingTrap;
        bool arrivedAtTarget = false;
        bool notMoving = false;
        */
        
        public new void Start()
        {
            base.Start();
            trapTimer = 3;
            fallSFX = "FoxFall";
            //a
            /*
            idleDelay = idleTime;
            placingTrap = false;
            cooldownDelay = idleCooldown;
            */
        }
        public new void Update()
        {
            if (PauseManager.ShouldNotRun()) return;

            if (canMove && !m_isDead && !m_waiting)
            {

                base.Update();

                CheckIfCloseToWall(top.transform.position.z, bottom.transform.position.z, left.transform.position.x, right.transform.position.x);
                if (!movingAwayFromWall)
                {
                    base.Update();
                    trapTimer -= Time.deltaTime;
                    if (trapTimer <= 0)
                    {
                        if (DropTrap())
                        {
                            trapTimer = trapDelay;
                            anim.SetTrigger("moving");
                        }
                        else trapTimer = 1;
                    }
                }
                else
                {
                    MoveAwayFromWall();
                    anim.SetTrigger("moving");
                }

                transform.LookAt(new Vector3(transform.position.x + rb.linearVelocity.x, transform.position.y, transform.position.z + rb.linearVelocity.z));
            }
        }

        //Seeks a target spot to drop a trap
        public bool DropTrap()
        {
            //Vector3 trapPos = transform.position + transform.forward * 2;
            //trapPos.y -= .5f;
            Vector3 trapDir = new Vector3(Random.value * 2 - 1, 0, Random.value * 2 - 1).normalized;
            if (!Physics.Raycast(transform.position, trapDir, 5, ~0)) {
                anim.SetTrigger("trapping");
                anim.Play("placeTrap");
                
                GameObject trapInst = Instantiate(trap, transform.position +trapDir * 3 - new Vector3(0, 1.5f, 0), Quaternion.LookRotation(trapDir, Vector3.up));
                AkUnitySoundEngine.PostEvent("TrapSet", gameObject);
                trapInst.GetComponent<Rigidbody>().AddForce(dir * (Random.value * 5), ForceMode.Impulse);
                return true;
            }
            return false;
        }
        
        /*
        //Skates around aimlessly
        //Adds a little impulse in a random direction for idleDelay/idleTime seconds
        public void Idle()
        {
            //Move in a direction
            if (cooldownDelay > 0f && !notMoving)
            {
                anim.Play("skate");
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
                cooldownDelay = idleCooldown;
            }

            idleDelay -= Time.deltaTime;
        }
        */
    }
}
