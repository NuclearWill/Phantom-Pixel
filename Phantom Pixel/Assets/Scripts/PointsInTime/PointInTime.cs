using UnityEngine;
using UnityEngine.UIElements;

public class PointInTime
{

}

public class DynamicPIT : PointInTime
{
    public bool isMoving;

    public DynamicPIT(bool isMoving)
    {
        this.isMoving = isMoving;
    }
}

public class MovingPIT : DynamicPIT
{
    // positional data
    public Vector3 position;
    public Quaternion rotation;

    public MovingPIT(bool isMoving, Transform transform) : base(isMoving)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
    }
}

public class ElevatorPIT : MovingPIT
{
    // elevator specific data
    public bool movingTowardsOrigin;
    public float elapsedTime;

    public ElevatorPIT(Transform transform, bool isMoving, bool movingTowardsOrigin, float elapsedTime) : base(isMoving, transform)
    {
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

public class WaterPIT : DynamicPIT
{
    public float waterLevel, elapsedTime;

    public WaterPIT(float waterLevel, bool isMoving, float elapsedTime) : base(isMoving)
    {
        this.waterLevel = waterLevel;
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
    public int fillCounter;

    public WaterFloodPIT(float waterLevel, bool isMoving, float timeUntilNextFall, int fillCounter, float elapsedTime) : base (waterLevel, isMoving, elapsedTime)
    {
        this.timeUntilNextFall = timeUntilNextFall;
        this.fillCounter = fillCounter;
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
