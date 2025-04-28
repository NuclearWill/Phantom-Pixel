using UnityEngine;

public class WaterLevel : Water
{
    [Header("Water Settings")]
    [SerializeField]
    [Tooltip("How many tiles high should the water end up at? Have it less than its starting to shrink and 0 to disappear")]
    private float endWaterLevel = 1f;


    // internal variables
    private bool movingTowardsOrigin = true;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        
        // only changes the water if time is not rewinding or paused, and if the water is supposed to be changing
        if (getMoving() && !TimeManager.isRewinding() && !TimeManager.isPaused())
        {
            // makes sure the destination and starting point is correct (due to time manipulation)
            targetHeight = (movingTowardsOrigin) ? initialWaterLevel : endWaterLevel;
            startingHeight = (!movingTowardsOrigin) ? initialWaterLevel : endWaterLevel;

            ShiftWaterLevel();
        }
    }

    public override void Interact()
    {
        // makes sure the water can't be triggered while its already active
        if (!getMoving())
        {
            // sets the water up to begin changing
            Debug.Log("water level changing");
            elapsedTime = 0f;
            setMoving(true);

            movingTowardsOrigin = !movingTowardsOrigin;
        }
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        base.ApplyRewindData(PIT);

        WaterLevelPIT nextPoint = (WaterLevelPIT)PIT;

        movingTowardsOrigin = nextPoint.movingTowardsOrigin;
    }

    public override PointInTime CreatePIT()
    {
        return new WaterLevelPIT(waterLevel, getMoving(), movingTowardsOrigin, elapsedTime);
    }
}
