using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PROVASWITCHSPRITE : MonoBehaviour
{
    public Image image;
    public Sprite winSpr, loseSpr;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Win()
    {
        image.color = new Color(1, 1, 1, 1);
        image.sprite = winSpr;
    }
    public void Lose()
    {
        image.color = new Color(1, 1, 1, 1);
        image.sprite = loseSpr;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Lose();
        }
    }
}
