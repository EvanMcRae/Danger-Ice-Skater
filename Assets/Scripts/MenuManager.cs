using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Menus;

    [SerializeField]
    string NextSceneName;

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
        Menus[1].SetActive(true);
    }

    public void Instructions()
    {
        TurnOffMenus();
        Menus[2].SetActive(true);
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
            menu.SetActive(false);
        }
    }
}
