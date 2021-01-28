using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Mgr : MonoBehaviour
{
    public Animator anim;
    private bool Audio_On = true;
    private int IDLE_Hashe;
    // Start is called before the first frame update
    void Start()
    {
        IDLE_Hashe = Animator.StringToHash("IDLE");
    }
    public void AudioOn()
    {
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!Audio_On && animInfo.IsName("IDLE"))
        {
            Audio_On = true;
            anim.SetTrigger("on");
        }

    }
    public void AudioOff()
    {
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (Audio_On && animInfo.IsName("IDLE"))
        {
            Audio_On = false;
            anim.SetTrigger("off");

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
