using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Mgr : MonoBehaviour
{
    //Animazione Bottoni
    public Animator anim;
    private bool Audio_On = true;
    private int IDLE_Hashe;

    //Animazione Camera
    private bool startCameraAnim = false;
    private Transform camera;
    public Transform tgtCamera;
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        IDLE_Hashe = Animator.StringToHash("IDLE");
    }
    public void AudioOn()
    {
        //wait for end animation
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!Audio_On && animInfo.IsName("IDLE"))
        {
            Audio_On = true;
            anim.SetTrigger("on");
        }

    }
    public void AudioOff()
    {
        //wait for end animation
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (Audio_On && animInfo.IsName("IDLE"))
        {
            Audio_On = false;
            anim.SetTrigger("off");
            
        }
    }
    public void StartGame()
    {
        startCameraAnim = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startCameraAnim)
        {
            camera.position = Vector3.Lerp(camera.position, tgtCamera.position, speed * Time.deltaTime);
            camera.rotation = Quaternion.Lerp(camera.rotation, tgtCamera.rotation, speed * Time.deltaTime);
        }
    }
}
