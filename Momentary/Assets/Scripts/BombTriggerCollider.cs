using UnityEngine;
using UnityEngine.SceneManagement;

public class BombTriggerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("You Win!");
        SceneManager.LoadScene(0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("You Win!");
        SceneManager.LoadScene(0);
    }
}
