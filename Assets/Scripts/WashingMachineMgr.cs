using System.Collections;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

public enum Level { Easy,Medium,Hard}
[SerializeField]
public class ActivatePlatformsEvent : UnityEvent<bool> { }
public class SetPlatformPosition : UnityEvent<bool> { }
public class WashingMachineMgr : MonoBehaviour
{

    public List<PlatformScript> Platform = new List<PlatformScript>();
    public List<AutorRotateOrbit> Orbits;

    public float GValue;
    public bool ApplyGAll, RemoveGForce;
    public bool PLAYSOUND;
    public ActivatePlatformsEvent GPlatformsEvent;
    public SetPlatformPosition SendPositionEvent;
    private float centrifugaNotMovingTimer;
    private float rotationSpeed;
    public bool startGame;
    bool startOrbiting;
    bool platformFall;
    public bool StopMotionTrigger;
    public bool TimerIsOver;
    public float TimeOfPlatformPositioning = 1;


    List<Vector3> positionList = new List<Vector3>();
    private float centrifugaNotMovingTimerValue;
    private float gameTimeLenghtValue;



    //TRAMITE EVENT IMPOSTA LE VELOCITà DELLE ORBITE


    public void InitializeGame(int diffLevel) //livello di difficoltà --Implementare Switch--
    {
        //alla fine di tutto
        switch ((Level)diffLevel)
        {
            case Level.Easy:
                centrifugaNotMovingTimerValue = 30f;
                gameTimeLenghtValue = 180f;
                centrifugaNotMovingTimer = centrifugaNotMovingTimerValue;
                break;
            case Level.Medium:
                centrifugaNotMovingTimerValue = 20f;
                gameTimeLenghtValue = 160f;
                centrifugaNotMovingTimer = centrifugaNotMovingTimerValue;
                break;
            case Level.Hard:
                centrifugaNotMovingTimerValue = 15f;
                gameTimeLenghtValue = 120f;
                centrifugaNotMovingTimer = centrifugaNotMovingTimerValue;
                break;
        }
        startGame = true;
    }

    

    public void SubscribePlatformGravitation(UnityAction<bool> onActivatePlatform, bool b)
    {
        if (b)
            UnityEventTools.AddPersistentListener(GPlatformsEvent, onActivatePlatform);
        else
            UnityEventTools.RemovePersistentListener(GPlatformsEvent, onActivatePlatform);

    }

    public void SubscribeSendPosition(UnityAction<bool> onSetPlatformPos, bool b)
    {
        if (b)
            UnityEventTools.AddPersistentListener(SendPositionEvent, onSetPlatformPos);
        else
            UnityEventTools.RemovePersistentListener(SendPositionEvent, onSetPlatformPos);

    }

    public void RestartWashingMachine()
    {
        startOrbiting = false;
        platformFall = false;
        StopMotionTrigger = false;

    }

    void Awake()
    {
        GPlatformsEvent = new ActivatePlatformsEvent();
        SendPositionEvent = new SetPlatformPosition();
        AutorotateOrbits(false);

        for (int i = 0; i < Platform.Count; i++)
        {
            Platform[i].SetOwner(this);
            Platform[i].Init();
        }
        //RIEMPIRE LA LISTA DI USABLE positionList
    }


    void Update()
    {
             
        if (startGame)
        {
            if (!startOrbiting)
            {
                AutorotateOrbits(true);
                startOrbiting = true;
            }

            TickEndGame();
            if(EndGame())
            {
                //FINE DEL GIOCO.
            }
            //METTE IN MOTO LE PIATTAFORME
            if (StopMotionTrigger)//QUANDO IL PLAYER RAGGIUNGE IL CENTRO DELLA LAVATRICE E TRIGGERA LO STOP
            {
                //FAI CADERE LE PIATTAFORME.

                if (!platformFall)
                {
                    GPlatformsEvent.Invoke(true);
                    platformFall = true;
                    AutorotateOrbits(false);
                    //Internal circle disappearing animation to add
                }
                Tick();

                if (/*TimeIsOver()*/ TimerIsOver) //scade IL TEMPO DELLA CENTRIFUGA FERMA.
                {
                    ResetTimer();
                    TimerIsOver = false;
                    //RIPOSIZIONARE LE PIATTAFORME ALLE POSIZIONI STABILITE.
                    //GPlatformsEvent.Invoke(false);
                    SendPositionEvent.Invoke(true);

                }
            }
        }
    }

    void AutorotateOrbits(bool b)
    {
        for (int i = 0; i < Orbits.Count; i++)
        {
            Orbits[i].enabled = b;
        }
    }
    void SendPositions()
    {
        int count = positionList.Count;
        for (int j = 0; j < Platform.Count; j++)
        {

            for (int i = 0; i < 3; i++)
            {
                int rndPosIndex = Random.Range(0, count);

                //SendPositionEvent.Invoke(positionList[rndPosIndex]);
                Vector3 pos = positionList[count];
                positionList[count] = positionList[rndPosIndex];
                positionList[rndPosIndex] = pos;
                count--;
            }
        }
    }

    void ResetTimer()
    {
        if (centrifugaNotMovingTimer >= 0)
            centrifugaNotMovingTimer = centrifugaNotMovingTimerValue;
    }
    void Tick()
    {
        centrifugaNotMovingTimer -= Time.deltaTime;
    }
    void TickEndGame()
    {
        gameTimeLenghtValue -= Time.deltaTime;
    }
    bool TimeIsOver()
    {
        return centrifugaNotMovingTimer <= 0;
    }

    bool EndGame()
    {
        return gameTimeLenghtValue <= 0;
    }

}
