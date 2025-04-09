using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BombTimer : MonoBehaviour
{
    [SerializeField]
    private float startTime = 120f;
    private float timeLeft => startTime - TimeManager.GetGameTime();

    [SerializeField]
    private TextMeshProUGUI uiElement;

    
    void Update()
    {
        uiElement.text = ("Time left: " + ((int) timeLeft / 60) + ":" + ((int) timeLeft % 60));
    }
}
