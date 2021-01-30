using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputWithKinematic : MonoBehaviour
{
    private Rigidbody rb;
    public Transform center;
    public float grav;
    public bool gravOn = true;
    public float speed;
    public float jumpForce;
    public float RotMultiplier=1f;
    bool jump;
    public bool grounded;
    float fwd;
    public Quaternion platformRot;
    public bool floating;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Debug
        floating = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dir = (center.position - transform.position).normalized;
        if (gravOn)
        {
            //Add Gravity
            transform.position+=-dir * grav;

            //Add orientation
            rb.rotation = Quaternion.LookRotation(dir);
        }
       
        //add translation
        rb.transform.position+=transform.right * fwd;


        if (!floating)
            rb.rotation = Quaternion.Slerp(transform.rotation, platformRot, Time.deltaTime * RotMultiplier).normalized;

        if (!grounded && floating)
            rb.rotation = Quaternion.Slerp(transform.rotation, platformRot, Time.deltaTime * RotMultiplier).normalized;


        //add jump force
        if (jump && grounded)
        {

            rb.transform.position+=dir * jumpForce;
            jump = false;
        }



    }

    private void Update()
    {

        fwd = Input.GetAxis("Horizontal")*speed*Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            jump = true;

        //Allining with platform
       

    }

    private void OnCollisionEnter(Collision collision)
    {
        //rb.velocity = Vector3.zero;
        gravOn = false;
        grounded = true;

        if (collision.gameObject.tag == "WashingMachine")
        {
            floating = false;
            Vector3 dir = (center.position - transform.position).normalized;
            platformRot = Quaternion.LookRotation(dir);
        }

        //transform.SetParent(collision.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        floating = false;
        platformRot = Quaternion.LookRotation(other.transform.forward);

    }

    private void OnTriggerExit(Collider other)
    {
           floating = true;
            Vector3 dir = (center.position - transform.position).normalized;
            platformRot = Quaternion.LookRotation(dir);
    }
    private void OnCollisionExit(Collision collision)
    {
        gravOn = true;
        grounded = false;

        if (collision.gameObject.tag == "WashingMachine")
        {
            floating = true;
            Vector3 dir = (center.position - transform.position).normalized;
            platformRot = Quaternion.LookRotation(dir);
        }
        //platformRot = Quaternion.identity;
        //transform.SetParent(null);

    }
}
