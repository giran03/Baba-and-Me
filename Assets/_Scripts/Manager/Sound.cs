using System.Collections;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [field: SerializeField] public AudioClip Audio { private set; get; }
    [field: SerializeField, Range(0f, 1f)] public float Volume { private set; get; } = 1f;
    [field: SerializeField] public float Range { private set; get; } = 500f;


    public void Play(Vector3 position)
    {
        SoundManager.Instance.PlaySound(this, position);
    }

    public void PlayWithRandomPitch(Vector3 position)
    {
        float pitch = 1f + Random.Range(-0.35f, 0.35f);
        SoundManager.Instance.PlaySound(this, position, true, pitch);
    }

    public void PlayAtSource(AudioSource audioSource)
    {
        audioSource.PlayOneShot(Audio, Volume);
    }
}