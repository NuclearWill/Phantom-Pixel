using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterFlooding : Water
{
    [SerializeField]
    [Tooltip("The height of tiles the water will rise")]
    private float fillAmount = 2f;
    [SerializeField]
    [Tooltip("The time before the water fills up again")]
    private float fillDelay = 4f;

    // internal variables
    private float timeUntilNextFill;

    // calculates the number of times the water level has risen
    private int fillCounter => (int) Mathf.Floor(TimeManager.GetGameTime() / fillDelay);
    
    // calculates the corrent starting height based off the number of times the water has risen
    private float startHeightCalc => initialWaterLevel + (fillAmount * (fillCounter - 1));

    // calculates the corrent starting height based off the number of times the water has risen
    private float targetHeightCalc => initialWaterLevel + (fillAmount * (fillCounter));

    private void Start()
    {
        timeUntilNextFill = fillDelay;
        
        startingPosition = transform.position;
    }

    private void Update()
    {
        // passively begins flooding the room except while time is rewinding
        if (TimeManager.GetGameTime() > timeUntilNextFill && !TimeManager.isRewinding())
        {
            timeUntilNextFill += fillDelay;

            // sets the water up to begin rising
            Debug.Log("water level rising");
            elapsedTime = 0f;
            isMoving = true;
        }

        // only changes the water if time is not rewinding or paused, and if the water is supposed to be changing
        if (isMoving && !TimeManager.isRewinding() && !TimeManager.isPaused())
        {
            startingHeight = startHeightCalc;
            targetHeight = targetHeightCalc;

            ShiftWaterLevel();
        }
    }

    public override void Interact()
    {
        // beat the level
        Debug.Log("You Win!");
        SceneManager.LoadScene(0);
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        base.ApplyRewindData(PIT);

        WaterFloodPIT nextPoint = (WaterFloodPIT)PIT;

        timeUntilNextFill = nextPoint.timeUntilNextFall;
    }

    public override PointInTime CreatePIT()
    {
        return new WaterFloodPIT(waterLevel, isMoving, timeUntilNextFill, elapsedTime);
    }
}
