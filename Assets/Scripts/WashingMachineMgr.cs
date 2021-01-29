using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class ActivatePlatformsEvent : UnityEvent<bool> { }
public class SetPlatformPosition : UnityEvent<Vector3> { }
public class WashingMachineMgr : MonoBehaviour
{

    public List<PlatformScript> Platform = new List<PlatformScript>();
    public List<Transform> Orbits;

    public float GValue;
    public bool ApplyGAll, RemoveGForce;
    public string PlayerSceneObjName;
    public ActivatePlatformsEvent GPlatformsEvent;
    public SetPlatformPosition SendPositionEvent;
    private float centrifugaNotMovingTimer, storedTimerValue;
    private float rotationSpeed;
    private bool startGame;
    bool stopMotionTrigger;

    List<Vector3> positionList = new List<Vector3>();
    List<Vector3> UsedPositions = new List<Vector3>();
    //TRAMITE EVENT IMPOSTA LE VELOCITà DELLE ORBITE


    public void InitializeGame(int diffLevel) //livello di difficoltà --Implementare Switch--
    {
        //alla fine di tutto
        startGame = true;
    }

    void Start()
    {
        //RIEMPIRE LA LISTA DI USABLE positionList
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
        if (startGame)
        {
            if (stopMotionTrigger)//QUANDO IL PLAYER RAGGIUNGE IL CENTRO DELLA LAVATRICE E TRIGGERA LO STOP
            {
                //FAI CADERE LE PIATTAFORME.
                GPlatformsEvent.Invoke(true);
                Tick();

                if (TimeIsOver()) //scade IL TEMPO DELLA CENTRIFUGA FERMA.
                {
                    //ApplyGAll = false;
                    ResetTimer();



                    //RIPOSIZIONARE LE PIATTAFORME ALLE POSIZIONI STABILITE.
                    SendPositionEvent.Invoke(positions);
                    //SPENGO LA GRAVITà ALLE PIATTAFORME
                    GPlatformsEvent.Invoke(false);

                }
            }
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

    void SendPositions()
    {
        for (int i = 0; i < 3; i++)
        {
            int rndPosIndex = Random.Range(0, positionList.Count - 1);

        }
    }

    void ResetTimer()
    {
        if (centrifugaNotMovingTimer >= 0)
            centrifugaNotMovingTimer = storedTimerValue;
    }
    void Tick()
    {
        centrifugaNotMovingTimer -= Time.deltaTime;
    }
    bool TimeIsOver()
    {
        return centrifugaNotMovingTimer <= 0;
    }

    void SetCentrifugaNotMovingTimer(float timer)//VERRà SETTATO TRAMITE INVOCAZIONE EVENTO
    {
        centrifugaNotMovingTimer = timer;
        storedTimerValue = timer;
    }

}
