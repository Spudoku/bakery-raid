using UnityEngine;

public class RandomSound : MonoBehaviour
{
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
