using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MishaInfant : MonoBehaviour
{
    [SerializeField] Sound helpSFX;
    bool _isRescued = false;
    private void Start()
    {
        StartCoroutine(PlayHelpSFX());
    }

    IEnumerator PlayHelpSFX()
    {
        float timer = 0;
        while (!_isRescued)
        {
            timer += Time.deltaTime;
            if (timer >= 3)
            {
                helpSFX.Play(transform.position);
                timer = 0;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogError($"Saved by {other.name}");
            StartCoroutine(FollowPlayer(other.transform));
        }
    }

    IEnumerator FollowPlayer(Transform player)
    {
        while (true)
        {
            _isRescued = true;
            Vector3 offset = player.position + new Vector3(-1, 0, 1);
            transform.position = Vector3.Lerp(transform.position, offset, 0.7f * Time.deltaTime);
            yield return null;
        }
    }

}
