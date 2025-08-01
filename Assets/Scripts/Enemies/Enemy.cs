using Game;
using UnityEngine;

namespace Enemies {
    public abstract class Enemy : MonoBehaviour {
        public GameEventBroadcaster gameEventBroadcaster;
        
        public Vector3 m_spawnLocation;
        public int m_enemyType;
        public int m_damage { get; set; }
        public int m_health { get; set; }
        public float m_speed { get; set; }

        public float m_killHeight = -10f;
        public bool m_isDead = false;
        public bool m_waiting; //default to true

        public Animator anim;
        float waitToDie = 0f;

        public void DestroyEnemy()
        {
            //if (m_isDead) return;
            m_isDead = true;

            if(waitToDie == 0) EnemyDied();
            anim.Play("fall");

            //Allow time for the enemy to fall through the ice
            waitToDie += Time.deltaTime;
            if (waitToDie >= 5)
                Destroy(gameObject);
        }
        
        public void Start() {
            EnemySpawned();
            anim = gameObject.GetComponent<Animator>();
            m_waiting = false; //TODO: change to true when implemented with waves
        }

        public void Update()
        {
            if (transform.position.y <= m_killHeight)
            {
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
        
        /// <summary>
        ///     Broadcasts that the enemy was spawned to the rest of the game.
        ///     PLEASE FOR THE LOVE OF GOD CALL THIS FUNCTION WHEN THE ENEMY IS SPAWNED.
        ///     ALSO CALL IT AFTER YOU INSTANTIATE THE ENEMY OBJECT.
        /// </summary>
        public void EnemySpawned() {
            gameEventBroadcaster.OnEnemySpawn.Invoke(this);
        }
        
        /// <summary>
        ///     Broadcasts that the enemy died to the rest of the game.
        ///     PLEASE FOR THE LOVE OF GOD CALL THIS FUNCTION WHEN THE ENEMY DIES FOR WHATEVER REASON.
        ///     ALSO CALL IT BEFORE YOU DESTROY THE ENEMY.
        /// </summary>
        public void EnemyDied() {
            gameEventBroadcaster.OnEnemyDeath.Invoke(this);
        }
    }
}