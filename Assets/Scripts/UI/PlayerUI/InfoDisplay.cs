using System;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerUI {
    public class InfoDisplay : MonoBehaviour {

        public GameEventBroadcaster geb;
        
        public TMP_Text wave;
        public TMP_Text score;
        
        public void OnEnable() {
            geb.OnWaveStart.AddListener(OnWaveIncrease);
            geb.OnScoreIncreased.AddListener(OnScoreIncrease);
            // Add score gain event here.
        }

        public void OnDisable() {
            geb.OnWaveStart.RemoveListener(OnWaveIncrease);
        }

        public void OnWaveIncrease(int newWave) {
            wave.text = (newWave + 1).ToString();
        }

        public void OnScoreIncrease(int newScore) {
            score.text = newScore.ToString();
        }
    }
}