using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    
    public AudioSource MainTheme, WinJingle, LoseJingle, JumpSound;
    Dictionary<string, AudioSource> soundsEffects = new Dictionary<string, AudioSource>();

    // Start is called before the first frame update

    private void Awake()
    {
        //EventManagerStatic.PlaySound.AddListener(PlaySounds);
        //EventManagerStatic.PlaySound.AddListener(StopSound);
        soundsEffects["Jump"] = JumpSound;
        soundsEffects["Lose"] = LoseJingle;
        soundsEffects["Win"] = WinJingle;
        soundsEffects["MainTheme"] = MainTheme;

    }
    void Start()
    {
        


    }

    public void PlaySound(string sound)
    {
        if(!soundsEffects[sound].isPlaying)
        soundsEffects[sound].Play();
    }
   

    public void StartFrom(string sounds,float timer)
    {

        soundsEffects[sounds].time = timer;
        soundsEffects[sounds].Play();
    }

    public void StopSound(string sound)
    {
        soundsEffects[sound].Stop();
    }
    public void StopSoundAndSave(string sounds,out float soundTime)
    {
        soundTime = 0;
        soundsEffects[sounds].time = soundTime;
        soundsEffects[sounds].Stop();
    }

}
