using System;
using System.Collections.Generic;

namespace Waves {
    [Serializable]
    public struct EnemyListEntry {
        public EnemyType enemy;
        public int count;
    }
    
    [Serializable]
    public struct SubWave {
        public List<EnemyListEntry> setEnemyTypes;
        public int randomEnemyCredits;
    }
}