using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScript : MonoBehaviour
{
    public bool testStart;
    public List<PlatformScript> platform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(testStart)
        {
            testStart = false;
            for (int i = 0; i < platform.Count; i++)
            {
                //platform[i].SetPlatformDestination(new Vector3(Random.Range(0, 10), Random.Range(0, 10), 0));
            }
            
        }
    }
}
