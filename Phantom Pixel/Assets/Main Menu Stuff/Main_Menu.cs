using UnityEngine;
using UnityEngine.SceneManagement;



public class Main_Menu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GameGo ()
    {
        TimeManager.resetGameTime();
        SceneManager.LoadScene(1);
    }
    public void QuitGame ()
    {
        Application.Quit();
    }
}
