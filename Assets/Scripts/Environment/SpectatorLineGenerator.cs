using System;
using System.Collections.Generic;
using Enemies;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment {
    public class SpectatorLineGenerator : MonoBehaviour {

        public GameEventBroadcaster geb;
        
        public List<Spectator> spectatorPrefabs;
        public List<Spectator> spectators;

        public int spectatorCount;

        public float gap;

        public float timerMin;
        public float timerMax;
        
        public float amplitudeMin;
        public float amplitudeMax;

        public float excitementTimer = 0;
        public float excitementTime = 3;
        
        public void Start() {
            for (int i = 0; i < spectatorCount; i++) {
                int rand = Random.Range(0, spectatorPrefabs.Count);
                
                float timer = Random.Range(timerMin, timerMax);
                float amplitude = Random.Range(amplitudeMin, amplitudeMax);
                
                Spectator curr = Instantiate(spectatorPrefabs[rand], transform);
                curr.transform.position = transform.position + Vector3.right * (gap * i); // For some reason it uses up.
                curr.timeScalar = timer;
                curr.amplitude = amplitude;
                curr.Start();
            }
        }

        public void Update() {
            excitementTimer = excitementTimer -= Time.deltaTime;
            if (excitementTimer > 0 && (excitementTimer - Time.deltaTime) <= 0) {
                StopExcitement();
            }
        }

        public void OnEnable() {
             geb.OnEnemyDeath.AddListener(OnEnemyDeath);
             geb.OnWaveClear.AddListener(OnWaveComplete);
        }

        public void OnDisable() {
            geb.OnEnemyDeath.RemoveListener(OnEnemyDeath);
            geb.OnWaveClear.RemoveListener(OnWaveComplete);
        }

        public void OnEnemyDeath(Enemy e) {
            StartExcitement();
        }

        public void OnWaveComplete(int wave) {
            StartExcitement(true);
        }

        public void StartExcitement(bool reallyExcited = false) {
            Debug.Log("startExcitement");
            excitementTimer = excitementTime * (reallyExcited ? 2 : 1);

            foreach (Spectator currSpectator in spectators) {
                currSpectator.setExcited = true;
            }
        }

        public void StopExcitement() {
            Debug.Log("stopExcitement");
            foreach (Spectator currSpectator in spectators) {
                currSpectator.setExcited = false;
            }
        }
    }
}