using NUnit.Framework;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    private bool isRewinding = false;

    public float recordTime = 5f;

    List<PointInTime> history;

    Rigidbody rb;

    private void Start()
    {
        history = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            StartRewind();
        if (Input.GetKeyUp(KeyCode.Q))
            StopRewind();
    }

    private void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
    }

    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
    }

    public void StopRewind()
    {
        if (isRewinding)
        {
            isRewinding = false;
            rb.isKinematic = false;

            // applies the linear and angular velocity of the latest point in history when time resumes
            rb.linearVelocity = history[0].linearVelocity;
            rb.angularVelocity = history[0].angularVelocity;
        }
    }

    private void Rewind()
    {
        if (history.Count > 1)
        {
            PointInTime nextPoint = history[0];

            // applies the position and the rotation of the point in history as its actively rewinding
            transform.position = nextPoint.position;
            transform.rotation = nextPoint.rotation;

            history.RemoveAt(0);
        }
        else
            StopRewind();
    }

    private void Record()
    {
        if (history.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            history.RemoveAt(history.Count - 1);
        }
        history.Insert(0, new PointInTime(transform, rb));
    }
}
