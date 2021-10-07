using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [SerializeField] string VersionNumber = "0.1";
    [SerializeField] private Menu[] menus;

    private void Awake()
    {
        Instance = this;
        OpenMenu("loading");
    }

    public void OpenMenu(string name)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == name)
                OpenMenu(menus[i]);
            else
                CloseMenu(menus[i]);
        }
    }

    public void OpenMenu(Menu menu)
    {
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
