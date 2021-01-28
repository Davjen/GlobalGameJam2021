using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAffectedScript : MonoBehaviour
{
    public WashingMachineMgr Mgr;
    public bool GravityOn;
    public Transform OriginGForce;

    float gSpeed;
    // Start is called before the first frame update
    void Start()
    {
        gSpeed = Mgr.GValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (GravityOn)
        {
            Vector3 dir = OriginGForce.position - transform.position;
            transform.up = dir;
            transform.position -= dir.normalized * gSpeed * Time.deltaTime;
        }
    }
}
