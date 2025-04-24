using UnityEngine;

public class TriggerObjectAtTime : MonoBehaviour
{
    [SerializeField]
    private float timeToTrigger;

    private bool canTrigger = true;

    void Update()
    {
        if (TimeManager.GetGameTime() >= timeToTrigger)
        {
            if (canTrigger)
            {
                canTrigger = false;
                
                if (TryGetComponent(out IInteractable interactObj))
                    interactObj.Interact();
            }
        }
        else
            canTrigger = true;
    }
}
