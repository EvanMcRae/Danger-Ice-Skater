using UnityEngine;

public class PlayerSkate : MonoBehaviour
{
    public AK.Wwise.Event StepEvent, GlideEvent;
    public PlayerController playerController;
    public void RunStepSound()
    {
        StepEvent?.Post(playerController.gameObject);
    }
    public void RunGlideSound()
    {
        GlideEvent?.Post(playerController.gameObject);
    }
}
