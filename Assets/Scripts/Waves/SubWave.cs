using System;
using System.Collections.Generic;
using Enemies;

namespace Waves { 
    
    /// <summary>
    ///     A list entry that stores enemy type and count to spawn.
    /// </summary>
    [Serializable]
    public struct EnemyListEntry {
        public EnemyType enemy;
        public int count;
    }
    
    /// <summary>
    ///     A sub-wave. Represents one of the smaller waves inside the larger wave structure.
    /// </summary>
    [Serializable]
    public struct SubWave {
        /// <summary>
        ///     The list of enemies and counts.
        /// </summary>
        public List<EnemyListEntry> setEnemyTypes;
        /// <summary>
        ///     The number of credits that the waves have to buy smaller enemies with.
        /// </summary>
        public int randomEnemyCredits;
    }
}