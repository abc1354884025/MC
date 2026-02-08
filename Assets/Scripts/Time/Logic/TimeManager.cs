using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
    

    private Season gameSeason=Season.Spring;

    public bool gameClockPause;

    private float tikTime;

    private void Awake()
    {
        NewGameTime();
    }
    private void Start()
    {
        EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
        EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
    }

    public void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;
            if (tikTime >= Settings.secondThreshold)
            {
                tikTime-=Settings.secondThreshold;
                UpdateGameTime();
            }
        }
    }
    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 16;
        gameDay = 0;
        gameMonth = 0;
        gameYear = 2026;
        gameSeason = Season.Spring;
    }
    private void UpdateGameTime()
    {
        gameSecond++;
        if(gameSecond>Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;
            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;
                if (gameHour > Settings.hourHold)
                {
                    gameHour = 0;
                    gameDay++;
                    if (gameDay > Settings.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;
                        if (gameMonth > Settings.monthHold)
                        {
                            gameMonth = 1;
                            gameSecond++;
                            if ((int)gameSeason > Settings.seasonHold)
                            {
                                gameSeason = Season.Spring;
                                gameYear++;
                            }
                        }
                    }
                }
                EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
        }

    }
}
