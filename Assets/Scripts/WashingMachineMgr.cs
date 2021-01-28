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
        Tick();

        if (ApplyGAll)//QUANDO IL PLAYER TRIGGERA IL CENTRO PER FAR FERMARE LA CENTRIFUGA
        {
            //LE PEDANE CADONO
            ActivateGForce();
            ApplyGAll = false;
        }

        if (centrifugaTimer <= 0) //Ci sarà il Timer
        {
            ResetTimer();
            //in tutti gli altri casi spegne la gravità alle pedane tranne che al player.
            DeactivateGForce();
            RemoveGForce = false;
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

    void SetCentrifugaTimer(float timerInMin)//VERRà SETTATO TRAMITE INVOCAZIONE EVENTO
    {
        centrifugaTimer = timerInMin * 60;
        storedTimerValue = timerInMin * 60;
    }
    void DeactivateGForce()
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
