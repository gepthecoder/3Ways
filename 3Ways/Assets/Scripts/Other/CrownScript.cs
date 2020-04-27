using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrownScript : MonoBehaviour
{
    public int numberOfTries;
    public float timeThinking;

    private int totalReward;
    private int maxReward = 300;

    public Text rewardAmount_txt;

    public Animator anime;

    private CalculationManager calcMan;

    private int displayOnce;

    public Animator uiCrownCoinAnime;

    void Start()
    {
        totalReward = maxReward;
        displayOnce = 0;
        calcMan = GetComponent<CalculationManager>();
    }
    
    void Update()
    {
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
        if(ChooseDoor.nTry == 0) { Debug.Log("Hey DUDE number of tries is zero WTF"); return; }
        SetNumberOfTries(ChooseDoor.nTry);
        SET_REWARD(numberOfTries, timeThinking);
        anime.SetBool("makeitrain", true);
        timeThinking = 0;
    }

    public void SET_REWARD(int nTries, float timeElapsed)
    {
        if(displayOnce == 0)
        {
            int reward = (int)(totalReward / nTries) - ((int)timeElapsed) * nTries;
            if(reward < 0) { reward = 0; }
            Debug.Log("<color=yellow>REWARD: </color>" + reward + "<color=yellow> nTry: </color>" + nTries);
            
            CoinManager.CROWNS += reward;
            CoinManager.Save();

            rewardAmount_txt.text = reward.ToString();

            displayOnce = 1;
        }
        else { return; }
    }

}
