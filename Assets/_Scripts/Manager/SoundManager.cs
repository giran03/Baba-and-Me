using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [Space]
    [SerializeField] private GameObject audioSourcePrefab;

    const int MAX_SOUNDS = 64;

    private SoundSource[] soundEffects = new SoundSource[MAX_SOUNDS];
    private SoundSource cachedSoundSource;

    private Transform listener;

    public static SoundManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        CreateAudioSources();
    }

    public void SetListener(Transform transform)
    {
        listener = transform;
    }

    void CreateAudioSources()
    {
        for (int i = 0; i < MAX_SOUNDS; i++)
        {
            GameObject obj = Instantiate(audioSourcePrefab, _transform);
            soundEffects[i] = new SoundSource(obj.transform, obj.GetComponent<AudioSource>());
        }
    }

    bool CanBeHeard(Vector3 position, Sound sound)
    {
        return (position - listener.position).sqrMagnitude <= sound.Range * sound.Range;
    }

    public void PlaySound(Sound sound, Vector3 position, float pitch = 1f)
    {
        if (!CanBeHeard(position, sound))
        {
            return;
        }

        cachedSoundSource = GetSoundSource();

        if (cachedSoundSource == null)
        {
            return;
        }

        cachedSoundSource.transform.position = position;
        cachedSoundSource.SetSound(sound);

        if (pitch != 1f)
        {
            cachedSoundSource.source.pitch = pitch;
        }

        cachedSoundSource.source.Play();
    }

    SoundSource GetSoundSource()
    {
        for (int i = 0; i < MAX_SOUNDS; i++)
        {
            if (!soundEffects[i].source.isPlaying)
            {
                return soundEffects[i];
            }
        }

        return null;
    }
}