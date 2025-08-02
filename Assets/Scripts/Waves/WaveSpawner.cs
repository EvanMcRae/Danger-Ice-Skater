using System.Collections.Generic;
using Enemies;
using Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Waves {
    public class WaveSpawner : MonoBehaviour {
        public EnemyTypeManager enemyTypeManager;
        public WaveManager waveManagerSO;
        public GameEventBroadcaster geb;

        public List<WaveSpawnPoint> validWaveSpawnPoints;
        public List<WaveSpawnPoint> invalidWaveSpawnPoints;

        public Wave currentWave;
        
        /// <summary>
        ///     The current wave index.
        /// </summary>
        public int currentWaveCount = -1;
        
        /// <summary>
        ///     The current sub wave index.
        /// </summary>
        public int currentSubWaveCount = -1;
        
        /// <summary>
        ///     Progresses the spawner to the next wave. Does not do additional checks.
        /// </summary>
        /// <returns>True if the wave was properly progressed, else false.</returns>
        public bool NextWave() {
            if (waveManagerSO.staticWaves.Count <= currentWaveCount + 1) {
                currentWave = waveManagerSO.GenerateWave(currentSubWaveCount);
            }
            else {
                currentWave = waveManagerSO.staticWaves[currentWaveCount];
            }
            currentWaveCount++;
            geb.OnWaveStart.Invoke(currentWaveCount);
            if (currentWave.subWaves.Count == 0) {
                Debug.Log("This wave didn't have any subwaves... What?");
                return false; //No subwaves?
            }
            
            Debug.Log("Next wave!" + currentWaveCount);
            currentSubWaveCount = -1;
            return true;
        }

        /// <summary>
        ///     Progresses the spawner to the next sub wave. Does not do additional checks.
        ///     <param name="spawnEnemies">Whether the sub wave progression should spawn enemies.</param>
        /// </summary>
        /// <returns>True if the sub wave was properly progressed, else false.</returns>
        public bool NextSubWave(bool spawnEnemies = false) {
            if (currentWave.subWaves.Count <= currentSubWaveCount + 1) {
                Debug.Log("All subwaves cleared!");
                return false; //All sub-waves cleared!
            }
            currentSubWaveCount++;
            if (spawnEnemies) SpawnSubWave();
            return true;
        }

        /// <summary>
        ///     Spawns the enemies in the current sub wave.
        /// </summary>
        public void SpawnSubWave() {
            Debug.Log("Spawning sub wave " + currentSubWaveCount);
            Wave wave = currentWave;
            SubWave subWave = wave.subWaves[currentSubWaveCount];

            bool valid = subWave.IsSubWaveInSize(validWaveSpawnPoints.Count); //NOTE THIS WORKS BECAUSE ALL ENEMIES SHOULD BE DEAD (and all spawn points valid) FOR THIS TO BE CALLED!
            if (!valid) {
                Debug.Log("Subwave is too large for this rink! It will be trimmed!");
            } 
            
            List<EnemyListEntry> enemies = ConvertCreditsToWaves(wave.waveLevel, subWave.randomEnemyCredits, subWave.enemies);
            
            foreach (EnemyListEntry enemySet in enemies) {
                
                Debug.Log("Spawning " + enemySet.count + " " + enemySet.enemy + " enemies!"); 
                
                for (int i = 0; i < enemySet.count; i++) {
                    WaveSpawnPoint point = GetValidSpawnLocation();
                    if (!point) Debug.Log("Spawn point was null! What?");
                    else {
                        point.SpawnEnemyAt(enemyTypeManager.Get(enemySet.enemy).enemy);
                        validWaveSpawnPoints.Remove(point);
                        invalidWaveSpawnPoints.Add(point);
                    }
                }
            }
        }

        public void SendSubWave() {
            Debug.Log("Sending sub wave " + currentSubWaveCount);
            foreach (WaveSpawnPoint spawnPoint in invalidWaveSpawnPoints) {
                spawnPoint.SendEnemy();
                validWaveSpawnPoints.Add(spawnPoint);
            }
            invalidWaveSpawnPoints.Clear();
        }

        /// <summary>
        ///     Gets a random valid spawn location for enemies.
        /// </summary>
        /// <returns>The spawn position.</returns>
        public WaveSpawnPoint GetValidSpawnLocation() {
            if (validWaveSpawnPoints.Count != 0) return validWaveSpawnPoints[^1];
            return null;
        }

        public List<EnemyListEntry> ConvertCreditsToWaves(int level, int credits, List<EnemyListEntry> enemyList) {
            
            //Get a list of the possible enemies we can spawn, ASSUMES THE ENEMIES IN THE ETM ARE SORTED BY LEVEL!
            List<EnemyMapping> validEnemies = new List<EnemyMapping>();
            foreach (EnemyMapping e in enemyTypeManager.enemies) {
                if (e.enemyData.enemyLevel > level) break;
                validEnemies.Add(e);
            }
            
            if (validEnemies.Count == 0) return enemyList;
            
            List<EnemyListEntry> enemyListCopy = new List<EnemyListEntry>(enemyList);
            
            //Add random enemies from possible to the valid enemies list
            int enemiesSpawned = 0;
            while (credits > 0 && enemiesSpawned < validWaveSpawnPoints.Count) {
                //Get a random enemy
                int random = Random.Range(0, validEnemies.Count);

                //Remove the enemy trying to be selected if the enemy is too expensive.
                if (credits < validEnemies[random].enemyData.creditCost) {
                    validEnemies.RemoveAt(random);
                    continue;
                }
                //Subtract the credits it costs
                credits -= validEnemies[random].enemyData.creditCost;

                //Iterate over the existing list to see if it's already in there
                bool found = false;
                for (int i = 0; i < enemyListCopy.Count; i++) {
                    if (enemyListCopy[i].enemy == validEnemies[random].enemyType) {
                        enemyListCopy[i].count++;
                        found = true;
                        break;
                    }
                }
                //If the enemy wasn't already added, add it to the list of enemies to spawn.
                if (!found) enemyListCopy.Add(new EnemyListEntry(validEnemies[random].enemyType, 1));
                
                enemiesSpawned++;
            }

            return enemyListCopy;
        }
        
        public bool IsNextSubWaveAvailable() {
            return currentSubWaveCount + 1 < currentWave.subWaves.Count;
        }
    }
}