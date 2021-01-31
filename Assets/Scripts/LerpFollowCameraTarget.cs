using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpFollowCameraTarget : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position,speed*Time.deltaTime);
    }
}
