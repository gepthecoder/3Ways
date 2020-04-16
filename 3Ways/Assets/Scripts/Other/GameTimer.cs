using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public static bool timeHasStarted;

    private Text gameTimer;
    private float time;
    
    void Start()
    {
        gameTimer = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (timeHasStarted)
        {
            time += Time.deltaTime;

            int seconds = (int)time % 60;
            int minutes = (int)(time / 60) % 60;

            string outputText = string.Format("{0:00}:{1:00}", minutes, seconds);

            gameTimer.text = outputText;
        }
    }
}
