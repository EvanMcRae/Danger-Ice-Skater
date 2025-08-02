using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Waves {
    
    /// <summary>
    ///     Contains a list of the waves.
    /// </summary>
    [CreateAssetMenu(fileName = "Wave Manager", menuName = "Managers")]
    public class WaveManager : ScriptableObject {
        /// <summary>
        ///     The waves.
        /// </summary>
        public List<Wave> staticWaves;
        
        
        public Wave GenerateWave(int lastWave) {

            Wave w = new Wave {
                subWaves = new List<SubWave>(),
                waveLevel = 100
            };
            for (int i = 0; i < 3; i++) {
                SubWave s = new SubWave {
                    enemies = new List<EnemyListEntry>(),
                    randomEnemyCredits = 100000 //Makes it possible for any enemy to spawn.
                };
                w.subWaves.Add(s);
            }

            return w;
        }
    }
}