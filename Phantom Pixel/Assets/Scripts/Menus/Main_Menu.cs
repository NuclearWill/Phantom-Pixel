using UnityEngine;
using UnityEngine.SceneManagement;



public class Main_Menu : MonoBehaviour
{
    public string i;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GameGo (string i)
    {
        TimeManager.resetGameTime();
        LevelManager.LoadLevelData();
        SceneManager.LoadScene(i);
        //SceneManager.LoadScene(i);
    }
    public void QuitGame ()
    {
        Application.Quit();
    }
}
