using UnityEngine;
using Input;
using UnityEngine.UI;
using TMPro;
using Game;
using DG.Tweening;

public class introCutscene : MonoBehaviour
{
    public GameObject gameManager, player;
    public GameObject nextButton, charSprite, textBox;
    public TMP_Text text;
    public InputManager imso;
    Animator anim;
    public Tween charTween, textTween, nextTween;

    float frame1 = .05f;
    float frame2 = 2f;
    float timer = 0f;
    float moveSpeed = 2f;

    float timer1 = 0f;
    bool startTimer = false;

    public bool gameStarted = false;
    string[] dialogue = { "Sheesh, who let all of these weirdos into the rink?!",
                          "I cant skate in peace when theres this much Danger around.",
                          "Hmmm... I know!",
                          "Ill skate in circles to cut holes in the ice.",
                          "Then the Danger will fall right in!"};
    int index = 0;
    public Image PlayerImage;
    public Sprite happyImage;
    public Sprite mehImage;

    public void Start()
    {
        text.text = dialogue[index];
        anim = nextButton.GetComponent<Animator>();

        AkUnitySoundEngine.PostEvent("LightDrumTrack", PauseManager.globalWwise);
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
                // used to do smth here
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
                AkUnitySoundEngine.PostEvent("SelectUI", PauseManager.globalWwise);
                if (index >= dialogue.Length)
                {
                    gameStarted = true;
                    text.text = "";
                    player.GetComponent<PlayerController>().startedThisFrame = true;
                    player.GetComponent<PlayerController>().startCutsceneActive = false;
                    gameManager.GetComponent<GameController>().StartController();
                    charTween = charSprite.transform.DOMoveY(charSprite.transform.position.y - 2000, 2f, false);
                    textTween = textBox.transform.DOMoveY(textBox.transform.position.y - 2000, 2f, false);
                    nextTween = nextButton.transform.DOMoveY(nextButton.transform.position.y - 2000, 2f, false);
                    AkUnitySoundEngine.PostEvent("LevelMusic", PauseManager.globalWwise);
                }
                else
                {
                    text.text = dialogue[index];
                    if (index < 2)
                        PlayerImage.sprite = mehImage;
                    else
                        PlayerImage.sprite = happyImage;
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

    void OnDestroy()
    {
        DOTween.Kill(textTween);
        DOTween.Kill(charTween);
        DOTween.Kill(nextTween);
    }
}