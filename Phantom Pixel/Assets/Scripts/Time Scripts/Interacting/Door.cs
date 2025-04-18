using System;
using UnityEngine;

public class Door : TimeBody, IInteractable
{
    [Header("Door Settings")]
    [SerializeField]
    private bool startingPosition = true;

    private bool open = false;

    private void Start()
    {
        if(startingPosition)
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
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    private void CloseDoor()
    {
        open = false;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }
}
