using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Elevator : TimeBody, IInteractable
{
    // destinations for elevator
    private Vector3 location1;
    private Vector3 location2;

    // the target location and originating location for when the elevator moves
    private Vector3 targetLocation;
    private Vector3 startingLocation;

    [Header("Elevator Settings")]
    [SerializeField]
    [Tooltip("The number of tiles the elevator goes up or down when activated. Positive numbers to go up, negative to go down")]
    private int destinationHeight;
    public float timeToMove = 1f;

    // movement based private variables
    private bool isMoving = false;
    private bool movingTowardsOrigin = true;
    private float elapsedTime = 0f;

    // variable storing the object riding the elevator
    GameObject elevatorUser;


    private void Start()
    {
        // sets the starting and ending locations of the elevator
        location1 = transform.position;
        location2 = new Vector3(location1.x, location1.y + (2 * destinationHeight), location1.z);
    }


    private void Update()
    {
        // only moves the elevator if time is not rewinding or paused, and if the elevator is supposed to be moving
        if (isMoving && !TimeManager.isRewinding() && !TimeManager.isPaused())
        {
            // makes sure the destination and starting point is correct (due to time manipulation)
            targetLocation = (movingTowardsOrigin) ? location1 : location2;
            startingLocation = (!movingTowardsOrigin) ? location1 : location2;

            // changes the position according to elapsed time
            transform.position = Vector3.Lerp(startingLocation, targetLocation, (elapsedTime / timeToMove));

            // moves the object on top of the elevator
            if (elevatorUser != null)
            {
                elevatorUser.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
            }

            elapsedTime += Time.deltaTime;

            if (elapsedTime > timeToMove)
            {
                // finishing up after done moving

                transform.position = targetLocation;
                if (elevatorUser != null)
                {
                    elevatorUser.transform.position = new Vector3(targetLocation.x, targetLocation.y + 2f, targetLocation.z);

                    // attempts to access the rider's movement script to allow them to move again after the elevator is finished
                    PlayerMovement usersMovement = elevatorUser.GetComponent<PlayerMovement>();
                    if(usersMovement != null)
                        usersMovement.CanMove();
                }

                isMoving = false;
                elevatorUser = null;
            }
        }
    }

    public void Interact()
    {
        // makes sure the elevator can't be triggered unless if it is already done moving
        if (!isMoving)
        {
            // sets the elevator up to begin moving
            Debug.Log("elevator activate");
            elapsedTime = 0f;
            isMoving = true;

            movingTowardsOrigin = !movingTowardsOrigin;

            // checks to see if anything is on the elevator and saves that object for later use
            Ray ray = new Ray(transform.position, Vector3.up);
            elevatorUser = null;

            if (Physics.Raycast(ray, out RaycastHit hit, 1.4f))
            {
                // the raycast will hit the model of the player. The parent of the model is the player object that we need
                elevatorUser = hit.transform.parent.gameObject;
                // attempts to access the rider's playerMovement script to prevent them from attemping to walk off the elevator while it's operating
                PlayerMovement usersMovement = elevatorUser.GetComponent<PlayerMovement>();
                if(usersMovement != null)
                    usersMovement.CantMove();
            }
        }
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        ElevatorPIT nextPoint = (ElevatorPIT) PIT;

        transform.position = nextPoint.position;
        transform.rotation = nextPoint.rotation;

        isMoving = nextPoint.isMoving;
        movingTowardsOrigin = nextPoint.isAtStartingLocation;
        elapsedTime = nextPoint.elapsedTime;
    }

    public override PointInTime CreatePIT()
    {
        return new ElevatorPIT(transform, isMoving, movingTowardsOrigin, elapsedTime);
    }
}
