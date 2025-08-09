using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Menus;

    private List<float> startPositions;

    [SerializeField]
    string NextSceneName;

    [SerializeField]
    public EventSystem eventSystem;

    [SerializeField]
    public GameObject instructionsBackButton;

    [SerializeField]
    public GameObject creditsBackButton;

    [SerializeField]
    public GameObject playButton;

    [SerializeField]
    public GameObject settingsBackButton;
    private bool settingsOpen;
    public GameObject WwiseGlobal;
    public static bool pleaseNoSound = true;
    public static bool tweening = false;
    public GameObject previousButton;

    private void Start()
    {
        startPositions = new List<float>(); ///[Menus.Length];
        foreach (GameObject menu in Menus)
        {
            //startPositions.Add(menu.transform.position.y);
            //if (menu != Menus[0])
            //{
            //    menu.transform.DOMoveY(-Screen.height, 0f);
            //}
        }
        if (PauseManager.globalWwise == null)
            PauseManager.globalWwise = WwiseGlobal;
        AkUnitySoundEngine.PostEvent("MenuMusic", PauseManager.globalWwise);
    }

    public void StartGame()
    {
        if (!ScreenWipe.over) return;
        AkUnitySoundEngine.PostEvent("StartGameUI", PauseManager.globalWwise);
        ScreenWipe.current.WipeIn();
        ScreenWipe.current.PostWipe += EnterGameScene;
    }

    public void EnterGameScene()
    {
        ScreenWipe.current.PostWipe -= EnterGameScene;
        SceneManager.LoadScene(NextSceneName);
    }

    public void Title()
    {
        if (!ScreenWipe.over) return;
        TurnOffMenus();

        Menus[0].SetActive(true);
        pleaseNoSound = true;
        eventSystem.SetSelectedGameObject(previousButton);
        previousButton = null;
    }

    public void Credits()
    {
        if (!ScreenWipe.over) return;
        TurnOffMenus(1);
        ActivateMenuWithAnimation(1);
        pleaseNoSound = true;
        eventSystem.SetSelectedGameObject(creditsBackButton);
    }

    public void Instructions()
    {
        if (!ScreenWipe.over) return;
        TurnOffMenus(2);
        ActivateMenuWithAnimation(2);
        pleaseNoSound = true;
        eventSystem.SetSelectedGameObject(instructionsBackButton);
    }

    public void Settings()
    {
        if (!ScreenWipe.over) return;
        TurnOffMenus(3);
        ActivateMenuWithAnimation(3);
        pleaseNoSound = true;
        eventSystem.SetSelectedGameObject(settingsBackButton);
        settingsOpen = true;
    }

    public void QuitGame()
    {
        if (!ScreenWipe.over) return;
        AkUnitySoundEngine.PostEvent("BacKUI", PauseManager.globalWwise);
        ScreenWipe.current.WipeIn();
        ScreenWipe.current.PostWipe += ActuallyQuitGame;
    }

    public void ActuallyQuitGame()
    {
        ScreenWipe.current.PostWipe -= ActuallyQuitGame;
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void TurnOffMenus(int excludeMenu = -1)
    {
        if (excludeMenu == -1)
            AkUnitySoundEngine.PostEvent("BackUI", PauseManager.globalWwise);

        if (settingsOpen)
        {
            PlayerPrefs.Save();
            settingsOpen = false;
        }
        for (int index = 0; index < Menus.Length; index++)
        {
            GameObject menu = Menus[index];
            if (menu != Menus[0])
            {
                float startPos = menu.transform.position.y;
                //menu.SetActive(false);

                tweening = true;
                DG.Tweening.Sequence mySequence = DOTween.Sequence();
                mySequence.Append(menu.transform.DOMoveY(-Screen.height, .4f));

                if (index != excludeMenu)
                {
                    mySequence.OnComplete(() =>
                    {
                        menu.transform.DOMoveY(startPos, 0f);
                        menu.SetActive(false);
                        tweening = false;
                    });
                }

            }
        }
    }

    private void ActivateMenuWithAnimation(int index)
    {
        previousButton = eventSystem.currentSelectedGameObject;
        AkUnitySoundEngine.PostEvent("SelectUI", PauseManager.globalWwise);

        GameObject menu = Menus[index];
        menu.GetComponent<RectTransform>().localScale = Vector3.one;
        menu.SetActive(true);

        float startPos = Menus[index].transform.position.y;// startPositions[index];
        Vector3 pos = Menus[index].transform.position;
        pos.y = -Screen.height;
        Menus[index].transform.position = pos;

        tweening = true;
        DG.Tweening.Sequence mySequence = DOTween.Sequence();
        mySequence.Append(menu.transform.DOMoveY(startPos + 50, .4f));
        mySequence.Append(menu.transform.DOMoveY(startPos, .5f));

        mySequence.OnComplete(() => { tweening = false; });
    }
}