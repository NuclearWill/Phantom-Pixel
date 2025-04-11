using UnityEngine;
using UnityEngine.UIElements;

public class PointInTime
{
    // positional data
    public Vector3 position;
    public Quaternion rotation;

    public PointInTime(Transform transform)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
    }
}

public class ElevatorPIT : PointInTime
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
