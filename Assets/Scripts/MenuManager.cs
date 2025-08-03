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
    }

    public void StartGame()
    {
        if (!ScreenWipe.over) return;
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
        eventSystem.SetSelectedGameObject(playButton);
    }

    public void Credits()
    {
        if (!ScreenWipe.over) return;
        TurnOffMenus();
        ActivateMenuWithAnimation(1);
        eventSystem.SetSelectedGameObject(creditsBackButton);
    }

    public void Instructions()
    {
        if (!ScreenWipe.over) return;
        TurnOffMenus();
        ActivateMenuWithAnimation(2);
        eventSystem.SetSelectedGameObject(instructionsBackButton);
    }

    public void Settings()
    {
        if (!ScreenWipe.over) return;
        TurnOffMenus();
        ActivateMenuWithAnimation(3);
        eventSystem.SetSelectedGameObject(settingsBackButton);
        settingsOpen = true;
    }

    public void QuitGame()
    {
        if (!ScreenWipe.over) return;
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

    private void TurnOffMenus()
    {
        if (settingsOpen)
        {
            PlayerPrefs.Save();
            settingsOpen = false;
        }
        foreach (GameObject menu in Menus)
        {
            if (menu != Menus[0])
            {
                float startPos = menu.transform.position.y;
                //menu.SetActive(false);

                DG.Tweening.Sequence mySequence = DOTween.Sequence();
                mySequence.Append(menu.transform.DOMoveY(-Screen.height, .4f));
                mySequence.Append(menu.transform.DOMoveY(startPos, 0f));
                menu.SetActive(false);
            }
        }
    }

    private void ActivateMenuWithAnimation(int index)
    {
        GameObject menu = Menus[index];
        menu.GetComponent<RectTransform>().localScale = Vector3.one;
        menu.SetActive(true);

        float startPos = Menus[index].transform.position.y;// startPositions[index];

        DG.Tweening.Sequence mySequence = DOTween.Sequence();
        mySequence.Append(menu.transform.DOMoveY(-Screen.height + 50, 0f));
        mySequence.Append(menu.transform.DOMoveY(startPos + 50, .4f));
        mySequence.Append(menu.transform.DOMoveY(startPos, .5f));
    }
}
