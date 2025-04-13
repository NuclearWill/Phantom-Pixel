using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTimeControl : MonoBehaviour
{
    [Header("Input Action Reference")]
    public InputSystem_Actions playerControls;

    private InputAction rewind;
    private InputAction pause;
    private InputAction restart;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        rewind = playerControls.Player.RewindTime;
        pause = playerControls.Player.PauseTime;
        restart = playerControls.Player.RestartLevel;
        rewind.Enable();
        pause.Enable();
        restart.Enable();
    }

    private void OnDisable()
    {
        rewind.Disable();
        pause.Disable();
        restart.Disable();
    }

    void Update()
    {
        if (!GetComponent<PlayerMovement>().getIsMoving())
        {
            // reverse time controls
            if (rewind.WasPressedThisFrame())
            {
                Debug.Log("Begin Reversing Time");
                TimeManager.BeginReverseTime();
            }
            if (rewind.WasReleasedThisFrame())
            {
                Debug.Log("Stop Reversing Time");
                TimeManager.StopReversingTime();
            }

            // pause time controls
            if (pause.WasPressedThisFrame())
            {
                TimeManager.TogglePause();
            }
        }

        // restart level
        if (restart.WasPressedThisFrame())
            TimeManager.RestartTime();
    }
}
