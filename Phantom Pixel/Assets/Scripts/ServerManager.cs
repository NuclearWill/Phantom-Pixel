using UnityEngine;

public class ServerManager : MonoBehaviour
{
    [SerializeField]
    private ServerRack[] servers;

    [SerializeField]
    private int serversToDestroyToWin = 4;
    private int serversDestroyed = 0;

    [SerializeField] 
    private DialogueManager dialogueManager;

    private void Update()
    {
        serversDestroyed = 0;
        foreach (var server in servers)
        {
            if (server.activated)
            {
                serversDestroyed++;
            }
        }

        if (serversDestroyed >= serversToDestroyToWin)
        {
            Debug.Log("You Win!");

            StartCoroutine(dialogueManager.Dialogue(dialogueManager.EndLevelDialogue));
            LevelManager.completeLevel(3);
        }
    }
}
