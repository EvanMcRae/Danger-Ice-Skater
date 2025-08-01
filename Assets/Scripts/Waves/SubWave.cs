using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine.Serialization;

namespace Waves { 
    
    /// <summary>
    ///     A list entry that stores enemy type and count to spawn.
    /// </summary>
    [Serializable]
    public class EnemyListEntry {
        public EnemyType enemy;
        public int count;

        public EnemyListEntry(EnemyType enemy, int count) {
            this.enemy = enemy;
            this.count = count;
        }
    }
    
    /// <summary>
    ///     A sub-wave. Represents one of the smaller waves inside the larger wave structure.
    /// </summary>
    [Serializable]
    public struct SubWave {
        /// <summary>
        ///     The list of enemies and counts.
        /// </summary>
        public List<EnemyListEntry> enemies;
        
        /// <summary>
        ///     The number of credits that the waves have to buy smaller enemies with.
        /// </summary>
        public int randomEnemyCredits;

        public bool IsSubWaveInSize(int maxNum) {
            int total = 0;
            foreach (EnemyListEntry e in enemies) {
                total += e.count;
            }

            if (total > maxNum) return false;
            return true;
        }
    }
}