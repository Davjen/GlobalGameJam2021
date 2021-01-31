using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScriptSimo : MonoBehaviour
{

    public Transform player;
    public Vector3 Offset;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
       cam= GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = player.position + Offset;

        Vector3 dir = (transform.position - cam.transform.position).normalized;
        cam.transform.rotation = Quaternion.LookRotation(dir);
        
    }
}
