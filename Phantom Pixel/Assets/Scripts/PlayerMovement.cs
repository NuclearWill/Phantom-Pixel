using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Object References")]
    public Transform orientation;
    public Transform playerObj;

    [Header("Camera References")]
    public Transform thirdPersonCamera;

    [Header("Movement")]
    public float timeToMove = 0.2f;
    public float moveDistance = 2f;
    public float raycastDistance = 1.4f;
    public float turnSpeed = 10f;

    [Header("Input Action Reference")]
    public InputSystem_Actions playerControls;

    Vector3 moveDirection;
    private bool isMoving;
    private Vector3 origPos, targetPos;

    private InputAction move;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
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

        // only continues if the player has put in a movement input and if the game isn't paused or rewinding
        if (moveDirection != Vector3.zero && !TimeManager.isRewinding() && !TimeManager.isPaused())
        { 
            FaceDirection();

            // ensures that the tile the player is attempting to move to is valid with no wall to block it and a floor below to stand on
            // also only works if the player isn't already moving between tiles
            if (checkWall() && checkFloor() && !isMoving) 
                StartCoroutine(MovePlayer(moveDirection)); // starts the function to move the player to the next tile
        }
    }

    private void MyInput()
    {
        // calculate moveDirection based on diagonal camera
        Vector2 initialInput = move.ReadValue<Vector2>();
        

        // collapses the input to only be at a single direction depending on which value is greater
        if (Mathf.Abs(initialInput.x) > Mathf.Abs(initialInput.y))
            moveDirection = new Vector3(initialInput.x, 0, 0);
        else if (Mathf.Abs(initialInput.y) > Mathf.Abs(initialInput.x))
            moveDirection = new Vector3(0, 0, initialInput.y);
        else
            moveDirection = Vector3.zero;

        moveDirection = (moveDirection.normalized) * moveDistance;
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0f;

        origPos = transform.position;

        targetPos = origPos + direction;

        while(elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }

    private void FaceDirection()
    {
        // rotate orientation
        Vector3 viewDir = transform.position - new Vector3(thirdPersonCamera.position.x, transform.position.y, thirdPersonCamera.position.z);
        orientation.forward = viewDir.normalized;

        // rotate player object
        playerObj.forward = Vector3.Slerp(playerObj.forward, moveDirection, Time.deltaTime * turnSpeed);
    }

    bool checkWall()
    {
        
        return !Physics.Raycast(transform.position, moveDirection, raycastDistance);
    }

    bool checkFloor()
    {
        
        return Physics.Raycast(transform.position + moveDirection, Vector3.down, raycastDistance);
    }

    public bool getIsMoving()
    {
        return isMoving;
    }
}
