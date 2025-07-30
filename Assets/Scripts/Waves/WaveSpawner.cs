using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Waves {
    public class WaveSpawner : MonoBehaviour {
        public EnemyTypeManager enemyTypeManager;
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
            Debug.Log("Spawning sub wave " + currentSubWave);
            Wave wave = waveManagerSO.waves[currentWave];
            SubWave subWave = wave.subWaves[currentSubWave];
            
            List<EnemyListEntry> enemies = ConvertCreditsToWaves(wave.waveLevel, subWave.randomEnemyCredits, subWave.enemies);
            
            foreach (EnemyListEntry enemySet in enemies) {
                
                Debug.Log("Spawning " + enemySet.count + " " + enemySet.enemy + " enemies!"); 
                
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

        public List<EnemyListEntry> ConvertCreditsToWaves(int level, int credits, List<EnemyListEntry> enemyList) {
            
            //Get a list of the possible enemies we can spawn, ASSUMES THE ENEMIES IN THE ETM ARE SORTED BY LEVEL!
            List<EnemyMapping> validEnemies = new List<EnemyMapping>();
            foreach (EnemyMapping e in enemyTypeManager.enemies) {
                if (e.enemyData.enemyLevel > level) break;
                validEnemies.Add(e);
            }
            
            if (validEnemies.Count == 0) return enemyList;
            
            //Add random enemies from possible to the valid enemies list
            
            while (credits > 0) {
                //Get a random enemy
                int random = Random.Range(0, validEnemies.Count);
                
                //Subtract the credits it costs
                credits -= validEnemies[random].enemyData.creditCost;

                //Iterate over the existing list to see if it's already in there
                bool found = false;
                for (int i = 0; i < enemyList.Count; i++) {
                    if (enemyList[i].enemy == validEnemies[random].enemyType) {
                        enemyList[i].count++;
                        found = true;
                        break;
                    }
                }
                //If the enemy wasn't already added, add it to the list of enemies to spawn.
                if (!found) enemyList.Add(new EnemyListEntry(validEnemies[random].enemyType, 1));
                
            }

            return enemyList;
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