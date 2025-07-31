using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Menus;

    [SerializeField]
    string MainMenuName;

    public void Settings()
    {

    }

    public void MainMenu()
    {
        SceneManager.LoadScene(MainMenuName);
    }
}
