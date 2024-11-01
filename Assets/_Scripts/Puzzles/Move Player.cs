using System.Collections;
using UnityEngine;

public class MovePlayer
{
    /// <summary>
    /// Call me using Start Coroutine
    /// </summary>
    /// <param name="caller">this gameobject</param>
    /// <param name="other">collider of the player</param>
    /// <param name="timeDelay">this coroutine will run for this many seconds</param>
    /// <param name="coroutine">optional coroutine to call after timeDelay</param>
    /// <returns></returns>
    public IEnumerator MoveThisPlayer(GameObject caller, Collider other, float timeDelay, IEnumerator coroutine = null)
    {
        PlayerStateMachine.StopPlayer();
        Vector3 startPosition = other.transform.position;
        Vector3 endPosition = caller.transform.GetChild(caller.transform.childCount - 1).gameObject.transform.position;
        Vector3 targetPosition = new(endPosition.x, other.transform.position.y, endPosition.z);
        float timeElapsed = 0;
        while (timeElapsed < timeDelay)
        {
            timeElapsed += Time.deltaTime;
            other.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / timeDelay);
            yield return null;
        }

        if (coroutine != null)
        {
            caller.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }
        else
            PlayerStateMachine.ResumePlayer();
    }
}
