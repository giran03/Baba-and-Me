using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// End Goal for the Level.
/// Must Clear all 'Orcs' before moving on.
/// </summary>
public class GoalArea : MonoBehaviour
{
    public static bool isGameFinished = false;

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Orc").Length == 0 && !isGameFinished)
        {
            isGameFinished = true;
            Debug.Log($"Can now go to the next level!");
        }
        else
        {
            Debug.Log($"There's Still ORC's remaining!");
            isGameFinished = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isGameFinished)
            Debug.Log($"LEVEL CLEAR!");
    }
}
