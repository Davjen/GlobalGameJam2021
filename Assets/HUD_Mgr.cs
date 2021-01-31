using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Mgr : MonoBehaviour
{
    public Sprite winSprite, loseSprite, lifeSprite;
    public TMP_Text timer, timerClose;
    private string timerString = "TIME LEFT ";
    private string timerToClose = "HURRY UP! ";
    private int minutes, seconds;
    public int timeLeft, timeToClose;
    private static List<Image> lifes;
    public HorizontalLayoutGroup lifeGroup;

    // Start is called before the first frame update
    void Start()
    {
        lifes = new List<Image>();
        Image[] images;
        images = lifeGroup.GetComponentsInChildren<Image>();

        foreach (Image item in images)
        {
            lifes.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int minuteLeft = 0;
        int secondsLeft = 0;
        CalculateTime(ref minuteLeft, ref secondsLeft,timeLeft);
        timer.text = timerString + $"{minuteLeft}:{secondsLeft.ToString("D2")}";
        CalculateTime(ref minuteLeft, ref secondsLeft,timeToClose);
        timerClose.text = timerToClose + $"{minuteLeft}:{secondsLeft.ToString("D2")}";


    }
    public void ActivateHurryUp(bool status)
    {
        timerClose.gameObject.SetActive(status);
    }
    void CalculateTime(ref int minutes,ref int seconds,int timeLeft)
    {
        minutes = timeLeft / 60;
        seconds = timeLeft - (minutes * 60);
    }
    public static void UpdateLifes(int lifesLost)
    {
        if (lifes.Count > 0)
        {
            Image image = lifes[lifes.Count - 1];
            image.enabled = false;
            lifes.Remove(image);
        }
    }
}
