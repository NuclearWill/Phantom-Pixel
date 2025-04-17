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
    public bool isMoving, movingTowardsOrigin;
    public float elapsedTime;

    public ElevatorPIT(Transform transform, bool moving, bool movingTowardsOrigin, float elapsedTime) : base(transform)
    {
        this.isMoving = moving;
        this.movingTowardsOrigin = movingTowardsOrigin;
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

public class WaterPIT : PointInTime
{
    public float waterLevel, elapsedTime;

    public bool isMoving;

    public WaterPIT(float waterLevel, bool isMoving, float elapsedTime)
    {
        this.waterLevel = waterLevel;
        this.isMoving = isMoving;
        this.elapsedTime = elapsedTime;
    }
}

public class WaterLevelPIT : WaterPIT
{
    public bool movingTowardsOrigin;

    public WaterLevelPIT(float waterLevel, bool isMoving, bool movingTowardsOrigin, float elapsedTime) : base(waterLevel, isMoving, elapsedTime)
    {
        this.movingTowardsOrigin = movingTowardsOrigin;
    }
}

public class WaterFloodPIT : WaterPIT
{
    public float timeUntilNextFall;

    public WaterFloodPIT(float waterLevel, bool isMoving, float timeUntilNextFall, float elapsedTime) : base (waterLevel, isMoving, elapsedTime)
    {
        this.timeUntilNextFall = timeUntilNextFall;
    }
}

public class DoorPIT : PointInTime
{
    public bool open;

    public DoorPIT(bool open)
    {
        this.open = open;
    }
}
