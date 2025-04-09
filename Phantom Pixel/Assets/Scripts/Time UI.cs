using UnityEngine;

public class ReverseTimeUI : MonoBehaviour
{
    [SerializeField]
    private GameObject reverseUI;

    [SerializeField]
    private GameObject pauseUI;

    // super basic script to turn on the visual indicating that time is reversing
    void Update()
    {
        // rewind ui
        if (TimeManager.isRewinding())
            reverseUI.SetActive(true);
        else
            reverseUI.SetActive(false);

        // pause ui
        if (TimeManager.isPaused())
            pauseUI.SetActive(true);
        else
            pauseUI.SetActive(false);
    }
}
