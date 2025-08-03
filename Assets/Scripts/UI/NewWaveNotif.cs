using UnityEngine;
using Waves;

public class NewWaveNotif : MonoBehaviour
{
    public Animator notifAnimation;

    public void PlayNotif()
    {
        Debug.Log("Playing notif anim");
        notifAnimation.Play("NewWaveNotifEnter");
    }

    private void Start()
    {
        WaveSpawner.WaveStarted += PlayNotif;
    }
}
