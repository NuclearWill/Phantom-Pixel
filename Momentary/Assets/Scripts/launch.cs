using UnityEngine;

public class launch : MonoBehaviour
{
    Rigidbody rb;

    public float force = 5f;
    public float splash = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            rb.AddForce(new Vector3(Random.Range(-splash, splash), force * 100, Random.Range(-splash, splash)));
        }
    }
}
