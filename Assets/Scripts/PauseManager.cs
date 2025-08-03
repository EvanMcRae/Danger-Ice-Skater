using DG.Tweening;
using Input;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UI.PlayerUI;


public class PauseManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Menus;

    private List<float> startPositions;

    [SerializeField]
    string MainMenuName;

    [SerializeField]
    public EventSystem eventSystem;

    [SerializeField]
    public GameObject settingsBackButton;

    [SerializeField]
    public GameObject settingsButton;

    [SerializeField]
    public GameObject playButton;

    public static bool paused;
    private bool menuOpen = false;
    private Sequence mySequence = null;

    public GameObject WwiseGlobal;

    private void Start()
    {
        startPositions = new List<float>();
        foreach (GameObject menu in Menus)
        {
            startPositions.Add(menu.transform.position.y);
            if (menu != Menus[0])
            {
                //menu.transform.DOMoveY(-Screen.height, 0f);
            }
        }
        Cursor.visible = false;
        paused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Menus[0].SetActive(true);
        paused = true;
        Cursor.visible = true;
        eventSystem.SetSelectedGameObject(playButton);
        AkUnitySoundEngine.PostEvent("PauseGame", WwiseGlobal);
    }

    public void Unpause()
    {
        if (menuOpen)
        {
            TurnOffMenus();
            return;
        }
        Time.timeScale = 1;
        Menus[0].SetActive(false);
        paused = false;
        AkUnitySoundEngine.PostEvent("ResumeGame", WwiseGlobal);
        Cursor.visible = false;
    }

    public void TopPauseMenu()
    {
        PlayerPrefs.Save();
        TurnOffMenus();
        Menus[0].SetActive(true);
        eventSystem.SetSelectedGameObject(settingsButton);
    }

    public void Settings()
    {
        TurnOffMenus(1);
        ActivateMenuWithAnimation(1);
        eventSystem.SetSelectedGameObject(settingsBackButton);
    }

    public void MainMenu()
    {
        ScreenWipe.current.WipeIn();
        ScreenWipe.current.PostWipe += GoToMainMenu;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        ScreenWipe.current.PostWipe -= GoToMainMenu;
        SceneManager.LoadScene(MainMenuName);
    }

    private void TurnOffMenus(int excludeMenu = -1)
    {
        if (mySequence != null && mySequence.active)
            DOTween.KillAll();

        for (int index = 0; index < Menus.Length; index++)
        {
            GameObject menu = Menus[index];
            if (menu != Menus[0])
            {
                float startPos = menu.transform.position.y;

                DG.Tweening.Sequence mySequence = DOTween.Sequence();
                mySequence.Append(menu.transform.DOMoveY(-Screen.height, .4f));
                mySequence.SetUpdate(true);

                if (index != excludeMenu)
                {
                    mySequence.OnComplete(() =>
                    {
                        menu.transform.DOMoveY(startPos, 0f).SetUpdate(true);
                        menu.SetActive(false);
                    });
                }
            }
        }
        menuOpen = false;
    }

    private void ActivateMenuWithAnimation(int index)
    {
        GameObject menu = Menus[index];
        menu.SetActive(true);

        menu.GetComponent<RectTransform>().localScale = Vector3.one;

        float startPos = Menus[index].transform.position.y;// startPositions[index];
        Vector3 pos = Menus[index].transform.position;
        pos.y = -Screen.height;
        Menus[index].transform.position = pos;

        DG.Tweening.Sequence mySequence = DOTween.Sequence();
        mySequence.Append(menu.transform.DOMoveY(startPos + 50, .4f));
        mySequence.Append(menu.transform.DOMoveY(startPos, .5f));
        mySequence.SetUpdate(true);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    public static bool ShouldNotRun()
    {
        return paused || WinLoseUI.lostGame;
    }
}
