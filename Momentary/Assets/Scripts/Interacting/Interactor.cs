using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of Interactables to Manually Test")]
    private GameObject[] interactee;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach(GameObject obj in interactee)
            {
                if (obj.TryGetComponent(out IInteractable interactObj))
                    interactObj.Interact();
            }
        }
    }
}
