using UnityEngine;

public class BehaviorScript
{
    [SerializeField] public int enemyType;

    public void Start()
    {
        switch (enemyType)
        {
            case: Waves.EnemyType.LINEAR:
                break;
            
        }

    }

    public void Update()
    {

    }
}
