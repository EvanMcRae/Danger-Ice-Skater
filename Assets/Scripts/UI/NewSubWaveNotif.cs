using Game;
using UnityEngine;
using Waves;

public class NewSubWaveNotif : MonoBehaviour
{
    public Animator notifAnimation;
    public GameEventBroadcaster geb;
    
    public void PlayNotif(int a, int b)
    {
        Debug.Log("Playing notif anim");
        if (notifAnimation != null && notifAnimation.gameObject.activeInHierarchy)
            notifAnimation.Play("NewSubwaveNotifEnter");
    }

    public void OnEnable() {
        geb.OnSubWaveStart.AddListener(PlayNotif);
    }
    
    public void OnDisable() {
        geb.OnSubWaveStart.RemoveListener(PlayNotif);
    }
}
