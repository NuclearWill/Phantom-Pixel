using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    [Header("Input Action Reference")]
    public InputSystem_Actions playerControls;

    Vector2 moveDirection;
    private InputAction move;

    Rigidbody rb;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Update()
    {
        MyInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // calculate moveDirection based on diagonal camera
        Vector2 initialInput = move.ReadValue<Vector2>();
        moveDirection = new Vector2(initialInput.x - initialInput.y, initialInput.y + initialInput.x);
        moveDirection.Normalize();
    }

    private void MovePlayer()
    {
        rb.linearVelocity = new Vector3( moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.y * moveSpeed);
    }
}
