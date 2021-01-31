using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningCondition : MonoBehaviour
{
    public WashingMachineMgr machineMgr;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        InputWithRB scr;
        if (collision.transform.TryGetComponent<InputWithRB>(out scr))
        {
            //stop machine

            //...
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
