using UnityEngine;
using UnityEngine.SceneManagement;

public class BigBomb : MonoBehaviour , IInteractable
{
    public void Interact()
    {
        Debug.Log("You Win!");
        LevelManager.completeLevel(1);
        SceneManager.LoadScene(0);
    }
}
