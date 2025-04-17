using NUnit.Framework;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public abstract class TimeBody : MonoBehaviour
{
    public const float recordTime = 30f;

    List<PointInTime> history;

    private void Awake()
    {
        // creates a new array to store history in
        history = new List<PointInTime>();
    }

    private void FixedUpdate()
    {
        if (TimeManager.isRewinding())
            Rewind();
        else if (!TimeManager.isPaused())
            Record(); // only records if its not rewinding and if its not paused
    }

    // function for rewinding time based objects to their previous positions and states
    private void Rewind()
    {
        // only rewinds as far as there is history left
        if (history.Count > 0)
        {
            // gets the latest point in history
            PointInTime nextPoint = history[0];

            // the child script deals with applying the data from the latest point in history
            ApplyRewindData(nextPoint);

            // removes the latest point of history
            history.RemoveAt(0);
        }
        else
            // stops rewinding once it runs out of history
            TimeManager.StopReversingTime();
    }

    // abstract function that deals with applying the data from the inserted PIT
    public abstract void ApplyRewindData(PointInTime PIT);

    // records the current state of the object and stores it into the history list
    private void Record()
    {
        // removes the latest history if the array is full
        if (history.Count > Mathf.Round(recordTime / Time.deltaTime))
        {
            history.RemoveAt(history.Count - 1);
        }

        // creates a new PIT based off the objects state and stores it into the first slot in history
        history.Insert(0, CreatePIT());
    }

    // abstract function that creates a specialized PIT for the particular child object
    public abstract PointInTime CreatePIT();
}
