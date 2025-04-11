using UnityEngine;

public class PlayerTimeControl : MonoBehaviour
{
    void Update()
    {
        if (!GetComponent<PlayerMovement>().getIsMoving())
        {
            // reverse time controls
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Begin Reversing Time");
                TimeManager.BeginReverseTime();
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                Debug.Log("Stop Reversing Time");
                TimeManager.StopReversingTime();
            }

            // pause time controls
            if (Input.GetKeyDown(KeyCode.E))
            {
                TimeManager.TogglePause();
            }
        }

        // restart level
        if (Input.GetKeyDown(KeyCode.R))
            TimeManager.RestartTime();
    }
}
