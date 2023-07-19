using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameLevel loadedlevel;

    [SerializeField]
    GameObject Player;

    float leveltimer;
    float objectiveCount;
    float collectedobjectives;
    bool timeractive;

    public delegate void TimerChangeEventHandler(float time);
    public delegate void GameConditionEventHandler();
    public delegate void PickupValueEventHandler(float current, float max);
    public delegate void GameoverEventHandler(bool isWon);


    public static event TimerChangeEventHandler GameTimerEvent;
    public static event GameConditionEventHandler GameConditionEvent;
    public static event PickupValueEventHandler UpdatePickupCountEvent;

    public static event GameoverEventHandler GameOverEvent;



    void Initlevel(GameLevel level)
    {
        loadedlevel = level;
        SetConditions();

        //spawn player at position
        Player.GetComponent<PlayerObject>().PlayerPickupEvent += IncreaseObjectiveValue;

    }

    void StartLevel()
    {
        //unpause the game and start the timer

        timeractive = true;
    }


    //remove this later
    [SerializeField]
    GameLevel leveltest;
    private void Start()
    {
        Initlevel(leveltest);
        StartLevel();
    }


    void SetConditions()
    {
        if (loadedlevel.istimed)
        {
            leveltimer = loadedlevel.timelimit;
        }
        else { leveltimer = 0; }

        if (loadedlevel.requirepickup)
        {
            objectiveCount = loadedlevel.pickupcount;
            UpdatePickupCountEvent?.Invoke(collectedobjectives, objectiveCount);
        }
        else { objectiveCount = 0; }
    }

    void IncreaseObjectiveValue(Pickup pickup)
    {
        collectedobjectives++;
        UpdatePickupCountEvent?.Invoke(collectedobjectives, objectiveCount);

        if (collectedobjectives >= objectiveCount) { WinLevel(); }
    }



    private void FixedUpdate()
    {
        if (timeractive) { Timer(); }
    }

    //win conditions are collecting all required collectible types and(if exists) touching the finish line 
    void WinLevel()
    {
        timeractive = false;
        GameOverEvent?.Invoke(true);
    }


    //lose conditions are if time runs out or player is out of bounds.
    void LoseLevel()
    {
        timeractive = false;
        GameOverEvent?.Invoke(false);

    }

    void Timer()
    {
        //reduce time if timed,otherwise increase from zero
        if (loadedlevel.istimed)
        {
            leveltimer -= Time.deltaTime;

            if (leveltimer <= 0)
            {

                Debug.Log("Time out");
                LoseLevel();

            }
        }

        else
        {
            leveltimer += Time.deltaTime;
        }

        GameTimerEvent?.Invoke(leveltimer);
    }


}
