using UnityEngine;

public class MovingPIT : DynamicPIT
{
    // positional data
    public Vector3 position;
    public Quaternion rotation;

    // time data
    public float elapsedTime;

    public MovingPIT(bool isMoving, Transform transform, float elapsedTime) : base(isMoving)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
        this.elapsedTime = elapsedTime;
    }
}

public class ElevatorPIT : MovingPIT
{
    // elevator specific data
    public bool movingTowardsOrigin;

    public ElevatorPIT(Transform transform, bool isMoving, bool movingTowardsOrigin, float elapsedTime) : base(isMoving, transform, elapsedTime)
    {
        this.movingTowardsOrigin = movingTowardsOrigin;
    }
}

public class DoorPIT : MovingPIT
{
    // door specific data
    public bool open;

    public DoorPIT(Transform transform, bool isMoving, bool open, float elapsedTime) : base(isMoving, transform, elapsedTime)
    {
        this.open = open;
    }
}