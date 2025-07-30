using UnityEngine;
using UnityEngine.InputSystem;

namespace Waves {
    public class WaveSpawner : MonoBehaviour {
        public WaveManager waveManagerSO;

        /// <summary>
        ///     The current wave index.
        /// </summary>
        public int currentWave = -1;
        
        /// <summary>
        ///     The current sub wave index.
        /// </summary>
        public int currentSubWave = -1;


        /// <summary>
        ///     Progresses the spawner to the next wave. Does not do additional checks.
        /// </summary>
        /// <returns>True if the wave was properly progressed, else false.</returns>
        public bool NextWave() {
            if (waveManagerSO.waves.Count <= currentWave + 1) {
                Debug.Log("All waves cleared!");
                return false; //All waves cleared!
            }
            currentWave++;
            if (waveManagerSO.waves[currentWave].subWaves.Count == 0) {
                Debug.Log("This wave didn't have any subwaves... What?");
                return false; //No subwaves?
            }
            
            Debug.Log("Next wave!" + currentWave);
            currentSubWave = -1;
            return true;
        }

        /// <summary>
        ///     Progresses the spawner to the next sub wave. Does not do additional checks.
        /// </summary>
        /// <returns>True if the sub wave was properly progressed, else false.</returns>
        public bool NextSubWave() {
            if (waveManagerSO.waves.Count <= currentSubWave + 1) {
                Debug.Log("All subwaves cleared!");
                return false; //All sub-waves cleared!
            }
            currentSubWave++;
            return true;
        }

        /// <summary>
        ///     Spawns the enemies in the current sub wave.
        /// </summary>
        public void SpawnSubWave() {
            //Debug.Log("Spawning sub wave " + currentSubWave);
            foreach (EnemyListEntry enemySet in waveManagerSO.waves[currentWave].subWaves[currentSubWave].setEnemyTypes) {
                
                //Debug.Log("Spawning " + enemySet.count + " " + enemySet.enemy + " enemies!"); 
                
                for (int i = 0; i < enemySet.count; i++) {
                    GetValidSpawnLocation();
                    //TODO: Instantiate the enemy.
                }
                
            }
        }

        /// <summary>
        ///     Gets a random valid spawn location for enemies.
        /// </summary>
        /// <returns>The spawn position.</returns>
        public Vector3 GetValidSpawnLocation() {
            return Vector3.zero; //TODO: Find a valid location, after rink impl.
        }

        public void Update() {
            
            // Debug
            if (Keyboard.current != null)
            {
                if (Keyboard.current.spaceKey.wasPressedThisFrame) {
                    NextWave();
                }

                if (Keyboard.current.backspaceKey.wasPressedThisFrame) {
                    if (NextSubWave()) SpawnSubWave();
                }
            }

        }
    }
}