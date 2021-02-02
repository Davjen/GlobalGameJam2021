using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Level { Easy,Medium,Hard,None}
[SerializeField]
public class ActivatePlatformsEvent : UnityEvent<bool> { }
public class SetPlatformPosition : UnityEvent<bool> { }
public class WashingMachineMgr : MonoBehaviour
{
    //Public Values
    public HUD_Mgr HUD_Mgr;
    public Transform Player;
    public List<PlatformScript> Platform = new List<PlatformScript>();
    public List<AutorRotateOrbit> Orbits;
    public Transform InternalCircleLowHandler;
    public Level DifficultyLevel= Level.None;
    public float GValue;
    public SoundManager SoundMgr;

    //UI
    public Transform winLoseUI;
    public Sprite win;
    public Sprite lose;
    public Transform CloseTimerHUD;

    //Events
    public ActivatePlatformsEvent GPlatformsEvent;
    public SetPlatformPosition SendPositionEvent;

    //Durations
    public float PlatformPositioningDuration = 1;
    private float gameTimeDuration;
    private float washingMachineStopDuration;
    public float LoweringInternalCircleDuration=1;
    public float PlayerRepositioningDuration = 1;
    private int levelDiff;

    //DurationSetByGameDesiner
    public float WashingMachineStopDuration;
    public float GameTimeDuration;


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

    public static bool GameEnd { get; private set; }


    private void Start()
    {
        levelDiff = StaticSavingScript.LEVEL_DIFFICULTY;
        InitializeGame(levelDiff);
       
        SoundMgr.StartFrom("MainTheme", StaticSavingScript.MUSIC_TIMER_START);

        
    }


    //TRAMITE EVENT IMPOSTA LE VELOCITà DELLE ORBITE


    public void InitializeGame(int value) //livello di difficoltà --Implementare Switch--
    {

//        DifficultyLevel = (Level)StaticSavingScript.LEVEL_DIFFICULTY;
        if(value==1)
        {
            washingMachineStopDuration = 12;
            gameTimeDuration = 300f;
            PassTimerToHUD(gameTimeDuration, washingMachineStopDuration, true);
            startGame = true;
        }
        if(value==2)
        {
            washingMachineStopDuration = 8;
            gameTimeDuration = 180f;
            PassTimerToHUD(gameTimeDuration, washingMachineStopDuration, true);
            startGame = true;
        }
        if(value==3)
        {
            washingMachineStopDuration = 4;
            gameTimeDuration = 120f;
            PassTimerToHUD(gameTimeDuration, washingMachineStopDuration, true);
            startGame = true;
        }
        #region switchR
        //alla fine di tutto
        //switch (DifficultyLevel)
        //{
        //    case Level.Easy:
        //        washingMachineStopDuration = 12;
        //        gameTimeDuration = 300f;
        //        PassTimerToHUD(gameTimeDuration, washingMachineStopDuration, true);
        //        startGame = true;
        //        break;
        //    case Level.Medium:
        //        washingMachineStopDuration = 8;
        //        gameTimeDuration = 180f;
        //        PassTimerToHUD(gameTimeDuration, washingMachineStopDuration, true);
        //        startGame = true;
        //        break;
        //    case Level.Hard:
        //        washingMachineStopDuration = 4;
        //        gameTimeDuration = 120f;
        //        PassTimerToHUD(gameTimeDuration, washingMachineStopDuration, true);
        //        startGame = true;   
        //        break;
        //}
        #endregion
    }



    public void PassTimerToHUD(float gameTimer, float stopTimer, bool passStopTimer)
    {
        HUD_Mgr.timeLeft = (int)gameTimer;

        if(passStopTimer)
            HUD_Mgr.timeToClose = (int)stopTimer;

    }

    public void SubscribePlatformGravitation(UnityAction<bool> onActivatePlatform, bool b)
    {
        if (b)
            GPlatformsEvent.AddListener(onActivatePlatform);
        else
            GPlatformsEvent.RemoveListener(onActivatePlatform);

    }

    public void SubscribeSendPosition(UnityAction<bool> onSetPlatformPos, bool b)
    {
        if (b)
            SendPositionEvent.AddListener(onSetPlatformPos);
        else
            SendPositionEvent.RemoveListener(onSetPlatformPos);

    }

    public static void RemoveLife()
    {
        HUD_Mgr.UpdateLifes(1);
    }

    public static void OnEndGame()
    {
        GameEnd = true;
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
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        InitializeGame(levelDiff);
             
        if (startGame)
        {
            if (!startOrbiting)
            {
                AutorotateOrbits(true);
                startOrbiting = true;
            }

            TickEndGame();

            //Script to End Game
            if(EndGame() || GameEnd)
            {
                YouLose();
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
                CloseTimerHUD.gameObject.SetActive(true);
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
                    CloseTimerHUD.gameObject.SetActive(false);
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

    public void AutorotateOrbits(bool b)
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
        PassTimerToHUD(gameTimeDuration - gameTimeTimer, washingMachineStopDuration - washingMachineStopTimer, true);
    }
    void TickEndGame()
    {
        gameTimeTimer += Time.deltaTime;
        PassTimerToHUD(gameTimeDuration - gameTimeTimer, 0, false);
    }
    bool TimeIsOver()
    {
        return washingMachineStopTimer >= washingMachineStopDuration;
    }
    public void BackToMenu()
    {
        //Debug.Log("ciao");
        Application.Quit();
    }
    public void YouWin()
    {
        CloseTimerHUD.gameObject.SetActive(false);
       winLoseUI.gameObject.SetActive(true);
       winLoseUI.GetComponentInChildren<Image>().sprite = win;
       SoundMgr.StopSound("MainTheme");
       SoundMgr.PlaySound("Win");
    }
    public void YouLose()
    {

        //Camera.main.

        winLoseUI.gameObject.SetActive(true);
        winLoseUI.GetComponentInChildren<Image>().sprite = lose;
        SoundMgr.StopSound("MainTheme");
        SoundMgr.PlaySound("Lose");
    }

    bool EndGame()
    {
        return gameTimeTimer >= gameTimeDuration;
    }

}

