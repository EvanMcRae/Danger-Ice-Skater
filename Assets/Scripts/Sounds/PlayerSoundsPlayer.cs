using Game;
using UnityEngine;

public class PlayerSoundsPlayer : MonoBehaviour
{
    public AK.Wwise.Event StepEvent, GlideEvent, HurtEvent, SplashEvent;
    public PlayerController playerController;
    public bool isGliding;

    public void RunStepSound()
    {
        if (PauseManager.paused) return;
        if (isGliding) return;
        StepEvent?.Post(playerController.gameObject);
    }
    public void RunGlideSound()
    {
        if (PauseManager.paused) return;
        isGliding = true;
        GlideEvent?.Post(playerController.gameObject);
    }

    public void RunHurtSound()
    {
        if (PauseManager.paused) return;
        if (playerController.isTouchingGround)
        {
            RunGlideSound();
        }
        HurtEvent?.Post(playerController.gameObject);
    }

    public void RunSplashSound()
    {
        if (PauseManager.paused) return;
        SplashEvent?.Post(playerController.gameObject);
    }
}
