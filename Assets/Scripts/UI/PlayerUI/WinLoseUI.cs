using Game;
using Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

namespace UI.PlayerUI {
    public class WinLoseUI : MonoBehaviour {

        public GameEventBroadcaster geb;
        
        public RectTransform diePanel;
        public TMP_Text scoreText;

        public void ToMainMenu() {
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1;
        }

        public void OnEnable() {
            geb.OnPlayerDeathByHit.AddListener(OnDeath);
            geb.OnScoreIncreased.AddListener(ScoreIncrease);
        }

        public void OnDeath(PlayerStatsHandler sh) {
            diePanel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        public void ScoreIncrease(int newScore)
        {
            scoreText.text = newScore.ToString();
        }
    }
}