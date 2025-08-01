using System;
using Game;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.PlayerUI {
    public class WinLoseUI : MonoBehaviour {

        public GameEventBroadcaster geb;
        
        public RectTransform diePanel;

        public void ToMainMenu() {
            SceneManager.LoadScene("MainMenu");
        }

        public void OnEnable() {
            geb.OnPlayerDeathByHit.AddListener(OnDeath);
        }

        public void OnDeath(PlayerStatsHandler sh) {
            diePanel.gameObject.SetActive(true);
            
        }
    }
}