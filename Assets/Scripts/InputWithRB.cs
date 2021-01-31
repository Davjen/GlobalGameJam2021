using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;


public class InputWithRB : MonoBehaviour
{
    private Rigidbody rb;
    public Transform center;
    public float grav;
    public bool gravOn = true;
    public float speed;
    public float jumpForce;
    public float FloatingMultiplier = 1f;
    public float GroundingMultiplier = 1f;
    bool jump;
    bool grounded;
    float fwd;
    Quaternion platformRot;
    float colliderSize;
    bool floating;
    bool recordInput;
    Vector3 startScale;
    //Vector3 currentScale;


    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        colliderSize = GetComponentInChildren<CapsuleCollider>().bounds.extents.z;
        //Debug
        recordInput = true;
        floating = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (recordInput)
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
            LookCenter();


            if (jump && grounded)
            {

                rb.AddForce(dir * jumpForce, ForceMode.Impulse);
                jump = false;
            }

            if (!floating)
                rb.rotation = Quaternion.Slerp(transform.rotation, platformRot, Time.deltaTime * GroundingMultiplier).normalized;

            if (!grounded && floating)
                rb.rotation = Quaternion.Slerp(transform.rotation, platformRot, Time.deltaTime * FloatingMultiplier).normalized;

        }

        //add jump force



    }

    public void RecordInput(bool b)
    {
        recordInput = b;
    }
    private void Update()
    {
        if (recordInput)
        {
            fwd = Input.GetAxis("Horizontal") * speed;


            if (Input.GetKeyDown(KeyCode.Space) && grounded)
                jump = true;

        }

        //Allining with platform


    }

    private void OnCollisionEnter(Collision collision)
    {

        float myDist = (center.position - transform.position).magnitude;
        float platformDist = 0;
        if (collision.transform.childCount != 0)
        {
            platformDist = (center.position - collision.transform.GetChild(0).position).magnitude;

        }

        if ((myDist + colliderSize - 0.1f <= platformDist) || collision.gameObject.tag == "WashingMachineInternal"|| collision.gameObject.tag == "WashingMachineExternal")
        {
            
            //currentScale = transform.localScale;
            gravOn = false;
            grounded = true;

            if (collision.gameObject.tag == "WashingMachineInternal"|| collision.gameObject.tag == "WashingMachineExternal")
            {
                floating = false;
                LookCenter();
                transform.SetParent(collision.transform);

            }
            else
            {
                transform.SetParent(collision.transform.parent);
            }
           
        }

    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag != "CentralButton")
        {
            floating = false;
            platformRot = Quaternion.LookRotation(other.transform.forward);
        }
        else
        {
              WashingMachineMgr.StopMotion();
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        float myDist = (center.position - transform.position).magnitude;

        float platformDist = 0;
        if (collision.transform.childCount != 0)
        {
            platformDist = (center.position - collision.transform.GetChild(0).position).magnitude;

        }

        PlatformColliderSize platformSize;
        collision.gameObject.TryGetComponent<PlatformColliderSize>(out platformSize);

        //N.B. useful if you don't wanna jump again when you touch a platform from edges
        if (collision.gameObject.tag != "WashingMachineInternal" && collision.gameObject.tag != "WashingMachineExternal" && collision.transform != transform.parent && (myDist + colliderSize - 0.1f >= platformDist))
        {
            grounded = false;
            gravOn = true;
        }
        else
        {
            grounded = true;
            LookCenter();

            //if (collision.gameObject.tag == "WashingMachine")
            //transform.localScale = currentScale;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "CentralButton")
        {
            floating = true;
            LookCenter();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        transform.SetParent(null);
        

        //transform.localScale = startScale;
        gravOn = true;
        grounded = false;

        if (collision.gameObject.tag == "WashingMachineInternal" || collision.gameObject.tag == "WashingMachineExternal")
        {
            floating = true;
            LookCenter();
        }

    }

    public void LookCenter()
    {
        Vector3 dir = (center.position - transform.position).normalized;
        platformRot = Quaternion.LookRotation(dir);
    }
}
