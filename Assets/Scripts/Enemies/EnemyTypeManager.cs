using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Waves;

namespace Enemies {
    [Serializable]
    public struct EnemyMapping {
        public EnemyType enemyType;
        public Enemy enemyPrefab;
    }
    
    [CreateAssetMenu(fileName = "Enemy Type Manager")]
    public class EnemyTypeManager : ScriptableObject {

        public List<EnemyMapping> enemies;

        public Enemy Get(EnemyType type) {
            foreach (EnemyMapping pair in enemies) {
                if (pair.enemyType == type) return pair.enemyPrefab;
            }
            return null;
        }
    }
}