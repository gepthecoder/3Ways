using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAdditionScript : MonoBehaviour
{
    public static float totalTimeAmount;

    public GameTimer    gT;
    public PauseTimer   pT;

    // time addition event 01
    public void AddTime()
    {
        gT.SetCurrentGameTime(totalTimeAmount);
        Debug.Log("Game time is now: " + gT.GetCurrentGameTime());
    }

    // time addition event 02 -> progress game
    public void Continue()
    {
        Debug.Log("Start game again!!!");
        //Time.timeScale = 1;
        UIManager.pauseMenuOpened = false;
    }
}
