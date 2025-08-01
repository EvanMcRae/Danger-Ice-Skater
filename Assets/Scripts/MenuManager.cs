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
                menu.transform.DOMoveY(-900, 0f);
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(NextSceneName);
    }

    public void Title()
    {
        TurnOffMenus();

        Menus[0].SetActive(true);
    }

    public void Credits()
    {
        TurnOffMenus();
        ActivateMenuWithAnimation(1);
    }

    public void Instructions()
    {
        TurnOffMenus();
        ActivateMenuWithAnimation(2);
    }

    public void QuitGame()
    {
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
            if(menu != Menus[0])
            {
                //menu.SetActive(false);
                menu.transform.DOMoveY(-900, .4f);
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
