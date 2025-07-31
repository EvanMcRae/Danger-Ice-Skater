using DG.Tweening;
using Input;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Menus;

    private List<float> startPositions;

    [SerializeField]
    string MainMenuName;

    private void Start()
    {
        startPositions = new List<float>();
        foreach (GameObject menu in Menus)
        {
            startPositions.Add(menu.transform.position.y);
            if (menu != Menus[0])
            {
                menu.transform.DOMoveY(-900, 0f);
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Menus[0].SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        TurnOffMenus();
        Menus[0].SetActive(false);
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
                menu.transform.DOMoveY(-900, .4f).SetUpdate(true);
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
