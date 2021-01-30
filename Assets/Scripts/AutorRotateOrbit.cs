using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutorRotateOrbit : MonoBehaviour
{
    public float Velocity;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Velocity*Time.deltaTime, 0);
    }
}
