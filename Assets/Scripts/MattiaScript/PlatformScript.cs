using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlatformScript : MonoBehaviour
{
   
    public Transform Center;
    public WashingMachineMgr Owner { get; protected set; }

    Vector3 startPosition;
    Vector3 PlatformNewDestination;
    Vector3 PreviousPosition;
    bool goToDestination;
    bool isGravityAffected;
    Collider[] myColliders= new Collider[2];
    float timer=0;

    List<Vector3> StoredPosition = new List<Vector3>();

    
    // Start is called before the first frame update
    public void Init()
    {
        startPosition = transform.position;
        enabled = true;
        myColliders = transform.GetComponents<Collider>();
    }

    private void OnEnable()
    {
        if(Owner!= null)
        {
            Owner.SubscribePlatformGravitation(SetGravity, true);
            Owner.SubscribeSendPosition(SetPlatformDestination, true);
        }
    }

    private void OnDisable()
    {
        if (Owner != null)
        {
            Owner.SubscribePlatformGravitation(SetGravity, false);
            Owner.SubscribeSendPosition(SetPlatformDestination, false);
        }
    }

    public void SetOwner(WashingMachineMgr wMgr)
    {
        if (wMgr != null)
            Owner = wMgr;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "WashingMachine")
        {
            SetGravity(false);
        }
        
    }
    


    // Update is called once per frame
    void Update()
    {
        if (isGravityAffected)
        {
            Vector3 direction = (Owner.transform.position - transform.position).normalized;

            transform.position += -direction * Owner.GValue * Time.deltaTime;

        }

        if (goToDestination)
        {
            Vector3 Dir = Center.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Dir);
            timer += Time.deltaTime;
            float fraction = timer / Owner.TimeOfPlatformPositioning;
            transform.position = Vector3.Slerp(PreviousPosition, PlatformNewDestination, fraction);
            if (timer>= Owner.TimeOfPlatformPositioning)
            {
                timer = 0;
                transform.position = PlatformNewDestination;
                myColliders[0].enabled = true;
                myColliders[1].enabled = true;
                StartSetPosition(false);
                Owner.RestartWashingMachine();
            }
        }
    }
    //public void SetPlatformDestination(Vector3 destination)
    //{
    //    myCollider.enabled = false;
    //    PlatformNewDestination = destination;
    //    PreviousPosition = transform.position;
    //    goToDestination = true;

    //}

    public void SetPlatformDestination(bool b)
    {
        myColliders[0].enabled = false;
        myColliders[1].enabled = false;
        PreviousPosition = transform.position;
        PlatformNewDestination = startPosition;
        StartSetPosition(b);

    }
    public void StartSetPosition(bool start)
    {
        goToDestination = start;
    }
    public void StoringPos(Vector3 pos)
    {
        StoredPosition.Add(pos);
        if (StoredPosition.Count == 3)
        {
            //SetPlatformDestination(StoredPosition[Random.Range(0, StoredPosition.Count)]);
        }
    }
 
  
    public void SetGravity(bool status)
    {
        isGravityAffected = status;
    }
}
