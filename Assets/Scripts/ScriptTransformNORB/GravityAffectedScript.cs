using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GravityAffectedScript : MonoBehaviour
{
    public static UnityAction testAction;
    public WashingMachineMgr Mgr;
    public bool GravityOn;
    public Transform OriginGForce;
    //public AudioSource audioSource;

    float gSpeed;
    // Start is called before the first frame update
    void Start()
    {
        gSpeed = Mgr.GValue;
        
    }
    private void Awake()
    {
        testAction += DeactivateP;
      
        //audioSource.time = StaticSavingScript.MUSIC_TIMER_START;
        //audioSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
 

        if (GravityOn)
        {
            Vector3 direction = OriginGForce.position - transform.position;
            
            transform.position -= direction.normalized * gSpeed * Time.deltaTime;
        }
    }
    private void DeactivateP()
    {
        Debug.Log("ascolto");
        enabled = false;
    }
    public Vector3 GetDir()
    {
        Vector3 dir;
        return dir = OriginGForce.position - transform.position;
    }
}
