using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class Elevator : DynamicObject, IInteractable
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
    //[SerializeField]
    //private AudioClip startSound, midSound, endSound;

    // movement based private variables
    private bool movingTowardsOrigin = true;
    private float elapsedTime = 0f;

    // variable storing the object riding the elevator
    GameObject elevatorUser;

    // the elevators audio source
    private AudioSource elevatorAudioSource;


    private void Start()
    {
        // sets the starting and ending locations of the elevator
        location1 = transform.position;
        location2 = new Vector3(location1.x, location1.y + (2 * destinationHeight), location1.z);

        // gets the audio source from the elevator
        elevatorAudioSource = GetComponent<AudioSource>();
        if (elevatorAudioSource == null)
        {
            Debug.LogError("Elevator does not have an audio source");
        }
    }


    private void Update()
    {
        // only moves the elevator if time is not rewinding or paused, and if the elevator is supposed to be moving
        if (getMoving() && !TimeManager.isRewinding() && !TimeManager.isPaused())
        {
            moveElevator();
        }

        // ensures the sound is playing correctly
        elevatorAudioSource.pitch = (TimeManager.isRewinding()) ? -1f : 1f;
        if (getMoving() && (!TimeManager.isPaused() || TimeManager.isRewinding()))
        {
            // play sound effect
            if (!elevatorAudioSource.isPlaying)
            {
                elevatorAudioSource.UnPause();
            }
        }
        else
        {
            // stop sound effect
            if (elevatorAudioSource.isPlaying)
            {
                elevatorAudioSource.Pause();
            }
        }
    }

    public void Interact()
    {
        // makes sure the elevator can't be triggered unless if it is already done moving
        if (!getMoving())
        {
            beginMoving();
        }
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        ElevatorPIT nextPoint = (ElevatorPIT) PIT;

        transform.position = nextPoint.position;
        transform.rotation = nextPoint.rotation;

        setMoving(nextPoint.isMoving);
        movingTowardsOrigin = nextPoint.movingTowardsOrigin;
        elapsedTime = nextPoint.elapsedTime;
    }

    public override PointInTime CreatePIT()
    {
        return new ElevatorPIT(transform, getMoving(), movingTowardsOrigin, elapsedTime);
    }

    private void beginMoving()
    {
        // sets the elevator up to begin moving
        Debug.Log("elevator activate");
        elapsedTime = 0f;
        setMoving(true);

        movingTowardsOrigin = !movingTowardsOrigin;

        // checks to see if anything is on the elevator and saves that object for later use
        Ray ray = new Ray(transform.position, Vector3.up);
        elevatorUser = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 1.4f))
        {
            // the raycast will hit the model of the player. The parent of the model is the player object that we need
            elevatorUser = hit.transform.gameObject;
            // attempts to access the rider's playerMovement script to prevent them from attemping to walk off the elevator while it's operating
            PlayerMovement usersMovement = elevatorUser.GetComponent<PlayerMovement>();
            if (usersMovement != null)
                usersMovement.CantMove();
        }

        // plays the elevator sound
        elevatorAudioSource.time = 0f; // reset the audio clip to the beginning
        elevatorAudioSource.Play();
    }

    private void moveElevator()
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

        // once elapsed time is greater than the time to move, stop moving
        if (elapsedTime > timeToMove)
        {
            stopMoving();
        }
    }

    private void stopMoving()
    {
        transform.position = targetLocation;
        if (elevatorUser != null)
        {
            elevatorUser.transform.position = new Vector3(targetLocation.x, targetLocation.y + 2f, targetLocation.z);

            // attempts to access the rider's movement script to allow them to move again after the elevator is finished
            PlayerMovement usersMovement = elevatorUser.GetComponent<PlayerMovement>();
            if (usersMovement != null)
                usersMovement.CanMove();
        }

        setMoving(false);
        elevatorUser = null;

        // stops the elevator sound
        elevatorAudioSource.Pause();
    }
}
