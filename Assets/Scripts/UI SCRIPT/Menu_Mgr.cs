using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Transform lastPosition;

    public int level = 1;
    public Image buttonImage;

    public AudioSource MusicTheme, MouseOver, Selection;

    Vector3 tgTPosition, oldPosition;
    Quaternion tgtRotation, oldRotation;

    float lerpTimer;
    int counterPos = 1;
    bool doingAnimation;
    bool startToFade;
    float alpha;
    public float alphaMultiplier = 2;
    public float CountDownToMenu = 1.5f;
    private bool canProceed;
    bool STOP;
    private float beta;
    public Image FadeOutImage;
    private bool fadeToStartGame;




    // Start is called before the first frame update
    void Start()
    {

        alpha = startMenu.color.a;
        oldPosition = CameraRef.position;
        oldRotation = CameraRef.rotation;

    }
    public void QuitGame()
    {

    }
    public void AudioOn()
    {
        //wait for end animation
        PlayClick();
        if (!Audio_On)
        {
            Audio_On = true;

            if (!MusicTheme.isPlaying)
                MusicTheme.Play();
        }

    }
    public void AudioOff()
    {
        PlayClick();
        //wait for end animation

        if (Audio_On)
        {
            Audio_On = false;
            MusicTheme.Stop();


        }
    }
    public void StartGame()
    {
        PlayClick();
        fadeToStartGame = true;
        StaticSavingScript.LEVEL_DIFFICULTY = level;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (startToFade)
        {
            alpha -= alphaMultiplier * Time.deltaTime;
            startMenu.color = new Color(startMenu.color.r, startMenu.color.g, startMenu.color.b, alpha);
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, alpha);
            if (startMenu.color.a <= 0)
            {


                startCameraAnim = true;
                Menu.enabled = false;
                startToFade = false;
            }
        }
        if (startCameraAnim)
        {

            if (!doingAnimation && counterPos < TgTCameraPositions.Count)
            {
                doingAnimation = true;
                tgTPosition = PickPositions(counterPos);
                tgtRotation = PickRotation(counterPos);
            }

         
            if(canProceed)
            {
                tgTPosition = lastPosition.position;
                tgtRotation = lastPosition.rotation;
                STOP = true;
            }

            lerpTimer += Time.deltaTime;
            CameraRef.position = Vector3.Slerp(oldPosition, tgTPosition, lerpTimer / TranslateTimer);
            CameraRef.rotation = Quaternion.Slerp(oldRotation, tgtRotation, lerpTimer / RotationTimer);
            NextAnimation();
          

            if(fadeToStartGame)
                FadeToStartGame();
           
        }
    }


    public void FadeToStartGame()
    {
        
        beta += 1 * Time.deltaTime;
        FadeOutImage.color = new Color(FadeOutImage.color.r, FadeOutImage.color.g, FadeOutImage.color.b, beta);
        if (FadeOutImage.color.a >= 1)
        {
            StaticSavingScript.MUSIC_TIMER_START = MusicTheme.time;
            SceneManager.LoadScene("PlayScene");

        }
    }

    private void NextAnimation()
    {
        if ((lerpTimer / TranslateTimer >= 1) && (lerpTimer / RotationTimer >= 1)&& counterPos < TgTCameraPositions.Count &&!STOP)
        {
            
            oldPosition = tgTPosition;
            oldRotation = tgtRotation;
            lerpTimer = 0;
            counterPos++;
            doingAnimation = false;
        }
        if(((lerpTimer / TranslateTimer >= 1) && (lerpTimer / RotationTimer >= 1) && counterPos == TgTCameraPositions.Count)&&!STOP)
        {
            canProceed = true;
            lerpTimer = 0;
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
        if (!MouseOver.isPlaying)
            MouseOver.Play();
    }

    public void PlayClick()
    {
        if (!Selection.isPlaying)
            Selection.Play();
    }
}
