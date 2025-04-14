using UnityEngine;
using UnityEngine.SceneManagement;

public class BombTriggerCollider : MonoBehaviour
{
    public bool colliderOn = false;
    private void OnTriggerEnter(Collider other)
    {
        if ((colliderOn))
        {
            Debug.Log("You Win!");
            SceneManager.LoadScene(0);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((colliderOn))
        {
            Debug.Log("You Win!");
            SceneManager.LoadScene(0);
        }
    }
}
