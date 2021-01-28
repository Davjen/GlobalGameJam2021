using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Fwd,Speed, JumpForce;
    public GravityAffectedScript GScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Fwd = Input.GetAxis("Horizontal") * Speed;

        //MOVEMENT
        transform.position += transform.right * Fwd * Speed * Time.deltaTime;
        //Debug.DrawRay(transform.position, GScript.GetDir().normalized*10,Color.red);
        if (Input.GetKey(KeyCode.Space))
        {
            //VALUTARE SE MIGLIORARE LA QUESTIONE DIRECTION
            Vector3 dir = GScript.GetDir().normalized;
            //esegue il jump usando dir.
            transform.position += JumpForce * dir * Time.deltaTime;
        }
    }
}
