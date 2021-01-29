using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public Transform origin;
    public float TimeOfPositioning;
    Vector3 PlatformNewDestination;
    Vector3 PreviousPosition;
    bool goToDestination;
    Collider myCollider;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = transform.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        //orientare verso il centro!
       
        if (goToDestination)
        {
            Vector3 Dir = origin.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Dir);
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(PreviousPosition, PlatformNewDestination, timer/TimeOfPositioning);
            if ((transform.position - PlatformNewDestination).magnitude < 0.05f)
            {
                
                myCollider.enabled = true;
                goToDestination = false;
                timer = 0;
            }
        }
    }
    public void SetPlatformDestination(Vector3 destination)
    {
        myCollider.enabled = false;
        PlatformNewDestination = destination;
        PreviousPosition = transform.position;
        goToDestination = true;

    }
}
