using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float minDist, maxDist;
    public float lerpSpeed = 6;


    private Transform cameraTarget;
    private float diff;
    private bool lerp;
    private bool canLerp = true;
    private Vector3 newPos;
    private bool near = true;

    // Start is called before the first frame update
    void Start()
    {
        diff = maxDist - minDist;
        cameraTarget = new GameObject("camera_target").transform;
        transform.SetParent(cameraTarget);
        cameraTarget.position = player.position;
        transform.position = cameraTarget.position + new Vector3(0,minDist,0);
    }



    // Update is called once per frame
    void Update()
    {
        Vector3 dist = player.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(dist,Vector3.up);
        transform.forward = -Vector3.up;
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            lerp = true;
            if (near)
            {
                newPos = cameraTarget.position + new Vector3(0, maxDist, 0);
                near = false;
            }
            else
            {
                newPos = cameraTarget.position + new Vector3(0, minDist, 0);
                near = true;
            }
        }

        if (lerp)
        {
            transform.position = Vector3.Lerp(transform.position, newPos,lerpSpeed * Time.deltaTime);
            if (Mathf.Approximately(transform.position.y, newPos.y))
            {
                lerp = false;
            }
        }
    }
}
