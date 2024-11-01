using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToEnable;
    [SerializeField] bool changeMusic;
    [SerializeField] string musicName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectsToEnable?.ForEach(x => x.SetActive(true));

            if (changeMusic)
                MusicManager.Instance.PlayMusic(musicName);
        }
    }
}
