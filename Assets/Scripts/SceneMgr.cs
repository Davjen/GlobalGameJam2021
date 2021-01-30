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
    AudioSource audioSource;
    public float test;
    public float currentTime;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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

            audioSource.time = test;

            PlayMusic();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            fade = true;
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
