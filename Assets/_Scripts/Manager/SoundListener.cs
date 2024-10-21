using UnityEngine;

public class SoundListener : MonoBehaviour
{
    [SerializeField] private AudioListener audioListener;

    void Start()
    {
        if (audioListener)
        {
            SoundManager.Instance.SetListener(audioListener.transform);
        }
    }
}