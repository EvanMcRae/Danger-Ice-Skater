using Game;
using UnityEngine;

namespace UI {
    public class NewWaveNotif : MonoBehaviour
    {
        public Animator notifAnimation;
        public GameEventBroadcaster geb;
    
        public void PlayNotif(int a)
        {
            Debug.Log("Playing notif anim");
            if (notifAnimation != null && notifAnimation.gameObject.activeInHierarchy)
                notifAnimation.Play("NewWaveNotifEnter");
        }

        public void OnEnable() {
            geb.OnWaveClear.AddListener(PlayNotif);
        }
    
        public void OnDisable() {
            geb.OnWaveClear.RemoveListener(PlayNotif);
        }
    }
}