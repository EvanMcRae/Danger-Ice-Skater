using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Waves;

namespace Game {
    public class GameController : MonoBehaviour {

        public GameEventBroadcaster gameEventBroadcaster;
        public WaveSpawner waveSpawner;
        public WaveStateHandler waveStateHandler;

        public List<Enemy> enemyList;

        public float timeBetweenWaves = 5f;
        public float waveDelayTimer = 5f;
        public bool tickDelay = true;

        public void Start() {
            waveDelayTimer = timeBetweenWaves;
            waveStateHandler.StartGame();
        }
        
        
        
        /*
         * Event handlers:
         */
        public void OnEnable() {
            gameEventBroadcaster.OnEnemySend.AddListener(OnEnemySend);
            gameEventBroadcaster.OnEnemyDeath.AddListener(OnEnemyDeath);
        }

        public void OnDisable() {
            gameEventBroadcaster.OnEnemySend.RemoveListener(OnEnemySend);
            gameEventBroadcaster.OnEnemyDeath.RemoveListener(OnEnemyDeath);
        }
        
        private void OnEnemySend(Enemy e) {
            enemyList.Add(e);
        }

        private void OnEnemyDeath(Enemy e) {
            bool removed = enemyList.Remove(e);
            if (!removed) {
                Debug.LogError("Enemy that died was untracked by game. Enemy: " + e + " died.");
            }

            if (enemyList.Count == 0) waveStateHandler.SubWaveDone();
        }
    }
}