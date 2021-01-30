using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Level { Easy,Medium,Hard}
[SerializeField]
public class ActivatePlatformsEvent : UnityEvent<bool> { }
public class SetPlatformPosition : UnityEvent<Vector3> { }
public class WashingMachineMgr : MonoBehaviour
{

    public List<PlatformScript> Platform = new List<PlatformScript>();
    public List<Transform> Orbits;

    public float GValue;
    public bool ApplyGAll, RemoveGForce;
    public bool PLAYSOUND;
    public ActivatePlatformsEvent GPlatformsEvent;
    public SetPlatformPosition SendPositionEvent;
    private float centrifugaNotMovingTimer;
    private float rotationSpeed;
    private bool startGame;
    bool stopMotionTrigger;

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

    void Start()
    {
        //RIEMPIRE LA LISTA DI USABLE positionList
    }


    void Update()
    {
             
        if (startGame)
        {
            TickEndGame();
            if(EndGame())
            {
                //FINE DEL GIOCO.
            }
            //METTE IN MOTO LE PIATTAFORME
            AutorotateOrbits();
            if (stopMotionTrigger)//QUANDO IL PLAYER RAGGIUNGE IL CENTRO DELLA LAVATRICE E TRIGGERA LO STOP
            {
                //FAI CADERE LE PIATTAFORME.
                GPlatformsEvent.Invoke(true);
                Tick();

                if (TimeIsOver()) //scade IL TEMPO DELLA CENTRIFUGA FERMA.
                {
                    ResetTimer();
                    //RIPOSIZIONARE LE PIATTAFORME ALLE POSIZIONI STABILITE.
                    GPlatformsEvent.Invoke(false);
                    SendPositions();
                }
            }
        }
    }

    void AutorotateOrbits()
    {
        for (int i = 0; i < Orbits.Count; i++)
        {
            Orbits[i].Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
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

                SendPositionEvent.Invoke(positionList[rndPosIndex]);
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
