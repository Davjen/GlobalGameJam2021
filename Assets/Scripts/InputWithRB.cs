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
    public float RepulseForce = 1;
    public float speed;
    public float jumpForce;
    public float FloatingMultiplier = 1f;
    public float GroundingMultiplier = 1f;
    public ForceMode RepulseForceType = ForceMode.Force;
    bool jump;
    bool grounded;
    float fwd;
    Animator anim;
    Quaternion platformRot;
    //float colliderSize;
    bool floating;
    bool recordInput;

    private int Lifes;
    private bool repulse;
    private bool turnLeft;
    private bool turnRight;

    void Start()
    {

        turnRight = true;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        //colliderSize = GetComponentInChildren<CapsuleCollider>().bounds.extents.z;
        //Debug
        recordInput = true;
        floating = true;
        Lifes = 3;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (recordInput)
        {
            anim.SetBool("Walking", true);
            
            if (fwd > 0)
            {
                if (!turnLeft)
                {
                    turnLeft = true;
                    turnRight = false;
                    transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

                }
            }
            else if(fwd<0)
            {
                if (!turnRight)
                {
                    turnLeft = false;
                    turnRight = true;
                    transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

                }
            }
            else
            {
                anim.SetBool("Walking", false);
            }


            Vector3 dir = (center.position - transform.position).normalized;
            if (gravOn)
            {
                //Add Gravity
                rb.AddForce(-dir * grav, ForceMode.Acceleration);

                //Add orientation
                rb.rotation = Quaternion.LookRotation(dir, transform.up);
            }




           
            
           
            //add translation
            rb.AddForce(transform.up * fwd);
            LookCenter();


            if (jump && grounded)
            {

                rb.AddForce(dir * jumpForce, ForceMode.Impulse);
                jump = false;
                anim.SetTrigger("Jump");
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

            fwd = -(Input.GetAxis("Horizontal") * speed);


            if (Input.GetKeyDown(KeyCode.Space) && grounded)
                jump = true;

        }

        //Allining with platform


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DangerousObj")
        {
            //Vite-1
            Lifes--;
            //if vite ==0 - Gameover.
            if (Lifes==0)
            {
                //Trigger gameover;
                //return;
            }
           
        }
        float myDist = (center.position - transform.position).magnitude;
        float platformDist = 0;
        if (collision.transform.childCount != 0)
        {
            platformDist = (center.position - collision.transform.GetChild(0).position).magnitude;

        }

        if ((myDist -0.1f <= platformDist) || collision.gameObject.tag == "WashingMachineInternal"|| collision.gameObject.tag == "WashingMachineExternal")
        {

            //anim.SetTrigger("EndJump");
            //currentScale = transform.localScale;
            gravOn = false;
            grounded = true;

            if (collision.gameObject.tag == "WashingMachineInternal"|| collision.gameObject.tag == "WashingMachineExternal")
            {
                floating = false;
                LookCenter();
                transform.SetParent(collision.transform);
                anim.SetTrigger("EndJump");


            }
            else
            {
                transform.SetParent(collision.transform.parent);
                anim.SetTrigger("EndJump");

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
        if (collision.gameObject.tag != "WashingMachineInternal" && collision.gameObject.tag != "WashingMachineExternal" && collision.transform != transform.parent && (myDist -0.2f >= platformDist))
        {
            grounded = false;
            repulse = true;
            Vector3 repulseDir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(repulseDir * RepulseForce, RepulseForceType);
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
        if(!repulse)
            transform.SetParent(null);

        repulse = false;
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
        platformRot = Quaternion.LookRotation(dir, transform.up);

    }
}
