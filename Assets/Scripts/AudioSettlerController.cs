using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettlerController : MonoBehaviour
{
    public float speed;
    public float onAngle, offAngle;
    private float angleTgt;
    public Transform button;
    public Vector3 initAngle;
    public Menu_Mgr Menu;

    // Start is called before the first frame update
    void Start()
    {
        initAngle = new Vector3(button.rotation.x, button.rotation.y, button.rotation.z);
        angleTgt = onAngle;
    }
    public void SetAudioOn()
    {
        angleTgt = onAngle;    //Aggiungere Rimozione e Attivazione suoni
        Menu.AudioOn();
        
    }
    public void SetAudioOff()
    {
        angleTgt = offAngle;
        Menu.AudioOff();
    }

    // Update is called once per frame
    void Update()
    {
        if (angleTgt != button.rotation.eulerAngles.z)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(initAngle.x,initAngle.y,angleTgt), speed * Time.deltaTime);
        }
    }
}
