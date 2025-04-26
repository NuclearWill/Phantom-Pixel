using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;

    AudioClip musicTrack;

    [SerializeField]
    AudioSource pauseMusicSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on this GameObject.");
            enabled = false;
        }

        musicTrack = audioSource.clip;
        if (musicTrack == null)
        {
            Debug.LogError("Music Track not included.");
            enabled = false;
        }
    }

    private void Update()
    {
        // changes the pitch to negative while time is rewinding
        audioSource.pitch = (TimeManager.isRewinding()) ? -1f : 1f;

        // pauses the audio if time is paused
        if (!TimeManager.isPaused() || TimeManager.isRewinding())
        {
            // play music effect
            if (!audioSource.isPlaying)
            {
                audioSource.time = (TimeManager.GetGameTime());
                audioSource.Play();
                pauseMusicSource.Stop();
            }
        }
        else
        {
            // stop music effect
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                pauseMusicSource.Play();
            }
        }
    }
}
