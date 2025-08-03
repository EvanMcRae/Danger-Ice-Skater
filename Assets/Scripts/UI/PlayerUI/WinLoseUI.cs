using Game;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.PlayerUI {
    public class WinLoseUI : MonoBehaviour {

        public GameObject menuButton;
        public GameEventBroadcaster geb;
        
        public RectTransform diePanel;
        public TMP_Text scoreText;

        public static bool lostGame;

        public void ToMainMenu()
        {
            ScreenWipe.current.WipeIn();
            ScreenWipe.current.PostWipe += GoToMainMenu;
        }

        public void GoToMainMenu()
        {
            ScreenWipe.current.PostWipe -= GoToMainMenu;
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1;
            lostGame = false;
        }

        public void Restart()
        {
            ScreenWipe.current.WipeIn();
            ScreenWipe.current.PostWipe += ToRestart;
        }

        private void ToRestart()
        {
            ScreenWipe.current.PostWipe -= ToRestart;
            SceneManager.LoadScene("GameScene");
            Time.timeScale = 1;
            lostGame = false;
        }


        public void OnEnable()
        {
            geb.OnPlayerDeathByHit.AddListener(OnDeath);
            geb.OnScoreIncreased.AddListener(ScoreIncrease);
        }

        public void OnDeath(PlayerStatsHandler sh)
        {
            diePanel.gameObject.SetActive(true);
            Time.timeScale = 0;

            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(menuButton);

            lostGame = true;
            AkUnitySoundEngine.PostEvent("GameOver", PauseManager.globalWwise);
        }

        public void ScoreIncrease(int newScore)
        {
            scoreText.text = newScore.ToString();
        }
    }
}