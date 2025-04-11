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
    private float cameraRotationRadians => thirdPersonCamera.eulerAngles.y * Mathf.Deg2Rad;

    [Header("Movement")]
    public float timeToMove = 0.2f;
    public float moveDistance = 2f;
    public float raycastDistance = 1.4f;
    public float turnSpeed = 10f;

    [Header("Input Action Reference")]
    public InputSystem_Actions playerControls;

    // private variables
    Vector3 moveDirection;
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private bool externalStop = false;

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

        /* 
         * only continues if the player has put in a movement input
         * if the game isn't rewinding
         * and if there isn't an external force stopping the player from moving
         */
        if (moveDirection != Vector3.zero && !TimeManager.isRewinding() && !externalStop)
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
        Vector2 initialInput = move.ReadValue<Vector2>();

        // gets a Vector2 determined by where the camera is facing the level
        Vector2 cameraInfluence = new Vector2(Mathf.Cos(cameraRotationRadians), Mathf.Sin(cameraRotationRadians));

        // warps input to be change direction based off the camera's rotation
        if (Mathf.Abs(cameraInfluence.x) >= Mathf.Abs(cameraInfluence.y))
        {
            moveDirection = collapseInput(initialInput);
            
            // inverts moveDirection if the camera is facing south
            if(cameraInfluence.x < 0)
                moveDirection *= -1;
        }
        else
        {
            moveDirection = collapseInput(new Vector2(initialInput.y, -initialInput.x));
            
            // inverts moveDirection if the camera is facing west
            if (cameraInfluence.y < 0)
                moveDirection *= -1;
        }

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
        bool struckSomething = Physics.Raycast(transform.position + moveDirection, Vector3.down, out RaycastHit hit, raycastDistance);
        if (struckSomething)
        {
            // checks to see if the floor checks is water
            if(hit.transform.CompareTag("Water"))
            {
                Debug.Log("Struck Water!");
                // checks to see if time is paused. If so, it passes as walkable terrain
                // if not, passes as unwalkable terrain
                if (TimeManager.isPaused())
                    return true;
                return false;
            }

            return true;
        }

        return false;
    }

    public bool getIsMoving()
    {
        return isMoving;
    }

    public void CantMove()
    {
        externalStop = true;
    }

    public void CanMove()
    {
        externalStop = false;
    }

    private Vector3 collapseInput(Vector2 input)
    {
        // collapses the input to only be at a single direction depending on which value is greater
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            return new Vector3(input.x, 0, 0);
        else if (Mathf.Abs(input.y) > Mathf.Abs(input.x))
            return new Vector3(0, 0, input.y);
        else
            return Vector3.zero;
    }
}
