using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Waves;

namespace Enemies {
    [Serializable]
    public struct EnemyMapping {
        public EnemyType enemyType;
        [FormerlySerializedAs("enemyPrefab")] public EnemyData enemyData;
    }
    
    [CreateAssetMenu(fileName = "Enemy Type Manager", menuName = "Managers")]
    public class EnemyTypeManager : ScriptableObject {

        public List<EnemyMapping> enemies;

        public EnemyData Get(EnemyType type) {
            foreach (EnemyMapping pair in enemies) {
                if (pair.enemyType == type) return pair.enemyData;
            }
            return null;
        }
    }
}