using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerRack : ButtonConsole
{
    [SerializeField]
    private static int serversToDestroyToWin = 4;
    private static int serversDestroyed = 0;

    [SerializeField] private DialogueManager dialogueManager;

    public override void Interact()
    {
        serversDestroyed++;
        if(serversDestroyed >= serversToDestroyToWin)
        {
            Debug.Log("You Win!");
            //Make sure this shit doesn't run 4 times
            StartCoroutine(dialogueManager.Dialogue(dialogueManager.EndLevelDialogue));
            LevelManager.completeLevel(3);
            //SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Server destroyed! " + (serversToDestroyToWin - serversDestroyed) + " left to destroy.");
        }
        base.Interact();
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        base.ApplyRewindData(PIT);
    }

    public override PointInTime CreatePIT()
    {
        return base.CreatePIT();
    }
}
