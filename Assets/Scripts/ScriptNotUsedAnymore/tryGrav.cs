using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tryGrav : MonoBehaviour
{
    private Rigidbody rb;
    public Transform center;
    public float grav;
    public bool gravOn = true;
    public float speed;
    public float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dir = (center.position - transform.position).normalized;
        if (gravOn)
        {
            rb.AddForce(-dir * grav, ForceMode.Acceleration);
        }
        float x = Input.GetAxis("Horizontal");

        rb.rotation = Quaternion.LookRotation(dir);
        rb.AddForce(transform.right * x * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(dir * jumpForce, ForceMode.Impulse);
        }
        

    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        gravOn = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        gravOn = true;
    }
}
