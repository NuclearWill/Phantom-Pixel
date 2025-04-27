using System;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Door : DynamicObject, IInteractable
{
    [SerializeField]
    GameObject door, leftDoor, rightDoor; // the actual door to be manipulated

    [Header("Door Settings")]
    [SerializeField]
    private bool startingPosition = true;
    [SerializeField]
    static float openHeight = 3f, closeHeight = 1f, openSpeed = 0.5f;

    // internal variables
    private bool open, doubleDoor = false;
    private float elapsedTime;
    private AudioSource doorAudioSource;

    // lambda functions
    bool activated => startingPosition != open;
    float soundPitch => (TimeManager.isRewinding()) ? -openSpeed / doorAudioSource.clip.length : openSpeed / doorAudioSource.clip.length;


    private void Start()
    {
        if (door == null)
            doubleDoor = true;

        if (startingPosition)
        {
            open = true;
            AdjustDoorHeight(openHeight);
        }
        else
        {
            open = false;
            AdjustDoorHeight(closeHeight);
        }

        // gets the audio source from the door
        doorAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // only moves the elevator if time is not rewinding or paused, and if the elevator is supposed to be moving
        if (getMoving() && !TimeManager.isRewinding() && !TimeManager.isPaused())
        {
            MoveDoor();
        }

        // ensures the sound is playing correctly
        doorAudioSource.pitch = soundPitch;
        if (getMoving() && (!TimeManager.isPaused() || TimeManager.isRewinding()))
        {
            // play sound effect
            if (!doorAudioSource.isPlaying)
            {
                doorAudioSource.UnPause();
            }
        }
        else
        {
            // stop sound effect
            if (doorAudioSource.isPlaying)
            {
                doorAudioSource.Pause();
            }
        }
    }

    public void Interact()
    {
        // makes sure the elevator can't be triggered unless if it is already done moving
        if (!activated)
        {
            BeginMove();
        }
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        DoorPIT nextPoint = (DoorPIT) PIT;

        door.transform.position = nextPoint.position;

        setMoving(nextPoint.isMoving);
        open = nextPoint.open;
        elapsedTime = nextPoint.elapsedTime;
    }

    public override PointInTime CreatePIT()
    {
        return new DoorPIT(door.transform, getMoving(), open, elapsedTime);
    }

    private void BeginMove()
    {
        // sets the elevator's variables up to begin moving
        Debug.Log("door activate");
        elapsedTime = 0f;
        setMoving(true);

        open = !open;

        // plays the elevator sound
        doorAudioSource.time = 0f; // reset the audio clip to the beginning
        doorAudioSource.Play();
    }

    private void MoveDoor()
    {
        // makes sure the destination and starting point is correct (due to time manipulation)
        float targetHeight = (!startingPosition) ? openHeight : closeHeight;
        float startHeight = (startingPosition) ? openHeight : closeHeight;

        // changes the position according to elapsed time
        AdjustDoorHeight(Mathf.Lerp(startHeight, targetHeight, elapsedTime / openSpeed));

        elapsedTime += Time.deltaTime;

        // once elapsed time is greater than the time to move, stop moving
        if (elapsedTime > openSpeed)
        {
            StopMoving();
        }
    }

    private void StopMoving()
    {
        setMoving(false);

        // stops the elevator sound
        doorAudioSource.Pause();
    }
    
    private void AdjustDoorHeight(float height)
    {
        door.transform.localPosition = new Vector3(-0.8f, height, 0f);
    }
}
