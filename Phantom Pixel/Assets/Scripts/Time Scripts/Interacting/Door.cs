using System;
using UnityEngine;

public class Door : TimeBody, IInteractable
{
    [SerializeField]
    GameObject door, leftDoor, rightDoor; // the actual door to be manipulated

    [Header("Door Settings")]
    [SerializeField]
    private bool startingPosition = true;

    [SerializeField]
    static float openHeight, closeHeight, openSpeed;

    private bool open = false, doubleDoor = false;

    private void Start()
    {
        if (door == null)
            doubleDoor = true;

        if (startingPosition)
            OpenDoor();
    }

    public void Interact()
    {
        if (startingPosition)
            CloseDoor();
        else
            OpenDoor();
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        DoorPIT nextPoint = (DoorPIT) PIT;

        if (nextPoint.open)
            OpenDoor();
        else
            CloseDoor();
    }

    public override PointInTime CreatePIT()
    {
        return new DoorPIT(open);
    }

    private void OpenDoor()
    {
        open = true;
        if (!doubleDoor)
        {
            door.GetComponent<MeshRenderer>().enabled = false;
            door.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            leftDoor.GetComponent<MeshRenderer>().enabled = false;
            leftDoor.GetComponent<BoxCollider>().enabled = false;
            rightDoor.GetComponent<MeshRenderer>().enabled = false;
            rightDoor.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void CloseDoor()
    {
        open = false;
        if (!doubleDoor)
        {
            
            door.GetComponent<MeshRenderer>().enabled = true;
            door.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            leftDoor.GetComponent<MeshRenderer>().enabled = true;
            leftDoor.GetComponent<BoxCollider>().enabled = true;
            rightDoor.GetComponent<MeshRenderer>().enabled = true;
            rightDoor.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
