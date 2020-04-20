﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private Animator CanvasAnimator;
    private Animator settingsUIanime;

    private GameTimer gameTimer;
    private PauseTimer pauseTimer;

    public GameObject timeAddition;
    public GameObject CountDown;

    void Start()
    {
        CanvasAnimator = GetComponent<Animator>();
        settingsUIanime = settingsUI.GetComponent<Animator>();
        gameTimer = GetComponentInChildren<GameTimer>();
        pauseTimer = GetComponentInChildren<PauseTimer>();
    }

    void Update()
    {
        if (PlayerControl.bStartCountDown)
        {
            Animator anime = CountDown.GetComponent<Animator>();
            anime.SetTrigger("321go");
            PlayerControl.bStartCountDown = false;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////
    //                                  P A U S E   M E N U                                          //
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    
    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        CanvasAnimator.SetTrigger("showPauseMenu");   
    }

    public void ClosePauseMenu()
    {
        pauseTimer.pauseMenuOpened = false;
        CanvasAnimator.SetTrigger("closePauseMenu");
        CalculateGameTime();
    }

    private void CalculateGameTime()
    {
        // ADD TIME TOGETHER
        float pausedTime = pauseTimer.GetCurrentTime();
        float gameTime = gameTimer.GetCurrentGameTime();

        float totalTime = gameTime + pausedTime;

        // APPEND PAUSED TIME TO ANIME
        string minutes = ((int)pausedTime / 60).ToString("00");
        string seconds = (pausedTime % 60).ToString("00");
        string miliseconds = ((int)(pausedTime * 100f) % 100).ToString("00");

        string timeAdd = minutes + ":" + seconds + ":" + miliseconds;
        Text timeAddTxt = timeAddition.GetComponent<Text>();
        timeAddTxt.text = timeAdd;

        SetTotalTimeAmount(totalTime);
        Debug.Log("Paused time: " + pausedTime + " Game time: " + gameTime + " Total time: " + totalTime);
    }

    private void SetTotalTimeAmount(float totalTime)
    {
        TimeAdditionScript.totalTimeAmount = totalTime;
    }

    public void PauseMenuClosed()
    {
        // called in anime event: closePauseMenu

        timeAddition.GetComponent<Animator>().SetTrigger("AddTime");
    }

    public void StartPauseTimer()
    {
        pauseTimer.pauseMenuOpened = true;
    }

    IEnumerator waitAndDoSomething(float t)
    {
        yield return new WaitForSeconds(t);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////
    //                                  I N - G A M E  S E T T I N G S                               //
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject settingsUI;

    public void ShowSettings()
    {
        settingsUIanime.SetTrigger("showSettings");
    }

    public void HideSettings()
    {
        settingsUIanime.SetTrigger("hideSettings");
    }
}
