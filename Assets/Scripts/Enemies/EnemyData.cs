using UnityEngine;

namespace Enemies {
    [CreateAssetMenu(fileName = "EnemyData")]
    public class EnemyData : ScriptableObject {
        public Enemy enemy;
        public EnemyType enemyType;

        [Header("Enemy Data")] 
        public int creditCost;
        public int enemyLevel;
    }
}