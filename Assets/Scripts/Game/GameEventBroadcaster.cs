using Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    [CreateAssetMenu(fileName = "GameEventBroadcaster", menuName = "EventBroadcasters/GameEventBroadcaster")]
    public class GameEventBroadcaster : ScriptableObject {
        
        /// Params: The player that died.
        public UnityEvent<PlayerControler> OnPlayerDeath;
        
        /// Params: The wave number that was spawned.
        public UnityEvent<int> OnWaveSpawned;
        
        /// Params: The wave number that was started;
        public UnityEvent<int> OnWaveStart;
        
        /// Params: The wave number that was cleared.
        public UnityEvent<int> OnWaveClear;

        /// Params: The wave number, the sub wave number that was spawned.
        public UnityEvent<int, int> OnSubWaveSpawned;
        
        /// Params: The wave number, the sub wave number that was started.
        public UnityEvent<int, int> OnSubWaveStart;
        
        /// Params: The wave number, the sub wave number that was cleared.
        public UnityEvent<int, int> OnSubWaveClear;
        
        /// Params: The hole that was created.
        public UnityEvent<Hole> OnIceHoleCreated;
        
        /// Params: The enemy that was spawned.
        public UnityEvent<Enemy> OnEnemySpawn;

        /// Params: The enemy that was sent.
        public UnityEvent<Enemy> OnEnemySend;
        
        /// Params: The enemy that was spawned, the place where it died.
        public UnityEvent<Enemy> OnEnemyDeath;

    }
}