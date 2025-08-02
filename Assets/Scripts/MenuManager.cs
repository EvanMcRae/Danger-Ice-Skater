using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Menus;

    private List<float> startPositions;

    [SerializeField]
    string NextSceneName;

    private void Start()
    {
        startPositions = new List<float>(); ///[Menus.Length];
        foreach (GameObject menu in Menus)
        {
            startPositions.Add(menu.transform.position.y);
            if (menu != Menus[0])
            {
                menu.transform.DOMoveY(-Screen.height, 0f);
            }
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
    }

    public void Credits()
    {
        if (!ScreenWipe.over) return;
        TurnOffMenus();
        ActivateMenuWithAnimation(1);
    }

    public void Instructions()
    {
        if (!ScreenWipe.over) return;
        TurnOffMenus();
        ActivateMenuWithAnimation(2);
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
        if (Application.isEditor)
        {
            EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }

    private void TurnOffMenus()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu != Menus[0])
            {
                //menu.SetActive(false);
                menu.transform.DOMoveY(-Screen.height, .4f);
            }
        }
    }

    private void ActivateMenuWithAnimation(int index)
    {
        GameObject menu = Menus[index];
        menu.SetActive(true);

        float startPos = startPositions[index];

        DG.Tweening.Sequence mySequence = DOTween.Sequence();
        mySequence.Append(menu.transform.DOMoveY(startPos + 50, .4f));
        mySequence.Append(menu.transform.DOMoveY(startPos, .5f));
    }
}
