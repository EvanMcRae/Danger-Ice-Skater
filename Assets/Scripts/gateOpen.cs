using UnityEngine;

public class gateOpen : MonoBehaviour
{
    public bool opened = false;
    public bool open90;
    float timer = 0f;
    float openTime = 1f;
    float openAmount = 1f;
    bool closing = false;

    public void Update()
    {
        if (opened) Open();
        if (closing) Close();
        
        if(timer >= openTime)
        {
            opened = false;
            timer = 0;
            closing = true;
        }
    }
    public void Open()
    {
        timer += Time.deltaTime;
        //print("Opening " + transform.localEulerAngles.y);
        //Debug.Log(transform.eulerAngles.y + ", " + transform.rotation.y + ", " + transform.localRotation.y + ", " + transform.localEulerAngles);

        if (open90)
        {
            if (transform.localEulerAngles.y < 90)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + openAmount, transform.localEulerAngles.z);
            }
        }
        else
        {
            if (transform.localEulerAngles.y > 270 || transform.localEulerAngles.y <= 0)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - openAmount, transform.localEulerAngles.z);
                print("hi");
            }
        }
    }
    public void Close()
    {
        print("Closing " + transform.localEulerAngles.y);
        if (open90)
        {
            if (transform.localEulerAngles.y > 0)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - openAmount, transform.localEulerAngles.z);
            else
            {
                closing = false;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
            }
        }
        else
        {
            if (transform.localEulerAngles.y < 0 || transform.localEulerAngles.y >= 268)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + openAmount, transform.localEulerAngles.z);
            else
            {
                closing = false;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
            }
        }
    }
}
