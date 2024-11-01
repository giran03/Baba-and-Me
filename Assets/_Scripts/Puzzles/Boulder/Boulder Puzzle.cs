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

    [SerializeField] Sound sfx;
    private bool _plateHasMoved;

    public bool BoulderPuzzleComplete { get; set; }

    private void Update()
    {
        if (CompareLists(boulders, currentBoulders) && !BoulderPuzzleComplete && !PlayerGrab.IsGrabbing)
        {
            BoulderPuzzleComplete = true;
            UnlockObjects();
            LockObjects();
            Debug.Log($"Puzzle Completed");
        }

        if (BoulderPuzzleComplete && !_plateHasMoved)
        {
            _plateHasMoved = true;
            sfx.Play(transform.position);
            var start = transform.position;
            var end = transform.position + Vector3.down * 0.15f;
            StartCoroutine(MoveOverTime(start, end, 1f));
        }
    }

    IEnumerator MoveOverTime(Vector3 start, Vector3 end, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = end;
    }

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

    bool CompareLists<T>(List<T> aListA, List<T> aListB) => aListA.Count == aListB.Count && aListA.All(aListB.Contains);
}