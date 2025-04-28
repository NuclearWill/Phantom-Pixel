using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PauseCanvas;
    public static bool isPaused;
    void Start()
    {
        PauseCanvas.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame() 
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame() 
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
