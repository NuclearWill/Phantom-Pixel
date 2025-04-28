using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Input Action Reference")]
    public InputSystem_Actions playerControls;

    [SerializeField]
    private float rayDistance = 1.4f;

    private InputAction interact;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        interact = playerControls.Player.Interact;
        interact.Enable();
    }

    private void OnDisable()
    {
        interact.Disable();
    }

    void Update()
    {
        if (interact.WasPressedThisFrame())
        {
            // shoots a ray out from where the player is facing, contining if it hits something
            if (Physics.Raycast(transform.position, transform.forward * rayDistance, out RaycastHit hit))
            {
                // checks to see if the hit object is a button, interacting with it if so
                ButtonConsole target = hit.collider.GetComponent<ButtonConsole>();
                if (hit.collider.GetComponent<ButtonConsole>() != null)
                    target.Interact();
            }
        }
    }
}
