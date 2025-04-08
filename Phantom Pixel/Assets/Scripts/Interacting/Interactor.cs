using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public GameObject interactee;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(interactee.TryGetComponent(out IInteractable interactObj))
                interactObj.Interact();
        }
    }
}
