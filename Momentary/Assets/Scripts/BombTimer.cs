using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BombTimer : MonoBehaviour
{
    [SerializeField]
    private float startTime;
    private float timeLeft => startTime - TimeManager.GetGameTime();

    [SerializeField]
    private TextMeshProUGUI uiElement;

    
    void Update()
    {
        uiElement.text = ("Time left: " + ((int) timeLeft / 60) + ":" + ((int) timeLeft % 60));

        if(timeLeft < 0)
        {
            Debug.Log("You Lose :(");
            SceneManager.LoadScene(0);
        }
    }
}
