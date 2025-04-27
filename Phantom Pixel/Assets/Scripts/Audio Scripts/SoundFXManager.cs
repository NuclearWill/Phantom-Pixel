using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }

    [SerializeField]
    private AudioSource soundFXObject;

    public AudioClip pauseStart, 
        pauseEnd,
        rewindStart,
        rewindEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        Destroy(audioSource, audioClip.length);
    }
}
