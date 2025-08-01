using DG.Tweening;
using Input;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Menus;

    private List<float> startPositions;

    [SerializeField]
    string MainMenuName;

    public bool paused;

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

        paused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Menus[0].SetActive(true);
        paused = true;
    }

    public void Unpause()
    {
        foreach (GameObject menu in Menus)
        {
            //this jank makes sure submenus don't stay on screen
            if (menu != Menus[0])
            {;
                DG.Tweening.Sequence mySequence = DOTween.Sequence().SetUpdate(true);
                mySequence.Append(menu.transform.DOMoveY(-Screen.height, 0f));
                mySequence.Append(menu.transform.DOMoveY(-Screen.height, 1f));
            }
        }
        Time.timeScale = 1;
        Menus[0].SetActive(false);
        paused = false;
    }

    public void TopPauseMenu()
    {
        TurnOffMenus();
        Menus[0].SetActive(true);
    }

    public void Settings()
    {
        TurnOffMenus();
        ActivateMenuWithAnimation(1);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(MainMenuName);
    }

    private void TurnOffMenus()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu != Menus[0])
            {
                menu.transform.DOMoveY(-Screen.height, .4f).SetUpdate(true);
            }
        }
    }

    private void ActivateMenuWithAnimation(int index)
    {
        GameObject menu = Menus[index];
        menu.SetActive(true);

        float startPos = startPositions[index];

        DG.Tweening.Sequence mySequence = DOTween.Sequence().SetUpdate(true);
        mySequence.Append(menu.transform.DOMoveY(startPos + 50, .4f));
        mySequence.Append(menu.transform.DOMoveY(startPos, .5f));
    }
}
