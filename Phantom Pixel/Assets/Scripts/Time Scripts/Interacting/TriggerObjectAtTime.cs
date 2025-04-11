using UnityEngine;

public class TriggerObjectAtTime : MonoBehaviour
{
    [SerializeField]
    private GameObject thingToTrigger;

    [SerializeField]
    private float timeToTrigger;

    private bool canTrigger = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.GetGameTime() >= timeToTrigger)
        {
            if (canTrigger)
            {
                canTrigger = false;
                thingToTrigger.TryGetComponent(out IInteractable interactObj);
                interactObj.Interact();
            }
        }
        else
            canTrigger = true;
    }
}
