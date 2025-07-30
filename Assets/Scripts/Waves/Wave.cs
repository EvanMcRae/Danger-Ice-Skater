using System;
using System.Collections.Generic;

namespace Waves {
    
    /// <summary>
    ///     An individual wave.
    /// </summary>
    [Serializable]
    public struct Wave {
        /// <summary>
        ///     A list of sub waves that the enemy has.
        /// </summary>
        public List<SubWave> subWaves;
    }
}