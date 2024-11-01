using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        switch (GetCurrentLevel())
        {
            case "Transition":
                MusicManager.Instance.PlayMusic("transition");
                break;
            case "Level 1":
                MusicManager.Instance.PlayMusic("level1");
                PlayerPrefs.SetString("CurrentScene", "Level 1");
                PauseManager.PauseGame();
                break;
            case "Level 2":
                MusicManager.Instance.PlayMusic("level2");
                PlayerPrefs.SetString("CurrentScene", "Level 2");
                PauseManager.PauseGame();
                break;
        }
    }

    public void ChangeScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public string GetCurrentLevel() => SceneManager.GetActiveScene().name;
    public void RestartCurrentLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}