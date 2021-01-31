using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Mgr : MonoBehaviour
{
    //Animazione Bottoni

    private bool Audio_On = true;
    private int IDLE_Hashe;

    //Animazione Camera
    public bool startCameraAnim;
    public Transform CameraRef;
    public float speed = 10f;
    public float TranslateTimer, RotationTimer;
    public Canvas Menu;
    public Image startMenu;

    public List<Transform> TgTCameraPositions;

    public int level = 1;

    public AudioSource MusicTheme,MouseOver,Selection;

    Vector3 tgTPosition,oldPosition;
    Quaternion tgtRotation, oldRotation;
    
    float lerpTimer;
    int counterPos = 1;
    bool doingAnimation;
    private bool startToFade;
    float alpha;
    public float alphaMultiplier=2;


    // Start is called before the first frame update
    void Start()
    {

        alpha = startMenu.color.a;
        oldPosition = CameraRef.position;
        oldRotation = CameraRef.rotation;

    }
    public void AudioOn()
    {
        //wait for end animation
       
        if (!Audio_On)
        {
            Audio_On = true;
            
            if (!MusicTheme.isPlaying)
                MusicTheme.Play();
        }

    }
    public void AudioOff()
    {
        //wait for end animation

        if (Audio_On)
        {
            Audio_On = false;
            MusicTheme.Stop();


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
        if(startToFade)
        {
            alpha -= alphaMultiplier*Time.deltaTime;
            startMenu.color = new Color(startMenu.color.r, startMenu.color.g, startMenu.color.b, alpha);
            if(startMenu.color.a<=0)
            {

                
                startCameraAnim = true;
                Menu.enabled = false;
                startToFade=false;
            }
        }
        if (startCameraAnim)
        {
            
            if (!doingAnimation && counterPos < TgTCameraPositions.Count - 1)
            {
                doingAnimation = true;
                tgTPosition = PickPositions(counterPos);
                tgtRotation = PickRotation(counterPos);
            }
            lerpTimer += Time.deltaTime;
            CameraRef.position = Vector3.Slerp(oldPosition, tgTPosition, lerpTimer / TranslateTimer);
            CameraRef.rotation = Quaternion.Slerp(oldRotation, tgtRotation, lerpTimer / RotationTimer);
            NextAnimation();
            if(counterPos <TgTCameraPositions.Count - 1)//SI TROVA DAVANTI AL MENù
            {

            }
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

    public void StartAnimation()
    {
        PlayClick();
        startToFade = true;
    }

    public void PlayOnMouseOver()
    {
        if(!MouseOver.isPlaying)
        MouseOver.Play();
    }

    public void PlayClick()
    {
        if (!Selection.isPlaying)
            Selection.Play();
    }
}
