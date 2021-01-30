using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStopWashingMachine : MonoBehaviour
{
    public Transform Player;
    public bool StopMachine;
    public WashingMachineMgr WMachineMgr;
    public Transform Cestello;
    Vector3 CestelloStartPos;
    Vector3 CestelloLastPosition;
    float counterTime;
    public float MovingCestelloTime;
    bool MovingCestelloAway;  //DEVE ESSERE CHIAMATO QUANDO COLLIDE 
    bool MovingCestelloBack;   //DEVE ESSERE CHIAMATO QUANDO SCADE IL TEMPO


    // Start is called before the first frame update
    void Start()
    {
        CestelloStartPos = Cestello.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        StopMachine = true;
        //bloccare rotazione - dare l'input per far  cadere le piattaforme (evento.invoke in wmMgr)
        for (int i = 0; i < WMachineMgr.Orbits.Count; i++)
        {
            WMachineMgr.Orbits[i].GetComponent<AutorRotateOrbit>().enabled = false;
        }

    }
    // Update is called once per frame
    void Update()
    {
        //spostare cestello piccolo 
        if (StopMachine)
        {
            if(MovingCestelloAway)
            {
                counterTime += Time.deltaTime;
                Cestello.transform.position = Vector3.Lerp(CestelloStartPos, /*posizione con empty o distanza di y*/ new Vector3(CestelloStartPos.x, CestelloStartPos.y + 6, CestelloStartPos.z), counterTime / MovingCestelloTime);
                if (counterTime >= MovingCestelloTime)
                {
                    Cestello.transform.GetComponent<AutorRotateOrbit>().enabled = false;
                    MovingCestelloAway = false;
                    CestelloLastPosition = Cestello.transform.position;
                    counterTime = 0;
                }
            }
            if (MovingCestelloBack)
            {
                counterTime += Time.deltaTime;
                Cestello.transform.position = Vector3.Lerp(CestelloLastPosition, CestelloStartPos, counterTime / MovingCestelloTime);
                if (counterTime>= MovingCestelloTime)
                {
                    Cestello.transform.GetComponent<AutorRotateOrbit>().enabled = true;
                    MovingCestelloBack = false;
                    counterTime = 0;

                }
                //BISOGNA RIMETTERE IL PLAYER SUL CESTELLO PICCOLO.
            }
            
        }
    }
    //nel Metodo "TimeIsOver" si riattiva la rotazione; 
}
