using UnityEngine;

namespace Enemies {
    public abstract class Enemy : MonoBehaviour {
        public Vector3 m_spawnLocation;
        public int m_enemyType;
        public int m_damage { get; set; }
        public int m_health { get; set; }
        public float m_speed { get; set; }

        public void DestroyEnemy()
        {
            //TODO: Play death animation
            Destroy(gameObject);
        }
        public float DistanceFromTarget(Vector3 target)
        {
            Vector2 enemyLoc = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
            Vector2 targetLoc = new Vector2(target.x, target.z);
            //print(Vector2.Distance(enemyLoc, targetLoc)); //Debug
            return Vector2.Distance(enemyLoc, targetLoc);
        }
    }
}