using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Mgr : MonoBehaviour
{
    //Animazione Bottoni
    //public Animator anim;
    private bool Audio_On = true;
    private int IDLE_Hashe;

    //Animazione Camera
    public bool startCameraAnim;
    public Transform CameraRef;
    public Transform tgtCamera;
    public float speed = 10f;
    public float TranslateTimer, RotationTimer;

    public List<Transform> TgTCameraPositions;

    public int level = 1;

    public AudioSource MusicTheme;

    Vector3 tgTPosition,oldPosition;
    Quaternion tgtRotation, oldRotation;
    
    float lerpTimer;
    int counterPos = 1;
    bool doingAnimation;


    // Start is called before the first frame update
    void Start()
    {

        oldPosition = CameraRef.position;
        oldRotation = CameraRef.rotation;
        //IDLE_Hashe = Animator.StringToHash("IDLE");
    }
    public void AudioOn()
    {
        //wait for end animation
        //AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!Audio_On /*&& animInfo.IsName("IDLE")*/)
        {
            Audio_On = true;
            //anim.SetTrigger("on");
            if (!MusicTheme.isPlaying)
                MusicTheme.Play();
        }

    }
    public void AudioOff()
    {
        //wait for end animation
        //AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (Audio_On /*&& animInfo.IsName("IDLE")*/)
        {
            Audio_On = false;
            MusicTheme.Stop();
            //anim.SetTrigger("off");

        }
    }
    public void StartGame()
    {
        StaticSavingScript.LEVEL_DIFFICULTY = level;
        StaticSavingScript.MUSIC_TIMER_START = MusicTheme.time;

        startCameraAnim = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startCameraAnim)
        {
            if (!doingAnimation && counterPos < TgTCameraPositions.Count - 1)
            {
                doingAnimation = true;
                tgTPosition = PickPositions(counterPos);
                tgtRotation = PickRotation(counterPos);
            }
            lerpTimer += Time.deltaTime;
            CameraRef.position = Vector3.Lerp(oldPosition, tgTPosition, lerpTimer / TranslateTimer);
            CameraRef.rotation = Quaternion.Lerp(oldRotation, tgtRotation, lerpTimer / RotationTimer);
            NextAnimation();
            //if(counterPos <TgTCameraPositions.Count - 1)
        }
    }

    private void NextAnimation()
    {
        if ((lerpTimer / TranslateTimer >= 1) && (lerpTimer / RotationTimer >= 1))
        {
            oldPosition = tgTPosition;
            oldRotation = tgtRotation;
            lerpTimer = 0;
            counterPos++;
            doingAnimation = false;
        }
    }

    Vector3 PickPositions(int index)
    {

        return TgTCameraPositions[index].position;
    }
    Quaternion PickRotation(int index)
    {
        return TgTCameraPositions[index].rotation;
    }
}
