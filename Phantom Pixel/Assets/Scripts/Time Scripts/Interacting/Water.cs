using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Water : TimeBody, IInteractable
{
    [Header("Water Body Reference")]
    public GameObject WaterBody;


    // internal variables
    [NonSerialized]
    public bool isMoving;
    [NonSerialized]
    public float waterLevel, elapsedTime = 0f, startingHeight, targetHeight;
    [NonSerialized]
    public Vector3 startingPosition;

    [Tooltip("How many tiles should the water start at? Have it at 0 to start empty")]
    public float initialWaterLevel = 0f;
    public float waterFillSpeed = 1f;

    private void Start()
    {
        waterLevel = initialWaterLevel;

        UpdateWaterLevel(waterLevel);
    }

    public abstract void Interact();

    public override void ApplyRewindData(PointInTime PIT)
    {
        WaterPIT nextPoint = (WaterPIT)PIT;

        isMoving = nextPoint.isMoving;
        elapsedTime = nextPoint.elapsedTime;

        UpdateWaterLevel(nextPoint.waterLevel);
    }

    public override PointInTime CreatePIT()
    {
        return new WaterPIT(waterLevel, isMoving, elapsedTime);
    }

    public void UpdateWaterLevel(float newLevel)
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
            WaterBody.transform.localPosition = new Vector3(0, -waterLevel - 0.01f, 0);
        }
    }

    public void ShiftWaterLevel()
    {
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
