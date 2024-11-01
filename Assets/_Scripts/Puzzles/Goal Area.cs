using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// End Goal for the Level.
/// Must Clear all 'Orcs' before moving on.
/// </summary>
public class GoalArea : MonoBehaviour
{
    public List<GameObject> objectsToEnable = new();
    public List<GameObject> objectsToDisable = new();
    public bool DestroyWhenFinished = false;

    public List<GameObject> enemiesToKill = new();
    List<GameObject> enemiesList = new();
    public static bool isGameFinished = false;
    private bool _isObjectsEnabled;

    private void Start() => enemiesList = enemiesToKill;

    private void Update()
    {
        for (int i = enemiesList.Count - 1; i >= 0; i--)
        {
            GameObject enemy = enemiesList[i];
            if (enemy == null)
            {
                enemiesList.RemoveAt(i);
                Timer.DeerCounter--;
            }
        }

        if (enemiesList.Count == 0 && !isGameFinished)
            isGameFinished = true;
        else
            isGameFinished = false;

        if (isGameFinished && !_isObjectsEnabled)
        {
            _isObjectsEnabled = true;
            objectsToEnable?.ForEach(x => x.SetActive(true));
            objectsToDisable?.ForEach(x => x.SetActive(false));

            if (DestroyWhenFinished)
                Destroy(gameObject);
        }
    }
}
