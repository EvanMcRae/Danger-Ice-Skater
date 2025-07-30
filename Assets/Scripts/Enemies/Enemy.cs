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
        public void LinearMovement(Vector3 target)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target, m_speed);
        }

        public abstract void Behavior();
    }
}