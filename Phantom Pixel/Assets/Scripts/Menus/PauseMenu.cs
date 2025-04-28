using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [Header("Input Action Reference")]
    public InputSystem_Actions playerControls;

    [Header("Pause Menu Canvas Reference")]
    public GameObject PauseCanvas;

    // internal variables
    private static bool isPaused;
    private InputAction pauseAction;

    void Awake()
    {
        playerControls = new InputSystem_Actions();

        PauseCanvas.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnEnable()
    {
        pauseAction = playerControls.Player.PauseGame;
        pauseAction.Enable();
    }

    void OnDisable()
    {
        pauseAction.Disable();
    }

    void Update()
    {
        if (pauseAction.WasPressedThisFrame())
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
    private void PauseGame() 
    {
        TimeManager.PauseTime();

        PauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ResumeGame() 
    {
        TimeManager.ResumeTime();

        PauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResumeGameButton()
    {
        ResumeGame();
    }

    public void GoToHub()
    {
        ResumeGame();

        LevelManager.LoadLevel("Hub");
    }

    public void GoToMainMenu()
    {
        ResumeGame();

        LevelManager.LoadLevel("Main menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public static bool GamePaused()
    {
        return isPaused;
    }
}
