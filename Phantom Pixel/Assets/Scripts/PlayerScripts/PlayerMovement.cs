using System.Collections;
using System.IO;
using Unity.VisualScripting;
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
    [SerializeField]
    private float timeToMove = 0.2f;
    [SerializeField]
    private float moveDistance = 2f, raycastDistance = 1.4f, turnSpeed = 10f;

    [Header("Input Action Reference")]
    public InputSystem_Actions playerControls;

    // private variables
    private Vector3 moveDirection, origPos, targetPos;
    private bool isMoving, externalStop = false;

    private InputAction move;

    private bool onStair = false, walkingUpStair = false, walkingDownStair = false;
    private float stairRot;

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

            // can only move if the character isn't moving between tiles
            if (!isMoving)
            {
                // calculates the ability to move differently if on a staircase
                if (onStair)
                {
                    int result = compareStairToPlayer(stairRot);

                    if (result != 0)
                    {
                        if (result == 1)
                            walkingUpStair = true;
                        else
                            walkingDownStair = true;

                        StartCoroutine(MovePlayer(moveDirection)); // starts the function to move the player to the next tile
                    }
                }

                // ensures that the tile the player is attempting to move to is valid with no wall to block it and a floor below to stand on
                else if (checkWall() && checkFloor())
                    StartCoroutine(MovePlayer(moveDirection)); // starts the function to move the player to the next tile
            }
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

        moveDirection = (moveDirection.normalized);
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0f;

        origPos = transform.position;

        targetPos = origPos + (direction * moveDistance);

        // changes the height of targetPos depending if the character is trying to go up or down stairs
        if (walkingUpStair)
            targetPos += new Vector3(0, moveDistance / 2f, 0);
        if (walkingDownStair)
            targetPos -= new Vector3(0, moveDistance / 2f, 0);

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // end of movement code

        transform.position = targetPos;

        // changes variables depending on state with stairs
        if (onStair)
        {
            onStair = false;
            walkingUpStair = false;
            walkingDownStair = false;
        }
        else if (walkingUpStair)
        {
            walkingUpStair = false;
            onStair = true;
        }
        else if (walkingDownStair)
        {
            walkingDownStair = false;
            onStair = true;
        }

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
        // shoots the raycast to check for walls slightly higher than the center of the character. Helps with stairs
        Vector3 wallCheckPosition = transform.position + new Vector3(0, 0.2f, 0);

        bool struckSomething = Physics.Raycast(wallCheckPosition, moveDirection, out RaycastHit hit, raycastDistance);
        if (struckSomething)
        {
            if (hit.transform.CompareTag("Stair"))
            {
                // gets the oriantation of the staircase in radians
                stairRot = hit.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

                // makes sure the oriantation of the staircase is correct so the player doesn't walk up it from the wrong side
                if (compareStairToPlayer(stairRot) == 1)
                {
                    walkingUpStair = true;
                    return true;
                }
                return false;
            }

            return false;
        }

        return true;
    }

    bool checkFloor()
    {
        bool struckSomething = Physics.Raycast(transform.position + moveDirection * 2f, Vector3.down, out RaycastHit hit, raycastDistance);

        if (struckSomething)
        {
            // uses a switch to apply different checks and variables depending on the floor the player is trying to walk on
            switch (hit.transform.tag)
            {
                case "Water":
                    // checks to see if time is paused. If so, it passes as walkable terrain
                    // if not, passes as unwalkable terrain
                    if (TimeManager.isPaused())
                        return true;
                    return false;
                case "Stair":
                    // sets walkingDownStair to true and saves the stairs oriantation before returning true
                    walkingDownStair = true;
                    stairRot = hit.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
                    return true;
            }

            return true;
        }

        return false;
    }

    /*
     * function which compares the oriantation of a staircase to the player
     * put in the desired staircase's rotation in radians and the function will output one of three results
     * 1 means the player should go up when walking on the staircase
     * -1 means the player should go down when walking on the staircase
     * 0 means the player is perpindicular and should't go up or down
     */
    private int compareStairToPlayer(float staircaseRot)
    {
        //north and south input directions
        if (Mathf.Abs(moveDirection.x) == 1)
        {
            // if the movement direction is going towards the oriantation of the stairs, the player goes up
            if (Mathf.Sin(stairRot) == moveDirection.x)
                return 1;

            // if the movement direction is going away from the oriantaion of the stairs, the player goes down
            else if (Mathf.Sin(stairRot) == moveDirection.x * -1)
                return -1;

            // if neither, the movement direction is invalid
            else
                return 0;
        }

        // east and west input directions
        else
        {
            // if the movement direction is going towards the oriantation of the stairs, the player goes up
            if (Mathf.Cos(stairRot) == moveDirection.z)
                return 1;

            // if the movement direction is going away from the oriantaion of the stairs, the player goes down
            else if (Mathf.Cos(stairRot) == moveDirection.z * -1)
                return -1;

            // if neither, the movement direction is invalid
            else
                return 0;
        }
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
