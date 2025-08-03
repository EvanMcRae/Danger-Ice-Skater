using Game;
using UnityEngine;

public class PlayerSoundsPlayer : MonoBehaviour
{
    public AK.Wwise.Event StepEvent, GlideEvent, HurtEvent, SplashEvent;
    public PlayerController playerController;
    public bool isGliding;

    public void RunStepSound()
    {
        if (PauseManager.ShouldNotRun()) return;
        if (isGliding) return;
        if (!playerController.isTouchingGround || PlayerController.fallThroughHole || playerController.psh.invincibilityTimer > 0) return;
        StepEvent?.Post(playerController.gameObject);
    }
    public void RunGlideSound()
    {
        if (PauseManager.ShouldNotRun()) return;
        if (!playerController.isTouchingGround || PlayerController.fallThroughHole || isGliding) return;
        isGliding = true;
        GlideEvent?.Post(playerController.gameObject);
    }

    public void RunHurtSound()
    {
        if (PauseManager.ShouldNotRun()) return;
        if (playerController.isTouchingGround && !isGliding)
        {
            RunGlideSound();
        }
        HurtEvent?.Post(playerController.gameObject);
    }

    public void RunSplashSound()
    {
        if (PauseManager.ShouldNotRun()) return;
        SplashEvent?.Post(playerController.gameObject);
    }
}
