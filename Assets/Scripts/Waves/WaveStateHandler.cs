using Game;
using UnityEngine;

namespace Waves {
    public class WaveStateHandler : MonoBehaviour {
        public GameController gc;
        public WaveSpawner ws;

        [Header("Time Values")] public float initialWaveDelayTimer = 5f;

        public bool tickTimer = false;

        public float spawnWaveTimer;

        public bool finalSubwave = false;

        //Should go:
        //Start wave -> Spawn subwave -> Wait 5 seconds (Opening subwave delay) -> Send subwave -> Spawn next subwave ->
        //                                                                      ^^ When all enemies die, send next subwave <- 
        //                                                                          If we were on the last subwave, restart the graph.


        public void StartGame() {
            //Start up
            SpawnNewWaveState();
        }

        public void Update() {
            if (PauseManager.ShouldNotRun()) return;

            //5s timer for sending enemies.
            if (tickTimer) spawnWaveTimer -= Time.deltaTime;
            if (tickTimer && spawnWaveTimer <= 0) {
                SendAndSpawnNewSubWaveState();
                tickTimer = false;
            }
        }

        public void SpawnNewWaveState() {
            //Progress the wave, progress the subwave, spawn the subwave, wait 5s to spawn them.
            if (ws.NextWave()) {
                ws.NextSubWave();
                ws.SpawnSubWave();
                spawnWaveTimer = initialWaveDelayTimer;
                tickTimer = true;
            }
            else {
                WinGame();
            }
        }

        public void SendAndSpawnNewSubWaveState() {
            //Send the enemies, spawn the next subwave if avalible.
            ws.SendSubWave();
            //ws.currentSubWave++;
            if (ws.IsNextSubWaveAvailable()) {
                ws.NextSubWave();
                ws.SpawnSubWave();
            }
            else finalSubwave = true;
        }

        public void SubWaveDone() {
            Debug.Log("Sub wave " + ws.currentSubWaveCount + " done!");
            //When finished with a subwave, send the next subwave if its avalible.
            if (!finalSubwave) { //Is there a next subwave?
                SendAndSpawnNewSubWaveState();
            }
            else {
                finalSubwave = false;
                WaveDone();
            }
        }

        public void WaveDone() {
            Debug.Log("Wave done!");
            SpawnNewWaveState();
        }

        public void WinGame() {
            Debug.Log("You Win!"); //Got through all the waves!
        }
    }
}