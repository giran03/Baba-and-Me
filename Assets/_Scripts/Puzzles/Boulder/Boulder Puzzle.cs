using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles unlocking objects in the boulder puzzle.
/// Put this script in the Drop Off Area gameobject
/// </summary>
public class BoulderPuzzle : MonoBehaviour
{
    [Tooltip("Boulder gameobject")]
    [SerializeField] List<GameObject> boulders;

    [HideInInspector] public List<GameObject> currentBoulders = new();

    [Tooltip("Set the gameobjects to inactive")]
    [SerializeField] List<GameObject> objectToUnlock;

    [Tooltip("Set the gameobjects to active")]
    [SerializeField] List<GameObject> objectToLock;

    public bool BoulderPuzzleComplete { get; set; }

    private void Update()
    {
        if (CompareLists(boulders, currentBoulders) && !BoulderPuzzleComplete)
        {
            BoulderPuzzleComplete = true;
            UnlockObjects();
            LockObjects();
            RemoveTags(boulders);
            Debug.Log($"Puzzle Completed");
        }
    }

    private void RemoveTags(List<GameObject> boulders) => boulders.ForEach(boulder => boulder.tag = "Untagged");

    private void OnTriggerEnter(Collider other)
    {
        if (boulders.Contains(other.gameObject))
        {
            if (!currentBoulders.Contains(other.gameObject))
                currentBoulders.Add(other.gameObject);
        }
    }

    void UnlockObjects() => objectToUnlock?.ForEach(obj => obj.SetActive(false));

    void LockObjects() => objectToLock?.ForEach(obj => obj.SetActive(true));

    public static bool CompareLists<T>(List<T> aListA, List<T> aListB) => aListA.Count == aListB.Count && aListA.All(aListB.Contains);
}
