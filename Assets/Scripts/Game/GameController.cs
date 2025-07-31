using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Waves;

namespace Game {
    public class GameController : MonoBehaviour {

        public GameEventBroadcaster gameEventBroadcaster;
        public WaveSpawner waveSpawner;

        public List<Enemy> enemyList;

        public float timeBetweenWaves = 5f;
        public float waveDelayTimer = 5f;
        public bool tickDelay = true;

        public void Start() {
            waveDelayTimer = timeBetweenWaves;
        }

        public void Update() {
            if (tickDelay) {
                if (waveDelayTimer >= 0) {
                    waveDelayTimer -= Time.deltaTime;
                }
                else {
                    tickDelay = false;
                    bool nextWaveSpawned = waveSpawner.NextWave();
                    if (nextWaveSpawned) {
                        gameEventBroadcaster.OnWaveStart.Invoke(waveSpawner.currentWave);
                        bool nextSubWaveSpawned = waveSpawner.NextSubWave(true);
                        if (nextSubWaveSpawned) {
                            gameEventBroadcaster.OnSubWaveStart.Invoke(waveSpawner.currentWave, waveSpawner.currentSubWave);
                        }
                    }
                }//()
            }/*=-o                                              */
        }/*   ^^^ Sisyphus pushing the boulder up the pyramid */ 
        
        private void WaveCleared() {
            gameEventBroadcaster.OnWaveClear.Invoke(waveSpawner.currentWave);
            waveDelayTimer = timeBetweenWaves;
            tickDelay = true;
        }

        private void SubWaveCleared() {
            gameEventBroadcaster.OnSubWaveClear.Invoke(waveSpawner.currentWave, waveSpawner.currentSubWave);
            bool nextSubWaveSpawned = waveSpawner.NextSubWave(true);
            if (nextSubWaveSpawned) {
                gameEventBroadcaster.OnSubWaveStart.Invoke(waveSpawner.currentWave, waveSpawner.currentSubWave);
            }
            else {
                WaveCleared();
            }
        }

        
        /*
         * Event handlers:
         */
        
        public void OnEnable() {
            gameEventBroadcaster.OnEnemySpawn.AddListener(OnEnemySpawn);
            gameEventBroadcaster.OnEnemyDeath.AddListener(OnEnemyDeath);
        }

        public void OnDisable() {
            gameEventBroadcaster.OnEnemySpawn.RemoveListener(OnEnemySpawn);
            gameEventBroadcaster.OnEnemyDeath.RemoveListener(OnEnemyDeath);
        }
        
        private void OnEnemySpawn(Enemy e) {
            enemyList.Add(e);
        }

        private void OnEnemyDeath(Enemy e) {
            bool removed = enemyList.Remove(e);
            if (!removed) {
                Debug.LogError("Enemy that died was untracked by game. Enemy: " + e + " died.");
            }

            if (enemyList.Count == 0) SubWaveCleared();
        }
    }
}