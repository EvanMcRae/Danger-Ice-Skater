using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Menus;

    public void StartGame()
    {

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

    }

    private void TurnOffMenus()
    {
        foreach (GameObject menu in Menus)
        {
            menu.SetActive(false);
        }
    }
}
