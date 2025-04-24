using UnityEngine;
using UnityEngine.SceneManagement;

public class BigBomb : MonoBehaviour , IInteractable
{
    public void Interact()
    {
        Debug.Log("You Win!");
        SceneManager.LoadScene(0);
    }
}
