using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

public class BombTimer : MonoBehaviour
{
    [Header("Blinking UI Settings")]
    [SerializeField]
    [Range(1, 6)]
    [Tooltip("How many times the ui should blink in one second")]
    private int numbOfBlinks = 3;
    [SerializeField]
    private float timeRemainingToBeginBlinking = 5f;

    [Header("Bomb Timer Settings")]
    [SerializeField]
    private float startTime;
    private float timeLeft => startTime - TimeManager.GetGameTime();

    [SerializeField]
    private GameObject digit1Object, digit2Object;

    [SerializeField]
    private Sprite[] number = new Sprite[10];

    // internal variables
    private UnityEngine.UI.Image digit1, digit2;
    Color color1, color2;

    // lambda functions
    bool timeToBlink => timeLeft <= timeRemainingToBeginBlinking;
    int firstPowerDigit => Mathf.CeilToInt(timeLeft) % 10;
    int secondPowerDigit => (int) (Mathf.Ceil(timeLeft) / 10) % 10;

    private void Awake()
    {
        digit1 = digit1Object.GetComponent<UnityEngine.UI.Image>();
        digit2 = digit2Object.GetComponent<UnityEngine.UI.Image>();

        color1 = digit1.color;
        color2 = digit2.color;
    }

    void Update()
    {
        // updates the sprites to the correct digits on the timer
        digit1.sprite = number[firstPowerDigit];
        digit2.sprite = number[secondPowerDigit];

        // makes the digits blink red when the time gets low
        Color tempColor = (timeToBlink) ? Color.Lerp(color1, Color.red, Mathf.PingPong((TimeManager.GetGameTime()) * (numbOfBlinks * 2), 1)) : color1;
        digit1.color = tempColor;
        tempColor = (timeToBlink) ? Color.Lerp(color2, Color.red, Mathf.PingPong((TimeManager.GetGameTime()) * (numbOfBlinks * 2), 1)) : color2;
        digit2.color = tempColor;

        // lose the game if you run out of time
        if (timeLeft < 0)
        {
            Debug.Log("You Lose :(");
            LevelManager.LoseLevel();
        }
    }
}
