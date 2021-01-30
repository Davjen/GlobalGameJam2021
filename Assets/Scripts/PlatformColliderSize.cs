using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColliderSize : MonoBehaviour
{
   // public GameObject Prefab;
    public float ColliderSize { get; protected set; }
    
    void Awake()
    {
       ColliderSize= GetComponent<Collider>().bounds.extents.z;
    }

   
}
