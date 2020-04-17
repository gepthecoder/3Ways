using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public static bool timeHasStarted;

    private Text gameTimer;
    private float t;
    
    void Start()
    {
        gameTimer = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (timeHasStarted)
        {
            t += Time.deltaTime;

            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");
            string miliseconds = ((int)(t * 100f) % 100).ToString("00");

            gameTimer.text = minutes + ":" + seconds + ":" + miliseconds;
        }else { return; }

    }
}
