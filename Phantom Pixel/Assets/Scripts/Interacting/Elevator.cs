using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Elevator : MonoBehaviour, IInteractable
{
    
    // destinations for elevator
    private Vector3 location1;
    private Vector3 location2;

    [Header("Elevator Settings")]
    [SerializeField]
    [Tooltip("The number of tiles the elevator goes up or down when activated. Positive numbers to go up, negative to go down")]
    private int destinationHeight;
    public float timeToMove = 1f;

    private bool isMoving = false;
    private bool atStartingLocation = true;

    private void Start()
    {
        // sets the starting and ending locations of the elevator
        location1 = transform.position;
        location2 = new Vector3(location1.x, location1.y + (2 * destinationHeight), location1.z);
    }

    public void Interact()
    {
        // makes sure the elevator can't be triggered unless if it is already done moving
        if (!isMoving)
        {
            Debug.Log("elevator activate");
            StartCoroutine(moveElevator());
        }
    }

    private IEnumerator moveElevator()
    {
        // sets up the elevator to begin moving
        isMoving = true;

        float elapsedTime = 0f;

        Vector3 targetLocation = (atStartingLocation) ? location2 : location1;
        Vector3 startingLocation = (!atStartingLocation) ? location2 : location1;

        // checks to see if anything is on the elevator and saves that object for later use
        Ray ray = new Ray(transform.position, Vector3.up);
        GameObject elevatorUser = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 1.4f))
        {
            elevatorUser = hit.transform.gameObject;
        }

        // actively moving the elevator
        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startingLocation, targetLocation, (elapsedTime / timeToMove));

            // moves the object on top of the elevator
            if(elevatorUser != null)
            {
                elevatorUser.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // finishing up after done moving
        atStartingLocation = !atStartingLocation;

        transform.position = targetLocation;

        isMoving = false;
    }
}
