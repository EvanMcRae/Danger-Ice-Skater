using Game;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemies {
    public abstract class Enemy : HoleFallable
    {
        public GameEventBroadcaster gameEventBroadcaster;

        public Rigidbody rb;
        public GameObject centerOfRink;

        public bool m_isDead = false;
        public bool m_waiting; //default to true

        public Animator anim;

        public bool canMove = false;
        float moveForce = 2f;
        public bool movingAwayFromWall = false;


        public string fallSFX = "";


        public void DestroyEnemy()
        {
            if (m_isDead) return;
            m_isDead = true;

            EnemyDied();
            FloatingTextManager.instance.ShowFloatingSprite(fallPos);
            AkUnitySoundEngine.PostEvent("PointScore", PauseManager.globalWwise);

            Destroy(gameObject);
        }

        //void OnCollisionExit(Collision other)
        //{
        //    if(other.gameObject.layer == 7 && flag == false)
        //    {
        //        FindObjectOfType<FloatingTextManager>().ShowFloatingSprite(transform.position);
        //        flag = true;
        //    }
        //}

        public void Start()
        {
            SetCanMove(false); //Starting value false
            EnemySpawned();
            //anim = gameObject.GetComponent<Animator>(); //Commented out so that this can be set in inspector, hopefully this doesnt break anything 
            rb = GetComponent<Rigidbody>();
            if (!rb)
            {
                Debug.Log("No rigidbody on this object! Please attach one!");
            }
        }

        public void Update()
        {
            if (PauseManager.ShouldNotRun()) return;

            if (transform.position.y <= 0) gameObject.layer = 2;

            if (transform.position.y <= GameController.KILL_HEIGHT && !m_isDead)
            {
                WaterSplashParticles.CreateSplashParticles(transform.position.x, transform.position.z);
                AkUnitySoundEngine.PostEvent("EnemySplash", gameObject);
                DestroyEnemy();
            }
        }

        public float DistanceFromTarget(Vector3 target)
        {
            Vector2 enemyLoc = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
            Vector2 targetLoc = new Vector2(target.x, target.z);
            //print(Vector2.Distance(enemyLoc, targetLoc)); //Debug
            return Vector2.Distance(enemyLoc, targetLoc);
        }

        //Ensures that enemies that don't follow the player can't linger next to the wall
        //Moves towards the center of the rink
        public void MoveAwayFromWall()
        {
            Vector3 vectorDiff = centerOfRink.transform.position - transform.position;
            vectorDiff.Normalize();
            rb.AddForce(vectorDiff * moveForce * 10, ForceMode.Force);
        }
        public void CheckIfCloseToWall(float top, float bottom, float left, float right)
        {
            float margin = 3f;
            if (transform.position.x <= left + margin || transform.position.x >= right - margin ||
                transform.position.z >= top - margin || transform.position.z <= bottom + margin)
            {
                movingAwayFromWall = true;
            }
            else
            {
                movingAwayFromWall = false;
            }
        }

        /// <summary>
        ///     Broadcasts that the enemy was spawned to the rest of the game.
        ///     PLEASE FOR THE LOVE OF GOD CALL THIS FUNCTION WHEN THE ENEMY IS SPAWNED.
        ///     ALSO CALL IT AFTER YOU INSTANTIATE THE ENEMY OBJECT.
        /// </summary>
        public void EnemySpawned()
        {
            gameEventBroadcaster.OnEnemySpawn.Invoke(this);
        }

        public void EnemySent()
        {
            gameEventBroadcaster.OnEnemySend.Invoke(this);
        }

        /// <summary>
        ///     Broadcasts that the enemy died to the rest of the game.
        ///     PLEASE FOR THE LOVE OF GOD CALL THIS FUNCTION WHEN THE ENEMY DIES FOR WHATEVER REASON.
        ///     ALSO CALL IT BEFORE YOU DESTROY THE ENEMY.
        /// </summary>
        public void EnemyDied()
        {
            gameEventBroadcaster.OnEnemyDeath.Invoke(this);
        }

        public void SetCanMove(bool newValue)
        {
            canMove = newValue;
            m_waiting = !newValue;
            if (newValue) rb.constraints = RigidbodyConstraints.FreezeRotation;
            else rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        public new void Fall()
        {
            base.Fall();
            anim.SetTrigger("falling");
            anim.Play("fall");
            if (fallSFX != "")
                AkUnitySoundEngine.PostEvent(fallSFX, gameObject);
        }
    }
}