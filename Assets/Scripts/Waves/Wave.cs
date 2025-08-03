using System;
using System.Collections.Generic;

namespace Waves {
    
    /// <summary>
    ///     An individual wave.
    /// </summary>
    [Serializable]
    public struct Wave {
        /// <summary>
        ///     A list of sub waves that the wave has.
        /// </summary>
        public List<SubWave> subWaves;
        /// <summary>
        ///     The level that random enemies should spawn with. Enemies with a level above this cannot spawn.
        /// </summary>
        public int waveLevel;
    }
}