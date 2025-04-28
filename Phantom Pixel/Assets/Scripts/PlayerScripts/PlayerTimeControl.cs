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
        // doesn't let the player manipulate time while moving, while the game is paused, or while dialogue is active
        if (!GetComponent<PlayerMovement>().getIsMoving() && !PauseMenuScript.GamePaused() && !DialogueManager.isDialogueActive)
        {
            // rewind time controls
            if (rewind.WasPressedThisFrame())
            {
                Debug.Log("Begin Reversing Time");
                TimeManager.BeginReverseTime();
            }
            // only triggers if the player is rewinding time and tries to stop
            if (rewind.WasReleasedThisFrame() && TimeManager.isRewinding())
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
