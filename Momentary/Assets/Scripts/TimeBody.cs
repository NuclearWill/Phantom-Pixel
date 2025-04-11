using NUnit.Framework;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    private static float recordTime = 20f;

    List<PointInTime> history;

    private bool isElevator = false;

    Elevator elevatorReference = null;

    private void Start()
    {
        // creates a new array to store history in
        history = new List<PointInTime>();

        // checks to see if the object is an elevator and stores its elevator reference if so
        if(GetComponent<Elevator>() != null)
        {
            isElevator = true;
            elevatorReference = GetComponent<Elevator>();
        }
    }

    private void FixedUpdate()
    {
        if (TimeManager.isRewinding())
            Rewind();
        else if (!TimeManager.isPaused())
            Record(); // only records if its not rewinding and if its not paused
    }

    private void Rewind()
    {
        if (history.Count > 0)
        {
            PointInTime nextPoint = history[0];
            if (isElevator)
            {
                elevatorReference.rewindElevator((ElevatorPIT) nextPoint);
            }

            // applies the position and the rotation of the point in history as its actively rewinding
            transform.position = nextPoint.position;
            transform.rotation = nextPoint.rotation;
            

            history.RemoveAt(0);
        }
        else
            TimeManager.StopReversingTime();
    }

    private void Record()
    {
        // removes the latest history if the array is full
        if (history.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            history.RemoveAt(history.Count - 1);
        }

        // inserts the newest history into the array
        if(isElevator) // creates special PIT objects for elevators
            history.Insert(0, new ElevatorPIT(transform, elevatorReference.getMoving(), elevatorReference.getStartingLocation(), elevatorReference.getElapsedTime()));
        else // creates default PIT objects for misc objects
            history.Insert(0, new PointInTime(transform));
    }
}
