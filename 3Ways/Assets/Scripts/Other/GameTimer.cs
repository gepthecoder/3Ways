using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public static bool won;
    public static bool timeHasStarted;
    public static bool playerHasBeatRecord;
    
    private Text gameTimer;
    private float gT;

    protected float bestTime = 0;

    public Text TXT_yourTime;
    public Text TXT_bestTime;

    void Awake()
    {
        if (PlayerPrefs.HasKey("bestTime"))
        {
            //we had a previous session
            bestTime = PlayerPrefs.GetFloat("bestTime", 0);
        }
        else
        {
            //first session
            Save();
        }
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("bestTime", bestTime);
        PlayerPrefs.Save();
    }

    void Start()
    {
        gameTimer = GetComponentInChildren<Text>();

        Debug.Log("Best time: " + bestTime);


        string minutes = ((int)bestTime / 60).ToString("00");
        string seconds = (bestTime % 60).ToString("00");
        string miliseconds = ((int)(bestTime * 100f) % 100).ToString("00");
        TXT_bestTime.text = minutes + ":" + seconds + ":" + miliseconds;
        Save();
    }

    void Update()
    {

        if (won)
        {
            Debug.Log("Game time END: " + gT);
            Debug.Log("Best time is: " + bestTime);

            if (bestTime == 0)
            {
                Debug.Log("SET BEST TIME!!");

                playerHasBeatRecord = true;
                bestTime = gT;

                string minutes = ((int)gT / 60).ToString("00");
                string seconds = (gT % 60).ToString("00");
                string miliseconds = ((int)(gT * 100f) % 100).ToString("00");
                TXT_bestTime.text = minutes + ":" + seconds + ":" + miliseconds;
                Save();
            }
            if (gT < bestTime)
            {
                playerHasBeatRecord = true;
                bestTime = gT;
                string minutes = ((int)bestTime / 60).ToString("00");
                string seconds = (bestTime % 60).ToString("00");
                string miliseconds = ((int)(bestTime * 100f) % 100).ToString("00");
                TXT_bestTime.text = minutes + ":" + seconds + ":" + miliseconds;
                Save();
            }
            won = false;
        }

        if (timeHasStarted)
        {
            gT += Time.deltaTime;

            string minutes = ((int)gT / 60).ToString("00");
            string seconds = (gT % 60).ToString("00");
            string miliseconds = ((int)(gT * 100f) % 100).ToString("00");

            gameTimer.text = minutes + ":" + seconds + ":" + miliseconds;
            TXT_yourTime.text = minutes + ":" + seconds + ":" + miliseconds;
        }
        else {
            return; }

       
    }


    public float GetCurrentGameTime()
    {
        return gT;
    }

    public void SetCurrentGameTime(float time)
    {
        gT = time;
    }
}
