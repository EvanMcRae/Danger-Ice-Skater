using UnityEngine;

namespace Enemies {
    public abstract class Enemy : MonoBehaviour {
        public Vector3 m_spawnLocation;
        public int m_enemyType;
        public int m_damage;
        public int m_health;

        public void DestroyEnemy()
        {
            //TODO: Play death animation
            Destroy(this.gameObject);
        }
        public void LinearMovement(Vector3 target)
        {

        }

        public abstract void Behavior();
    }

    public class LinearEnemy : Enemy
    {
        public LinearEnemy(Vector3 spawnLocation)
        {
            m_spawnLocation = spawnLocation;
        }

        public override void Behavior()
        {
            print("WIP");
        }
    }
}