using System.Collections.Generic;
using UnityEngine;

namespace Waves {
    
    /// <summary>
    ///     Contains a list of the waves.
    /// </summary>
    [CreateAssetMenu(fileName = "Wave Manager", menuName = "Managers")]
    public class WaveManager : ScriptableObject {
        /// <summary>
        ///     The waves.
        /// </summary>
        public List<Wave> waves;
    }
}