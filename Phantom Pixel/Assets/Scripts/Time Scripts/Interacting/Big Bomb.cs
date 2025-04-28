using UnityEngine;
using UnityEngine.SceneManagement;

public class BigBomb : MonoBehaviour , IInteractable
{
    [SerializeField] private DialogueManager dialogueManager;
    public void Interact()
    {
        Debug.Log("You Win!");
        StartCoroutine(dialogueManager.Dialogue(dialogueManager.EndLevelDialogue));
        LevelManager.completeLevel(1);
        //SceneManager.LoadScene(0);
    }
}
