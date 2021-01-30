using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlatformScript : MonoBehaviour
{
    [Range (0f,10f)]
    public float platformGravityValue;

    public Transform origin;
    public float TimeOfPositioning;
    public float WashingMachieRadius;
   
    WashingMachineMgr owner;
    Vector3 PlatformNewDestination;
    Vector3 PreviousPosition;
    bool goToDestination;
    Collider myCollider;
    float timer;
    public bool isGravityAffected;
    List<Vector3> StoredPosition = new List<Vector3>();

    
    // Start is called before the first frame update
    void Start()
    {
        myCollider = transform.GetComponent<Collider>();
    }

    private void Awake()
    {
        owner.GPlatformsEvent.AddListener(SetGravity);
        owner.SendPositionEvent.AddListener(StoringPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGravityAffected)
        {
            Vector3 direction = owner.transform.position - transform.position;

            transform.position -= direction.normalized * owner.GValue * Time.deltaTime;

            if ((owner.transform.position - transform.position).magnitude >= WashingMachieRadius)
            {
                isGravityAffected = false;
            }
        }

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
    public void StartSetPosition(bool start)
    {
        goToDestination = start;
    }
    public void StoringPos(Vector3 pos)
    {
        StoredPosition.Add(pos);
        if (StoredPosition.Count == 3)
        {
            SetPlatformDestination(StoredPosition[Random.Range(0, StoredPosition.Count)]);
        }
    }

    public void Attractor(Rigidbody rbToAttract)
    {
        Vector3 gravityDirection = transform.position - rbToAttract.position;
        //float distance = gravityDirection.magnitude; VOGLIAMO RENDERLA PROPORZIONALE ALLA DISTANZA E ALLE MASSE DEGLI OGGETTI IN GIOCO COME NELLA REALTà? m*m/r^2
        Vector3 force = gravityDirection.normalized;
        rbToAttract.AddForce(force);       

    }
    public void SetGravity(bool status)
    {
        isGravityAffected = status;
    }
}
