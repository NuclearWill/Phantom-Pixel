using UnityEngine;

public class ButtonConsole : TimeBody, IInteractable
{
    [Header("Terminal Settings")]
    [SerializeField]
    [Tooltip("All the objects that the console should activate")]
    private GameObject[] thingsToTrigger;
    [SerializeField]
    [Tooltip("The delay each object should wait before activating after the button is pressed")]
    private float[] triggerDelay;
    [SerializeField]
    private bool canReTrigger = false;

    private bool[] objectTriggered;

    private bool activated = false;
    private float timeActivated;

    private void Start()
    {
        objectTriggered = new bool[thingsToTrigger.Length];


        for (int i = 0; i < thingsToTrigger.Length; i++)
        {
            objectTriggered[i] = false;
        }
    }

    void Update()
    {
        // if the button has been activated, waits until each object's delay has passed before triggering them
        if (activated) 
            for (int i = 0; i < thingsToTrigger.Length; i++)
            {
                // checks if enough time has passed for the delay to be trigger the corresponding interactable
                if(TimeManager.GetGameTime() >= timeActivated + triggerDelay[i] && !objectTriggered[i])
                {
                    objectTriggered[i] = true;
                    thingsToTrigger[i].TryGetComponent(out IInteractable interactObj);
                    interactObj.Interact();
                }
            }
    }

    public void Interact()
    {
        if (!activated)
        {
            Debug.Log("I was pressed!");

            timeActivated = TimeManager.GetGameTime();
            activated = true;
        }
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        ButtonPIT nextPoint = (ButtonPIT)PIT;

        activated = nextPoint.isActivated;
        objectTriggered = nextPoint.objectTriggered;
    }

    public override PointInTime CreatePIT()
    {
        return new ButtonPIT(activated, objectTriggered);
    }
}
