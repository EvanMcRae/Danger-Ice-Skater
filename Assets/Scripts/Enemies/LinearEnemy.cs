using UnityEngine;

namespace Enemies
{
    public class LinearEnemy : Enemy
    {
        public void Start()
        {
            m_speed = .005f;
            m_damage = 20;
            m_health = 20;
        }
        public void Update()
        {
            Behavior();
        }

        public LinearEnemy(Vector3 spawnLocation, float speed, int damage, int health)
        {
            m_spawnLocation = spawnLocation;
            m_speed = speed; m_damage = damage; m_health = health;
        }

        public override void Behavior()
        {
            //print(m_speed);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //LinearMovement(player.transform.position);
        }
    }
}
