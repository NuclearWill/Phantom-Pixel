using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void MainMenuFunction() 
    {
        SceneManager.LoadScene("Main menu");
    }
    public void ExitToHubFunction()
    {
        SceneManager.LoadScene("Hub");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
