using UnityEngine;

namespace Enemies {
    public class EnemyData : ScriptableObject {
        public Enemy enemy;
        public EnemyType enemyType;

        [Header("Enemy Data")] 
        public int creditCost;
        public int enemyLevel;
    }
}