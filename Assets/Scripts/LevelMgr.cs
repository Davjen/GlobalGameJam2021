using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class MgrEvent : UnityEvent<float> { };

public class LevelMgr : MonoBehaviour
{
    float timer;
    float orbitsSpeed;
    int nHeart;
    int selectedLevel;
    int maxObjSpawned;
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
