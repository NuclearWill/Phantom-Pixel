using UnityEngine;

public class PointInTime
{
    public Vector3 position;
    public Quaternion rotation;

    public Vector3 linearVelocity;
    public Vector3 angularVelocity;

    public PointInTime(Transform transform, Rigidbody rb)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
        this.linearVelocity = rb.linearVelocity;
        this.angularVelocity = rb.angularVelocity;
    }
}
