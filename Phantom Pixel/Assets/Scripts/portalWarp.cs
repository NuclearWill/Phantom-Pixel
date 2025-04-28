using UnityEngine;
using UnityEngine.SceneManagement;

public class portalWarp : MonoBehaviour
{
    public string levelName;
    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            Debug.Log("Portal Triggered");
            LevelManager.LoadLevel(levelName);
        }
    }
}
