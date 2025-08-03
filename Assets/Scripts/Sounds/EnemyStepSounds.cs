using UnityEngine;

public class EnemySoundPlayer : MonoBehaviour
{
    public string stepSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Step()
    {
        AkUnitySoundEngine.PostEvent(stepSound, gameObject);
    }
}
