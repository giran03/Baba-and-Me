using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    [Header("Timer")]
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text deerCount;
    [SerializeField] GoalArea _goalArea;
    public static int DeerCounter { get; set; }

    [SerializeField] float timeLeft = 5f;
    [SerializeField] bool isTimerRunning = false;


    private void Start()
    {
        if (_goalArea != null)
            DeerCounter = _goalArea.enemiesToKill.Count;
    }

    private void Update()
    {
        deerCount.SetText($"Deer Count: {DeerCounter}");

        if (isTimerRunning)
        {
            timeLeft -= Time.deltaTime;
            timer.SetText($"TIME LEFT: {Mathf.Round(timeLeft)}");
            if (timeLeft <= 0)
            {
                isTimerRunning = false;
                StartCoroutine(StartRefreshLevel());
                Debug.LogError("Game Over");
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                timeLeft = 2;
            }
        }
    }

    IEnumerator StartRefreshLevel()
    {
        PlayerConfigs.Instance.CanMove = false;
        PlayerStateMachine.StopPlayer();

        yield return new WaitForSeconds(2f);

        PlayerStateMachine.ResumePlayer();

        // TODO: restart???
        LevelManager.Instance.RestartCurrentLevel();
        PlayerConfigs.Instance.CanMove = true;
    }
}