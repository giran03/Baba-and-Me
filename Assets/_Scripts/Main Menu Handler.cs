using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayMusic("mainMenu");
        PlayerPrefs.SetString("CurrentScene", "MainMenu");
    }

    public void Button_Start() => LevelManager.Instance.ChangeScene("Transition");
    public void Button_Quit() => Application.Quit();

}
