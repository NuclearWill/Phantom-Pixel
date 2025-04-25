using UnityEngine;

public class RewindFailsafe : MonoBehaviour
{
    [SerializeField]
    private float raycastDistance = 2f;
    [SerializeField]
    private bool debug = false;

    void Update()
    {
        /*
         * if the player is rewinding time, check if there is a dynamic object below the player
         * then checks if that object is about to move in its next point in time
         */
        if (!debug && TimeManager.isRewinding() && // checks if the player is rewinding time
            Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hit1, raycastDistance) && // shoots a ray downwards to see if anything is below the player
            hit1.transform.gameObject.TryGetComponent<DynamicObject>(out DynamicObject dynObject1) && // checks if the object below is a dynamic object
            ((DynamicPIT) dynObject1.GetPointInHistory(1)).isMoving) // checks if the next point in time of the object below is moving
        {
            // if the object below is about to move, stop rewinding time
            Debug.Log("Object below is about to move, aborting rewind");
            TimeManager.StopReversingTime();
        }
        
        // uses this code path with debug logs on
        else if (TimeManager.isRewinding()) {
            Debug.Log("Rewinding time");
            if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hit2, raycastDistance)) {
                Debug.Log("Hit " + hit2.transform.gameObject.name);
                if (hit2.transform.gameObject.TryGetComponent<DynamicObject>(out DynamicObject dynObject2)) {
                    Debug.Log("Hit a dynamic object");
                    if (((DynamicPIT)dynObject2.GetPointInHistory(1)).isMoving) {
            
                        // if the object below is about to move, stop rewinding time
                        Debug.Log("Object below is about to move, aborting rewind");
                        TimeManager.StopReversingTime();
                    }
                }
            }
        }
        
    }
}
