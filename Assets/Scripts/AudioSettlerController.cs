using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettlerController : MonoBehaviour
{
    public float speed;
    public float onAngle, offAngle;
    private float angleTgt;


    // Start is called before the first frame update
    void Start()
    {
        angleTgt = onAngle;
    }
    public void SetAudioOn()
    {
        angleTgt = onAngle;    //Aggiungere Rimozione e Attivazione suoni
        
    }
    public void SetAudioOff()
    {
        angleTgt = offAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (angleTgt != transform.rotation.eulerAngles.x)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angleTgt, -90, -90), speed * Time.deltaTime);
        }
    }
}
