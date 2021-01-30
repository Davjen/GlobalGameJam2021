using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputWithRB : MonoBehaviour
{
    private Rigidbody rb;
    public Transform center;
    public float grav;
    public bool gravOn = true;
    public float speed;
    public float jumpForce;
    public float FloatingMultiplier=1f;
    public float GroundingMultiplier = 1f;
    bool jump;
    bool grounded;
    float fwd;
    Quaternion platformRot;
    float colliderSize;
    bool floating;
    Vector3 startScale;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        colliderSize = GetComponentInChildren<CapsuleCollider>().bounds.extents.z;
       
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
                rb.AddForce(-dir * grav, ForceMode.Acceleration);

                //Add orientation
                rb.rotation = Quaternion.LookRotation(dir);
            }

            //add translation
            rb.AddForce(transform.right * fwd);

            if (!floating)
                rb.rotation = Quaternion.Slerp(transform.rotation, platformRot, Time.deltaTime * GroundingMultiplier).normalized;

            if (!grounded && floating)
                rb.rotation = Quaternion.Slerp(transform.rotation, platformRot, Time.deltaTime * FloatingMultiplier).normalized;


            //add jump force
            if (jump && grounded)
            {

                rb.AddForce(dir * jumpForce, ForceMode.Impulse);
                jump = false;
            }

        

    }

    private void Update()
    {

        fwd = Input.GetAxis("Horizontal")*speed;


        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            jump = true;

        //Allining with platform
       

    }

    private void OnCollisionEnter(Collision collision)
    {

        float myDist = (center.position - transform.position).magnitude;
        float platformDist = 0;
        if (collision.transform.childCount!=0)
        {
            platformDist = (center.position - collision.transform.GetChild(0).position).magnitude;

        }

        Debug.Log(platformDist);
        if ((myDist + colliderSize-0.1f <= platformDist) || collision.gameObject.tag == "WashingMachine")
        {
           // transform.SetParent(collision.transform);
            gravOn = false;
            grounded = true;

            if (collision.gameObject.tag == "WashingMachine")
            {
                floating = false;
                LookCenter();
            }


        }
        //rb.velocity = Vector3.zero;

        //if (collision.gameObject.tag == "WashingMachine")
        //{
        //    floating = false;
        //    Vector3 dir = (center.position - transform.position).normalized;
        //    platformRot = Quaternion.FromToRotation(transform.forward, dir);

        //}

    }

    private void OnTriggerEnter(Collider other)
    {
       


        floating = false;
        platformRot = Quaternion.LookRotation(other.transform.forward);


    }

    private void OnCollisionStay(Collision collision)
    {
        float myDist = (center.position - transform.position).magnitude;

        float platformDist=0;
        if (collision.transform.childCount!=0)
        {
            platformDist = (center.position - collision.transform.GetChild(0).position).magnitude;

        }
        Debug.Log(platformDist);

        PlatformColliderSize platformSize;
        collision.gameObject.TryGetComponent<PlatformColliderSize>(out platformSize);


            //N.B. useful if you don't wanna jump again when you touch a platform from edges
            if(collision.gameObject.tag != "WashingMachine" && collision.transform != transform.parent && (myDist + colliderSize - 0.1f >= platformDist))
                grounded = false;
            else
            {
                grounded = true;

                if (collision.gameObject.tag == "WashingMachine")
                    LookCenter();

            }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
           floating = true;
            LookCenter();
    }
    private void OnCollisionExit(Collision collision)
    {
        //transform.SetParent(null);
        //transform.localScale = startScale;
        gravOn = true;
        grounded = false;

        if (collision.gameObject.tag == "WashingMachine")
        {
            floating = true;
            LookCenter();
        }
        //platformRot = Quaternion.identity;

    }

    public void LookCenter()
    {
        Vector3 dir = (center.position - transform.position).normalized;
        platformRot = Quaternion.LookRotation(dir);
    }
}
