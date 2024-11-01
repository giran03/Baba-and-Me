using System.Collections;
using TMPro;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    public TMP_Text chapterTitle;
    string _nextChapter;
    string _previousChapter;

    void Start()
    {
        _previousChapter = PlayerPrefs.GetString("CurrentScene");

        Debug.LogError($"_previousChapter: {_previousChapter}");

        MusicManager.Instance.PlayMusic("transition");

        switch (_previousChapter)
        {
            case "MainMenu":
                _nextChapter = "Level 1";
                chapterTitle.SetText("Chapter 1");
                break;
            case "Level 1":
                _nextChapter = "Level 2";
                chapterTitle.SetText("Chapter 2");
                break;
            case "Level 2":
                _nextChapter = "MainMenu";
                chapterTitle.SetText("To Be Continued...");
                break;
        }

        StartCoroutine(TransitionToNextLevel());
    }

    IEnumerator TransitionToNextLevel()
    {
        Debug.LogError($"Transitioning to {_nextChapter}");
        yield return new WaitForSeconds(5f);
        LevelManager.Instance.ChangeScene(_nextChapter);
    }
}
