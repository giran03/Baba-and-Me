using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class EscapeLevel : MonoBehaviour
{
    MovePlayer _movePlayer = new();


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(_movePlayer.MoveThisPlayer(gameObject, other, 2, EndLevel(2)));
        }
    }

    [Button]
    public void EndThisLevel() => StartCoroutine(EndLevel(2));

    IEnumerator EndLevel(float duration)
    {
        Debug.LogError($"GOING TO NEXT LEVEL~");
        yield return new WaitForSeconds(duration);
        PlayerStateMachine.ResumePlayer();
        switch (LevelManager.Instance.GetCurrentLevel())
        {
            case "Level 1":
                MusicManager.Instance.PlayMusic("level2");
                PlayerPrefs.SetString("NextLevel", "Level 2");
                // LevelManager.Instance.ChangeScene("Level 2");
                break;
            case "Level 2":
                PlayerPrefs.SetString("NextLevel", "MainMenu");
                // LevelManager.Instance.ChangeScene("MainMenu");
                break;
        }

        LevelManager.Instance.ChangeScene("Transition");
    }
}