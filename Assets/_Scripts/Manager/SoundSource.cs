using UnityEngine;

[System.Serializable]
public class SoundSource
{
    public Transform transform;
    public AudioSource source;

    public SoundSource(Transform transform, AudioSource audioSource)
    {
        this.transform = transform;
        source = audioSource;
    }

    public void SetSound(Sound sound)
    {
        source.clip = sound.Audio;
        source.volume = sound.Volume;
        source.maxDistance = sound.Range;
    }
}