using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Transform Origin;
    public float Speed, JumpForce;
    public GravityAffectedScript GScript;
    private float Fwd;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Fwd += Input.GetAxis("Horizontal") * Speed * Time.deltaTime;

        Quaternion qx = Quaternion.AngleAxis(Fwd, transform.right);
        transform.rotation = qx;

        transform.position = Origin.position - (transform.rotation * Vector3.forward * 5);

        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 dir = Origin.position - transform.position;
            transform.position += JumpForce * dir * Time.deltaTime;
        }
    }
}
