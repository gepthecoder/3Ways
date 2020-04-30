using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public static bool timeHasStarted;
    
    private Text gameTimer;
    private float gT;

    
    void Start()
    {
        gameTimer = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (timeHasStarted)
        {
            gT += Time.deltaTime;

            string minutes = ((int)gT / 60).ToString("00");
            string seconds = (gT % 60).ToString("00");
            string miliseconds = ((int)(gT * 100f) % 100).ToString("00");

            gameTimer.text = minutes + ":" + seconds + ":" + miliseconds;
        }
        else { return; }
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
