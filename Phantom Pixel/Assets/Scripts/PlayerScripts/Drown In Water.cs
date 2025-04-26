using UnityEngine;

public class DrownInWater : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Water"))
            LevelManager.LoseLevel();
    }
}
