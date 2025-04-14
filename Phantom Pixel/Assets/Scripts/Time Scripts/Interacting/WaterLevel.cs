using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class WaterLevel : TimeBody, IInteractable
{
    [Header("Water Body Reference")]
    public GameObject WaterBody;
    

    [Header("Water Settings")]
    [SerializeField]
    [Tooltip("How many tiles should the water start at? Have it at 0 to start empty")]
    private float initialWaterLevel = 0f;
    [SerializeField]
    [Tooltip("How many tiles high should the water end up at? Have it less than its starting to shrink and 0 to disappear")]
    private float endWaterLevel = 1f;
    [SerializeField]
    private float waterFillSpeed = 1f;


    // internal variables
    private float waterLevel, elapsedTime = 0f;
    private bool isMoving, movingTowardsOrigin = true;

    private Vector3 startingPosition;

    private void Start()
    {
        waterLevel = initialWaterLevel;

        startingPosition = transform.position;

        UpdateWaterLevel(waterLevel);
    }

    private void Update()
    {
        
        // only changes the water if time is not rewinding or paused, and if the water is supposed to be changing
        if (isMoving && !TimeManager.isRewinding() && !TimeManager.isPaused())
        {
            // makes sure the destination and starting point is correct (due to time manipulation)
            float targetHeight = (movingTowardsOrigin) ? initialWaterLevel : endWaterLevel;
            float startingHeight = (!movingTowardsOrigin) ? initialWaterLevel : endWaterLevel;

            // changes the position according to elapsed time
            UpdateWaterLevel(Mathf.Lerp(startingHeight, targetHeight, (elapsedTime / waterFillSpeed)));

            elapsedTime += Time.deltaTime;

            // finishes after done moving
            if (elapsedTime > waterFillSpeed)
            {
                UpdateWaterLevel(targetHeight);
                
                isMoving = false;
            }
        }
    }

    public void Interact()
    {
        // makes sure the water can't be triggered while its already active
        if (!isMoving)
        {
            // sets the water up to begin changing
            Debug.Log("water level changing");
            elapsedTime = 0f;
            isMoving = true;

            movingTowardsOrigin = !movingTowardsOrigin;
        }
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        WaterPIT nextPoint = (WaterPIT)PIT;

        isMoving = nextPoint.isMoving;
        movingTowardsOrigin = nextPoint.movingTowardsOrigin;
        elapsedTime = nextPoint.elapsedTime;

        UpdateWaterLevel(nextPoint.waterLevel);
    }

    public override PointInTime CreatePIT()
    {
        return new WaterPIT(waterLevel, isMoving, movingTowardsOrigin, elapsedTime);
    }

    private void UpdateWaterLevel(float newLevel)
    {
        waterLevel = newLevel;
        
        // this transform
        transform.position = new Vector3(startingPosition.x, startingPosition.y + (waterLevel * 2), startingPosition.z);

        // water body transform
        if (waterLevel == 0)
            WaterBody.SetActive(false);
        else
        {
            WaterBody.SetActive(true);
            WaterBody.transform.localScale = new Vector3(WaterBody.transform.localScale.x, waterLevel * 2, WaterBody.transform.localScale.z);
            WaterBody.transform.localPosition = new Vector3(0, -waterLevel, 0);
        }
    }
}
