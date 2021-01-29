using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachineMgr : MonoBehaviour
{
    public List<GravityAffectedScript> Platform = new List<GravityAffectedScript>();
    public GravityAffectedScript Player;
    public float GValue;
    public bool ApplyGAll, RemoveGForce;
    public string PlayerSceneObjName;

    private float centrifugaNotMovingTimer, storedTimerValue;

    //TRAMITE EVENT IMPOSTA LE VELOCITà DELLE ORBITE

   
    void Start()
    {
        Player.GravityOn = true;
    }

   
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



        if (centrifugaNotMovingTimer <= 0) //scade IL TEMPO DELLA CENTRIFUGA FERMA.
        {
            //ApplyGAll = false;
            ResetTimer();
            //in tutti gli altri casi spegne la gravità alle pedane tranne che al player.
            DeactivateGForcePlatforms();
            //RIPOSIZIONARE LE PIATTAFORME ALLE POSIZIONI STABILITE.
            
        }
    }
    void PlatformComeBackAtPosition()
    {
        for (int i = 0; i < Platform.Count; i++)
        {

            // opzione 1 -> Platform[i]."ComeBack (Bool)" = true; -> nell'update delle piattaforme ci sara' un check e torneranno al loro posto
            // opzione 1.5 -> Platform[i].MoveTo(position) (metodo con argomento destinazione dove la piattaforma deve andare
            //NB: quando le piattaforme si riposizionano devono avere il collider disattivato perche' potrebbero collidere fra di loro.

            // opzione 2 -> Invoke(position) - unity event 

            //-- nell'update della piattaforma ci sara' il lerp che lo portera' a destinazione
        }
    }
    void ActivateGForce()
    {
        for (int i = 0; i < Platform.Count; i++)
        {
            if (!Platform[i].GravityOn)
            {
                Platform[i].GravityOn = true;
            }
        }
    }
    void ResetTimer()
    {
        if (centrifugaNotMovingTimer > 0)
            centrifugaNotMovingTimer = storedTimerValue;
    }
    void Tick()
    {
        centrifugaNotMovingTimer -= Time.deltaTime;
    }

    void SetCentrifugaNotMovingTimer(float timer)//VERRà SETTATO TRAMITE INVOCAZIONE EVENTO
    {
        centrifugaNotMovingTimer = timer;
        storedTimerValue = timer;
    }
    void DeactivateGForcePlatforms()
    {
        for (int i = 0; i < Platform.Count; i++)
        {
            if (Platform[i].GravityOn)
            {
                //si deve disattivare il collider mentre cadono!
                //devono passare dietro il player cosi' da evitare sovrapposizioni e che il player si incastri.(layer/ foreground/spostare la Z)
                Platform[i].GravityOn = false;
            }
        }
    }
}
