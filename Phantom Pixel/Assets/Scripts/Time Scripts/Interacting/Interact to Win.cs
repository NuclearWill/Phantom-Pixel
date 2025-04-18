using UnityEngine;
using UnityEngine.SceneManagement;

public class InteracttoWin : MonoBehaviour, IInteractable
{
    // interact with this to win the level

    public void Interact()
    {
        // beat the level
        Debug.Log("You Win!");
        SceneManager.LoadScene(0);
    }
}
