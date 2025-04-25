using UnityEngine;
using UnityEngine.SceneManagement;



public class Main_Menu : MonoBehaviour
{
    public int i;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GameGo (int i)
    {
        TimeManager.resetGameTime();
        SceneManager.LoadScene(i);
    }
    public void QuitGame ()
    {
        Application.Quit();
    }
}
