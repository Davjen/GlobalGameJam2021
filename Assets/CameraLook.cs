using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraLook : MonoBehaviour
{
    public Transform CameraRotation;
    public float mindDist, MaxDist;
    public float Speed = 7;
    private bool near = false;

    private float newDist;
    private bool instaSwap;

    public void SwitchDistance()
    {
        if (near)
        {
            newDist = mindDist;
        }
        else
        {
            newDist = MaxDist;
        }
        near = !near;
        //transform.localPosition = new Vector3(-distance, transform.localPosition.y, transform.localPosition.z);
    }

    private void Start()
    {
        newDist = mindDist;
        transform.rotation = CameraRotation.rotation;
        instaSwap = true;
    }

    private void Update()
    {
        if (instaSwap)
        {
            instaSwap = false;
            SwitchDistance();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchDistance();
        }
    }
    private void FixedUpdate()
    {

        //Quaternion newRot = Quaternion.Euler(new Vector3(startRot.x,CameraRotation.rotation.y,CameraRotation.rotation.z));
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-transform.localPosition.x, -newDist, transform.localPosition.z), Speed * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, CameraRotation.rotation, Speed * Time.deltaTime);
        

    }
}
