using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrownScript : MonoBehaviour
{
    private int numberOfTries;
    public float timeThinking;

    private int totalReward;
    private int maxReward = 300;

    public Text rewardAmount_txt;

    public Animator anime;

    private CalculationManager calcMan;

    private int displayOnce;

    void Start()
    {
        totalReward = maxReward;
        displayOnce = 0;
        calcMan = GetComponent<CalculationManager>();
    }
    
    void Update()
    {
        SetNumberOfTries(ChooseDoor.nTry);
        HandleTimeEffect();
    }

    private void HandleTimeEffect()
    {
        if (StateMachine.iCurrentState == (int)StateMachine.PlayerStates.THINKING)
        {
            playerThinkingTime(true);
        }
        else if (StateMachine.iCurrentState == (int)StateMachine.PlayerStates.TRANSITION)
        {
            displayOnce = 0;
            playerThinkingTime(false);
        }
        else if (StateMachine.iCurrentState == (int)StateMachine.PlayerStates.REPEAT)
        {
            playerThinkingTime(true);
        }    
        else if(StateMachine.iCurrentState == (int)StateMachine.PlayerStates.UNLOCKING)
        {
            if (ChooseDoor.selectedDoor == calcMan.currentCorrectDoor)
            {
                Debug.Log("GEET REWAAARD!");
                ResetThinkingTime();
            }
        }
        else
        {
            anime.SetBool("makeitrain", false);
        }
    }

    private void SetNumberOfTries(int nTry)
    {
        numberOfTries = nTry;
    }

    private void playerThinkingTime(bool thinking)
    {
        if (thinking)
        {
            timeThinking += Time.deltaTime;
        }
        else { return; }
    }

    private void ResetThinkingTime()
    {
        SET_REWARD(numberOfTries, timeThinking);
        anime.SetBool("makeitrain", true);
        timeThinking = 0;
    }

    private void SET_REWARD(int nTries, float timeElapsed)
    {
        if(displayOnce == 0)
        {
            int reward = (int)(totalReward / nTries) - ((int)timeElapsed) * nTries;
            Debug.Log("<color=yellow>REWARD: </color>" + reward);
            rewardAmount_txt.text = reward.ToString();
            CoinManager.CROWNS += reward;
            CoinManager.Save();

            displayOnce = 1;
        }
        else { return; }
    }

}
