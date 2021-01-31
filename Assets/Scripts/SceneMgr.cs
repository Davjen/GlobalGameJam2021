using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneMgr : MonoBehaviour
{
    public Image image;
    bool fade;
    float alpha = 0;
    public AudioSource audioSource,jump;
    public float test;
    public float currentTime;
    public ParticleSystem ps;
    float particlestartPos;
    float particleleftPos;
    private bool positioning;
    private bool positioningleft;







    // Start is called before the first frame update
    void Start()
    {
        

        //particlestartPos = ps.shape.position.x;
        //particleleftPos = 4.04f;
    }
    private void Awake()
    {

    }
    void PlayMusic()
    {
        audioSource.Play();
    }

    void StopMusic()
    {
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = audioSource.time;
        if (Input.GetKeyDown(KeyCode.B))
        {
            test = audioSource.time;
            StopMusic();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {

            //audioSource.time = test;

            Debug.Log("test");
            PlayMusic();

        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if(!jump.isPlaying)
            jump.Play();

            //if (positioning)
            //{
            //    positioning = false;
            //    particlestartPos = ps.shape.position.x;
            //}


        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //if(positioningleft)
            //{
            //    positioningleft = false;
            //    ps.shape.position = new Vector2(particlestartPos, ps.shape.position.y);
            //    ps.shape.
            //}
        }
        if (fade)
        {

            alpha += 0.9f * Time.deltaTime;
            image.color = new Color(0, 0, 0, alpha);
            if (image.color.a >= 1)
            {
                StaticSavingScript.MUSIC_TIMER_START = audioSource.time;
                audioSource.Stop();
                SceneManager.LoadScene("MattiaScene");
            }
        }
    }
}
