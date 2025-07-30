using UnityEngine;
using UnityEngine.InputSystem;

namespace Waves {
    public class WaveSpawner : MonoBehaviour {
        public WaveManager waveManagerSO;

        public int currentWave = -1;
        public int currentSubWave = -1;


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

        public bool NextSubWave() {
            if (waveManagerSO.waves.Count <= currentSubWave + 1) {
                Debug.Log("All subwaves cleared!");
                return false; //All sub-waves cleared!
            }
            currentSubWave++;
            return true;
        }

        public void SpawnSubWave() {
            Debug.Log("Spawning sub wave " + currentSubWave);
            foreach (EnemyListEntry enemySet in waveManagerSO.waves[currentWave].subWaves[currentSubWave].setEnemyTypes) {
                
                Debug.Log("Spawning " + enemySet.count + " " + enemySet.enemy + " enemies!"); 
                
                for (int i = 0; i < enemySet.count; i++) {
                    GetValidSpawnLocation();
                    //Instantiate enemy
                }
                
            }
        }

        public Vector3 GetValidSpawnLocation() {
            return Vector3.zero; //TODO: Find a valid location, after rink impl.
        }

        public void Update() {
            
            
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