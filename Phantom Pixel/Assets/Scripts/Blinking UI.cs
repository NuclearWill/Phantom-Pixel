using UnityEngine;
using UnityEngine.UI;

public class BlinkingUI : MonoBehaviour
{
    [Header("Flooding Water Reference")]
    [SerializeField]
    WaterFlooding waterFlooding;

    [Header("Blinking UI Settings")]
    [SerializeField]
    [Tooltip("How much time before the water floods should the ui start blinking?")]
    private float blinkBuffer = 1f;
    [SerializeField]
    [Range(1, 6)]
    private int numbOfBlinks = 3;

    // internal variables
    Color color;
    Image imgComp;
    AudioSource audioSource;
    AudioClip audioClip;

    // lambda functions
    float timeToStartBlinking => waterFlooding.GetFillDelay() - blinkBuffer;
    bool timeToBlink => TimeManager.GetGameTime() % waterFlooding.GetFillDelay() >= timeToStartBlinking;

    private void Awake()
    {
        imgComp = GetComponent<Image>();
        if (imgComp == null)
        {
            Debug.LogError("No Image component found on this GameObject.");
            enabled = false; // Disable this script if no Image component is found
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject.");
            enabled = false; // Disable this script if no AudioSource component is found
        }
        audioClip = audioSource.clip;

        color = imgComp.color;
    }

    private void Update()
    {
        /*
         * only blinks during the calculated window between the buffer and when the water floods
         * uses lerp to fade in and out the warning image
         * uses TimeManager to get the game time in accordance with time travel
         * uses ping pong to create a smooth transition in and out
         * subtracts the game time by the time to start blinking so that it starts blinking at alpha = 0
         * multiplies the number of blinks by 2 so that it has enough time to blink in and blink out within the buffer window and end on a 0
         */
        color.a = (timeToBlink) ? Mathf.Lerp(0f, 1f, Mathf.PingPong((TimeManager.GetGameTime() - timeToStartBlinking) * (numbOfBlinks * 2), 1)) : 0f;
        imgComp.color = color;

        // sound effect code
        // changes the pitch to be negative when rewinding time to reverse the audio
        audioSource.pitch = (TimeManager.isRewinding()) ? -numbOfBlinks : numbOfBlinks;
        if (timeToBlink && (!TimeManager.isPaused() || TimeManager.isRewinding()))
        {
            // play sound effect
            if (!audioSource.isPlaying)
            {
                audioSource.time = (TimeManager.GetGameTime() - timeToStartBlinking) % waterFlooding.GetFillDelay();
                audioSource.Play();
            }
        }
        else
        {
            // stop sound effect
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
