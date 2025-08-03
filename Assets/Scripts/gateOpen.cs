using UnityEngine;

public class gateOpen : MonoBehaviour
{
    public bool opened = false;
    public bool open90;
    float timer = 0f;
    float openTime = 1f;
    float openAmount = 1f;
    bool closing = false;

    bool playedCloseSound = false, playedOpenSound = false;

    float baseValue;

    public void Start()
    {
        baseValue = transform.localEulerAngles.y;
    }

    public void Update()
    {
        if (PauseManager.ShouldNotRun()) return;

        if (opened) Open();
        if (closing) Close();
        
        if(timer >= openTime)
        {
            opened = false;
            playedOpenSound = false;
            timer = 0;
            closing = true;
        }
    }
    public void Open()
    {
        if (!playedOpenSound)
        {
            AkUnitySoundEngine.PostEvent("GatesOpen", gameObject);
            playedOpenSound = true;
        }
        timer += Time.deltaTime;

        if (open90)
        {
            if (transform.localEulerAngles.y < baseValue + 90)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + openAmount, transform.localEulerAngles.z);
            }
        }
        else
        {
            if (transform.localEulerAngles.y > 360 - (baseValue + 90) || transform.localEulerAngles.y <= baseValue) //(transform.localEulerAngles.y > 270 || transform.localEulerAngles.y <= 0)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - openAmount, transform.localEulerAngles.z);
            }
        }
    }
    public void Close()
    {
        if (!playedCloseSound)
        {
            AkUnitySoundEngine.PostEvent("GatesClose", gameObject);
            playedCloseSound = true;
        }
        if (open90)
            {
                if (transform.localEulerAngles.y > baseValue && !(transform.localEulerAngles.y >= 360 - (baseValue + 2))) //(transform.localEulerAngles.y > 0 && !(transform.localEulerAngles.y >= 358))
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - openAmount, transform.localEulerAngles.z);
                else
                {
                    closing = false;
                    playedCloseSound = false;
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, baseValue, transform.localEulerAngles.z);
                }
            }
            else
            {
                if (transform.localEulerAngles.y < baseValue || transform.localEulerAngles.y >= 360 - (baseValue + 92)) //(transform.localEulerAngles.y < 0 || transform.localEulerAngles.y >= 268)
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + openAmount, transform.localEulerAngles.z);
                else
                {
                    closing = false;
                    playedCloseSound = false;
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, baseValue, transform.localEulerAngles.z);
                }
            }
    }
}
