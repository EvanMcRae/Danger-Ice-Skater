using UnityEngine;
using Waves;

public class NewWaveNotif : MonoBehaviour
{
    public Animator notifAnimation;

    public void PlayNotif()
    {
        notifAnimation.Play("NewWaveNotifEnter");
    }

    private void Start()
    {
        WaveSpawner.WaveStarted += PlayNotif;
    }
}
