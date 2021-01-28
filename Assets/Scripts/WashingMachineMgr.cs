using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachineMgr : MonoBehaviour
{
    public List<GravityAffectedScript> GAffectedObj = new List<GravityAffectedScript>();
    public GravityAffectedScript Player;
    public float GValue;
    public bool ApplyGAll, RemoveGForce;
    public string PlayerSceneObjName;

    private float centrifugaTimer, storedTimerValue;

    //TRAMITE EVENT IMPOSTA LE VELOCITà DELLE ORBITE

    // Start is called before the first frame update
    void Start()
    {
        Player.GravityOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        //il tick parte solo se triggerato il ccomando. //GESTIRE BENE I BOOL

        /*
         if(STOPTRIGGER)//le pedane cadono
        {
        IF(ONETIMEONLY)//PER EVITARE INUTILI CICLI FOR
        {
        ActivateGForce();
        }

        Tick();
        }
        */



        if (centrifugaTimer <= 0) //scade IL TEMPO DELLA CENTRIFUGA FERMA.
        {
            //ApplyGAll = false;
            ResetTimer();
            //in tutti gli altri casi spegne la gravità alle pedane tranne che al player.
            DeactivateGForcePlatforms();
            //RIPOSIZIONARE LE PIATTAFORME ALLE POSIZIONI STABILITE.
            
        }
    }

    void ActivateGForce()
    {
        for (int i = 0; i < GAffectedObj.Count; i++)
        {
            if (!GAffectedObj[i].GravityOn)
            {
                GAffectedObj[i].GravityOn = true;
            }
        }
    }
    void ResetTimer()
    {
        if (centrifugaTimer > 0)
            centrifugaTimer = storedTimerValue;
    }
    void Tick()
    {
        centrifugaTimer -= Time.deltaTime;
    }

    void SetCentrifugaTimer(float timer)//VERRà SETTATO TRAMITE INVOCAZIONE EVENTO
    {
        centrifugaTimer = timer;
        storedTimerValue = timer;
    }
    void DeactivateGForcePlatforms()
    {
        for (int i = 0; i < GAffectedObj.Count; i++)
        {
            if (GAffectedObj[i].GravityOn && GAffectedObj[i].name != PlayerSceneObjName)
            {
                GAffectedObj[i].GravityOn = false;
            }
        }
    }
}
