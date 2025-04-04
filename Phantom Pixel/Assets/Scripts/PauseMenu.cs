using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool TimePaused = false;

    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!TimePaused)
                PauseTime();
            else
                ResumeTime();
        }
    }

    private void PauseTime()
    {
        pauseMenuUI.SetActive(true);
        TimePaused = true;
    }

    private void ResumeTime()
    {
        pauseMenuUI.SetActive(false);
        TimePaused = false;
    }
}
