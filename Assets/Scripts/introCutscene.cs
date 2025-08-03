using UnityEngine;
using Input;
using UnityEngine.UI;
using TMPro;

public class introCutscene : MonoBehaviour
{
    public GameObject gameManager, player;
    public GameObject nextButton, charSprite, textBox;
    public TMP_Text text;
    public InputManager imso;

    public bool gameStarted = false;
    string[] dialogue = { "Holy Bingle",
                          "We got the no fly list",
                          "We actually did it",
                          "I'm gonna skate around in celebration."};
    int index = 0;

    public void Start()
    {
        text.text = dialogue[index];
    }

    public void Update()
    {
        if (PauseManager.ShouldNotRun()) return;

        if (imso.jump.action.WasPressedThisFrame() && !gameStarted)
        {
            index += 1;
            if (index >= dialogue.Length) gameStarted = true;
            else
            {
                text.text = dialogue[index];
            }
        }

        if (gameStarted)
        {
            player.GetComponent<PlayerController>().startCutsceneActive = false;
            //Instantiate(gameManager);
            Destroy(gameObject);
        }
    }
}
