using Game;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemies {
    public abstract class Enemy : MonoBehaviour {
        public GameEventBroadcaster gameEventBroadcaster;

        public Rigidbody rb;

        public float m_killHeight = -10f;
        public bool m_isDead = false;

        public bool canMove = false;

        public void DestroyEnemy()
        {
            if (m_isDead) return;
            m_isDead = true;
            EnemyDied();
            //TODO: Play death animation
            Destroy(gameObject);
        }
        
        public void Start() {
            SetCanMove(false);
            EnemySpawned();
            rb = GetComponent<Rigidbody>();
            if (!rb) {
                Debug.Log("No rigidbody on this object! Please attach one!");
            }
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

        public void EnemySent() {
            gameEventBroadcaster.OnEnemySend.Invoke(this);
        }
        
        /// <summary>
        ///     Broadcasts that the enemy died to the rest of the game.
        ///     PLEASE FOR THE LOVE OF GOD CALL THIS FUNCTION WHEN THE ENEMY DIES FOR WHATEVER REASON.
        ///     ALSO CALL IT BEFORE YOU DESTROY THE ENEMY.
        /// </summary>
        public void EnemyDied() {
            gameEventBroadcaster.OnEnemyDeath.Invoke(this);
        }

        public void SetCanMove(bool newValue) {
            canMove = newValue;
            if (newValue) rb.constraints = RigidbodyConstraints.FreezeRotation;
            else rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}