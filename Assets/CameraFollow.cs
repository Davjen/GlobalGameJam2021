using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float FadeOutDistance = 6;
    public float FadeOutSpeed = 3;
    public float minDistFromTarget = 4;
    public float maxDistFromTarget;
    private float currDist;
    public KeyCode switchCamera = KeyCode.W;
    private float newDist;
    private bool moveToNewPos;
    private bool near = true;

    // Start is called before the first frame update
    void Start()
    {
        currDist = minDistFromTarget;
        maxDistFromTarget = currDist + minDistFromTarget;
        transform.position = target.position + (Vector3.up * currDist);
    }



    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(switchCamera))
        {
            if (near)
            {
                newDist = maxDistFromTarget;
            }
            else
            {
                newDist = minDistFromTarget;
            }
        }

        currDist = Vector3.Distance(transform.position, target.position);
        if (currDist != newDist)
        {
            transform.position = Vector3.Lerp(transform.position,target.position + (Vector3.up * newDist) ,FadeOutSpeed);
        }
    }
}
