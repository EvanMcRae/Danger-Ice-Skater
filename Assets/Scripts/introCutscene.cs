using UnityEngine;
using Input;
using UnityEngine.UI;
using TMPro;
using Game;

public class introCutscene : MonoBehaviour
{
    public GameObject gameManager, player;
    public GameObject nextButton, charSprite, textBox;
    public TMP_Text text;
    public InputManager imso;
    Animator anim;

    float frame1 = .05f;
    float frame2 = 1f;
    float timer = 0f;
    float moveSpeed = 2f;

    float timer1 = 0f;
    bool startTimer = false;

    public bool gameStarted = false;
    string[] dialogue = { "Sheesh, who let all of these creatures into the rink?!",
                          "I cant skate around in peace with all this commotion.",
                          "Hmmm... I know!",
                          "Ill skate in circles to cut holes in the ice.",
                          "Then the creatures will fall right in!"};
    int index = 0;

    public void Start()
    {
        text.text = dialogue[index];
        anim = nextButton.GetComponent<Animator>();
    }

    public void Update()
    {
        if (PauseManager.ShouldNotRun()) return;
        
        if (gameStarted)
        {
            if (timer < frame1)
            {
                charSprite.transform.position = new Vector3(charSprite.transform.position.x, charSprite.transform.position.y + moveSpeed, charSprite.transform.position.z);
                textBox.transform.position = new Vector3(textBox.transform.position.x, textBox.transform.position.y + moveSpeed, textBox.transform.position.z);
                nextButton.transform.position = new Vector3(nextButton.transform.position.x, nextButton.transform.position.y + moveSpeed, nextButton.transform.position.z);

            }
            else if (frame1 <= timer && timer < frame2)
            {
                charSprite.transform.position = new Vector3(charSprite.transform.position.x, charSprite.transform.position.y - moveSpeed, charSprite.transform.position.z);
                textBox.transform.position = new Vector3(textBox.transform.position.x, textBox.transform.position.y - moveSpeed, textBox.transform.position.z);
                nextButton.transform.position = new Vector3(nextButton.transform.position.x, nextButton.transform.position.y - moveSpeed, nextButton.transform.position.z);

            }
            else
            {
                Destroy(gameObject);
            }
            timer += Time.deltaTime;
        }
        else
        {
            if (imso.jump.action.WasPressedThisFrame() && !gameStarted)
            {
                anim.Play("click");
                startTimer = true;
                index += 1;
                if (index >= dialogue.Length)
                {
                    gameStarted = true;
                    text.text = "";
                    player.GetComponent<PlayerController>().startCutsceneActive = false;
                    gameManager.GetComponent<GameController>().StartController();
                }
                else
                {
                    text.text = dialogue[index];
                }
            }
            if (startTimer) timer1 += Time.deltaTime;
            else anim.Play("still");
            if (timer1 >= .5f)
            {
                timer1 = 0;
                startTimer = false;
            }

        }
    }
}
