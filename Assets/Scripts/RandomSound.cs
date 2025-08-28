using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] private float minSoundInterval = 1f;
    [SerializeField] private float maxSoundInterval = 3f;

    private float timer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (audioSource != null && audioClips.Length > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
                audioSource.clip = clip;

                timer = clip.length + Random.Range(minSoundInterval, maxSoundInterval);
                audioSource.Play();
            }
        }
    }
    public static void Play(AudioSource source, AudioClip[] clips)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.clip = clip;
        source.Play();
    }

    public static void PlayClipAtPoint(AudioClip[] clips, Vector2 position)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        AudioSource.PlayClipAtPoint(clip, position);
    }
}
