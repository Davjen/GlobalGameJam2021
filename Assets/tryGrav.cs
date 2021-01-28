using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tryGrav : MonoBehaviour
{
    private Rigidbody rb;
    public Transform center;
    public float grav;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dir = (center.position - transform.position).normalized;
        rb.AddForce(-dir * grav);

        float x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(x * speed, 0, 0);

        transform.rotation = Quaternion.LookRotation(dir);
        

    }
}
