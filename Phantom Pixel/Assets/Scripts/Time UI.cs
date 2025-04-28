using UnityEngine;

public class ReverseTimeUI : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;

    // super basic script to turn on the visual indicating that time is reversing
    void Update()
    {
        // pause ui
        if (TimeManager.isPaused())
            pauseUI.SetActive(true);
        else
            pauseUI.SetActive(false);
    }
}
