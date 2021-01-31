using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningCondition : MonoBehaviour
{
    public WashingMachineMgr machineMgr;
   
    private void OnCollisionEnter(Collision collision)
    {
        InputWithRB scr;
        if (collision.transform.TryGetComponent<InputWithRB>(out scr))
        {
            machineMgr.AutorotateOrbits(false);
            machineMgr.YouWin();
        }
    }

}
