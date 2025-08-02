using DG.Tweening;
using Input;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public bool paused;
    private bool menuOpen = false;
    private Sequence mySequence = null;

    private void Start()
    {
        startPositions = new List<float>();
        foreach (GameObject menu in Menus)
        {
            startPositions.Add(menu.transform.position.y);
            if (menu != Menus[0])
            {
                menu.transform.DOMoveY(-Screen.height, 0f);
            }
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        paused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Menus[0].SetActive(true);
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause()
    {
        if (menuOpen)
        {
            TurnOffMenus();
            return;
        }
        foreach (GameObject menu in Menus)
        {
            //this jank makes sure submenus don't stay on screen
            if (menu != Menus[0])
            {
                menu.transform.position = new Vector3(menu.transform.position.x, -Screen.height, menu.transform.position.z);
            }
        }
        Time.timeScale = 1;
        Menus[0].SetActive(false);
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TopPauseMenu()
    {
        TurnOffMenus();
        Menus[0].SetActive(true);
        eventSystem.SetSelectedGameObject(settingsButton);
    }

    public void Settings()
    {
        TurnOffMenus();
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

    private void TurnOffMenus()
    {
        if (mySequence != null && mySequence.active)
            DOTween.KillAll();

        foreach (GameObject menu in Menus)
        {
            if (menu != Menus[0])
            {
                menu.transform.DOMoveY(-Screen.height, .4f).SetUpdate(true);
            }
        }
        menuOpen = false;
    }

    private void ActivateMenuWithAnimation(int index)
    {
        GameObject menu = Menus[index];
        menu.SetActive(true);

        float startPos = startPositions[index];

        mySequence = DOTween.Sequence().SetUpdate(true);
        mySequence.Append(menu.transform.DOMoveY(startPos + 50, .4f));
        mySequence.Append(menu.transform.DOMoveY(startPos, .5f));
        menuOpen = true;
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
