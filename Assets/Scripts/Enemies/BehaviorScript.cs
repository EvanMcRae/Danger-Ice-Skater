using UnityEngine;

public class BehaviorScript : MonoBehaviour
{
    [SerializeField] public Enemies.EnemyType enemyType;
    public Enemies.Enemy entity;
    public Vector3 spawnPoint = Vector3.zero;

    public float speed = 1f;
    public int health = 20;
    public int damage = 5;

    public void Start()
    {
        //Enemy type is decided based on the serialized value in editor
        //This then assigns the correct class to entity
        switch (enemyType)
        {
            case Enemies.EnemyType.LINEAR:
                entity = new Enemies.LinearEnemy(spawnPoint, speed, damage, health);
                break;
            default:
                entity = new Enemies.LinearEnemy(spawnPoint, speed, damage, health);
                break;
        }
    }

    public void Update()
    {
        entity.Behavior();
    }
}
