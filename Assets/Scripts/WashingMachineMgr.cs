using System.Collections;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

public enum Level { Easy,Medium,Hard,None}
[SerializeField]
public class ActivatePlatformsEvent : UnityEvent<bool> { }
public class SetPlatformPosition : UnityEvent<bool> { }
public class WashingMachineMgr : MonoBehaviour
{
    //Public Values
    public Transform Player;
    public List<PlatformScript> Platform = new List<PlatformScript>();
    public List<AutorRotateOrbit> Orbits;
    public Transform InternalCircleLowHandler;
    public Level DifficultyLevel= Level.None;
    public float GValue;
    public bool PLAYSOUND;

    //Events
    public ActivatePlatformsEvent GPlatformsEvent;
    public SetPlatformPosition SendPositionEvent;

    //Durations
    public float PlatformPositioningDuration = 1;
    private float gameTimeDuration;
    private float washingMachineStopDuration;
    public float LoweringInternalCircleDuration=1;
    public float PlayerRepositioningDuration = 1;

    //Timers
    private float gameTimeTimer;
    private float washingMachineStopTimer;
    private float loweringInternalCircleTimer;
    private float playerRepositioningTimer;


    //Bools
    public bool startGame;
    bool startOrbiting;
    bool platformFall;
    public static bool StopMotionTrigger;
    private bool lowCircle;
    bool raiseCircle;
    private bool playerRepositioning;

    //Handlers positions
    Vector3 internalCircleStartPosition;
    Vector3 playerHandlerLastPos;
     Vector3 playerLastPos;

    List<Vector3> positionList = new List<Vector3>();





    //TRAMITE EVENT IMPOSTA LE VELOCITà DELLE ORBITE


    public void InitializeGame() //livello di difficoltà --Implementare Switch--
    {
        //alla fine di tutto
        switch (DifficultyLevel)
        {
            case Level.Easy:
                washingMachineStopDuration = 30f;
                gameTimeDuration = 180f;
                startGame = true;
                break;
            case Level.Medium:
                washingMachineStopDuration = 20f;
                gameTimeDuration = 160f;
                startGame = true;
                break;
            case Level.Hard:
                washingMachineStopDuration = 15f;
                gameTimeDuration = 120f;
                startGame = true;   
                break;
        }
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

    public static void StopMotion()
    {
        StopMotionTrigger = true;
    }

    public void RestartWashingMachine()
    {
        startOrbiting = false;
        platformFall = false;

    }

    internal void RegisterPlayerHandlerPos()
    {
        playerHandlerLastPos = Player.GetChild(1).position;
        playerLastPos = Player.transform.position;
        playerRepositioning = true;
        Player.GetComponent<InputWithRB>().RecordInput(false);
        //Player.GetComponent<InputWithRB>().gravOn=false;

    }

    void Awake()
    {
        GPlatformsEvent = new ActivatePlatformsEvent();
        SendPositionEvent = new SetPlatformPosition();

        AutorotateOrbits(false);

        internalCircleStartPosition = Orbits[0].transform.position;

        for (int i = 0; i < Platform.Count; i++)
        {
            Platform[i].SetOwner(this);
            Platform[i].Init();
        }
        //RIEMPIRE LA LISTA DI USABLE positionList
    }


    void Update()
    {
        InitializeGame();
             
        if (startGame)
        {
            if (!startOrbiting)
            {
                AutorotateOrbits(true);
                startOrbiting = true;
            }

            TickEndGame();

            //Script to End Game
            if(EndGame())
            {
                Debug.Log("Scene Ended");
            }

            //Raise Internal Circle when searching time is over
            if (raiseCircle)
                MovingInternalCircle(false);

            //Reposition Player when searching time is over
            if (playerRepositioning)
                ReposiotioningPlayer();


            //METTE IN MOTO LE PIATTAFORME
            if (StopMotionTrigger)//QUANDO IL PLAYER RAGGIUNGE IL CENTRO DELLA LAVATRICE E TRIGGERA LO STOP
            {
                //Let platforms fall
                if (!platformFall)
                {
                    GPlatformsEvent.Invoke(true);
                    platformFall = true;
                    AutorotateOrbits(false);
                    LowInternalCircle(true);
                }

                Tick();

                //Low Internal circle when Central Button is pressed
                if (lowCircle)
                    MovingInternalCircle(true);

                //Restarts washing machine
                if (TimeIsOver()) 
                {
                    ResetTimer();
                    //Invokes platforms repositioning
                    SendPositionEvent.Invoke(true);
                    raiseCircle = true;
                    RegisterPlayerHandlerPos();
                    StopMotionTrigger = false;

                }
            }
        }
    }

    private void ReposiotioningPlayer()
    {
        //Lerps to PlayerHandler
        playerRepositioningTimer += Time.deltaTime;
        float fraction = playerRepositioningTimer / PlayerRepositioningDuration;
        Player.transform.position = Vector3.Lerp(playerLastPos, playerHandlerLastPos, fraction);
        if (playerRepositioningTimer >= PlayerRepositioningDuration)
        {
            Player.transform.position = playerHandlerLastPos;
            playerRepositioningTimer = 0;
            playerRepositioning = false;

        }
    }

    private void MovingInternalCircle(bool low)
    {
        //Lows or Raises internal circle, depending on the value of low
        Vector3 startPos = low ? internalCircleStartPosition : InternalCircleLowHandler.position;
        Vector3 endPos = low ?  InternalCircleLowHandler.position : internalCircleStartPosition;

        loweringInternalCircleTimer += Time.deltaTime;
            float fraction = loweringInternalCircleTimer / LoweringInternalCircleDuration;
            Orbits[0].transform.position = Vector3.Lerp(startPos, endPos, fraction);
        if (loweringInternalCircleTimer >= LoweringInternalCircleDuration)
        {
            Orbits[0].transform.position = endPos;
            loweringInternalCircleTimer = 0;
            if (low)
                lowCircle = false;
            else
            {
                raiseCircle = false;
                //realeases player and restarts washing machine
                Player.GetComponent<InputWithRB>().RecordInput(true);
                RestartWashingMachine();
                ResetTimer();


            }

        }

    }

    private void LowInternalCircle(bool b)
    {
        lowCircle = b;
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
        if (washingMachineStopTimer >= washingMachineStopDuration)
            washingMachineStopTimer = 0;
    }
    void Tick()
    {
        washingMachineStopTimer+= Time.deltaTime;
    }
    void TickEndGame()
    {
        gameTimeTimer += Time.deltaTime;
    }
    bool TimeIsOver()
    {
        return washingMachineStopTimer >= washingMachineStopDuration;
    }

    bool EndGame()
    {
        return gameTimeTimer <= gameTimeDuration;
    }

}

