using UnityEngine;
using UnityEngine.UIElements;

public class PointInTime
{

}

public class MovingPIT : PointInTime
{
    // positional data
    public Vector3 position;
    public Quaternion rotation;

    public MovingPIT(Transform transform)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
    }
}

public class ElevatorPIT : MovingPIT
{
    // elevator specific data
    public bool isMoving;
    public bool isAtStartingLocation;
    public float elapsedTime;

    public ElevatorPIT(Transform transform, bool moving, bool startingLocal, float elapsedTime) : base(transform)
    {
        this.isMoving = moving;
        this.isAtStartingLocation = startingLocal;
        this.elapsedTime = elapsedTime;
    }
}

public class ButtonPIT : PointInTime
{
    public bool isActivated;
    public bool[] objectTriggered;

    public ButtonPIT(bool isActivated, bool[] inputTriggers)
    {
        this.isActivated = isActivated;
        this.objectTriggered = new bool[inputTriggers.Length];
        for (int i = 0; i < objectTriggered.Length; i++)
        {
            this.objectTriggered[i] = inputTriggers[i];
        }
    }
}
