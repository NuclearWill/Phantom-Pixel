using UnityEngine;
using UnityEngine.SceneManagement;



public class Main_Menu : MonoBehaviour
{
    
    public void GameGo ()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame ()
    {
        Application.Quit();
    }
}
