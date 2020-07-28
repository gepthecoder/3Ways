using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStateMachine : MonoBehaviourPunCallbacks
{
    public static PlayerStateMachine PSM;

    public int iTempState;
    public int iCurrentState;

    //public bool bIsWaiting = true;

    private MyMovementController playerController;
    private PlayerChooseDoor chooseDoor;
    private PlayerCalculationManager calculations;
    private MapSpawner levelManager;
    private PlayerCageDoor cage;
    
    public enum PlayerStates
    {
        SPAWNING = 0,
        RUNNING,
        THINKING,
        UNLOCKING,
        ENTERING,
        PASS,
        REPEAT,
        TRANSITION,
        WIN,
    }

    public enum TempState
    {
        OTHER = 0,
        NEXT_STOP_WIN_ROOM,
    }

    private void OnEnable()
    {
        if (PlayerStateMachine.PSM == null)
        {
            PlayerStateMachine.PSM = this;
        }
    }

    void Start()
    {
        playerController = GetComponent<MyMovementController>();
        chooseDoor = GetComponent<PlayerChooseDoor>();
        cage = GetComponent<PlayerCageDoor>();
        levelManager = GetComponent<MapSpawner>();
        calculations = GetComponent<PlayerCalculationManager>();
    }

    void Update()
    {
        PLAYER_STATE_HANDLING();
    }

    private bool PLAYER_STATE_HANDLING()
    {
        if (!GameSetup.GS.bSTART_GAME)
        {
            iCurrentState = (int)PlayerStates.SPAWNING;
        }
        else
        {
            if (playerController.isPlayer1)
            {
                // RUNNING
                if (!StopCollider.choosingPosition && iCurrentState == (int)PlayerStates.SPAWNING)
                {
                    iCurrentState = (int)PlayerStates.RUNNING;
                    //TO:DO -> START GAME TIMER

                }
                // STOP PLAYER
                else if (StopCollider.choosingPosition && iCurrentState == (int)PlayerStates.RUNNING)
                {
                    // PLAYER REACHED THINKING ZONE -> STOP PLAYER
                    iCurrentState = (int)PlayerStates.THINKING;
                }
            }
            else
            {  // RUNNING
                if (!Stop2Collider.choosingPosition && iCurrentState == (int)PlayerStates.SPAWNING)
                {
                    iCurrentState = (int)PlayerStates.RUNNING;
                    //TO:DO -> START GAME TIMER

                }
                // STOP PLAYER
                else if (Stop2Collider.choosingPosition && iCurrentState == (int)PlayerStates.RUNNING)
                {
                    // PLAYER REACHED THINKING ZONE -> STOP PLAYER
                    iCurrentState = (int)PlayerStates.THINKING;
                }
            }
        }

        if (chooseDoor.doorPressed)
        {
            iCurrentState = (int)PlayerStates.UNLOCKING;
            chooseDoor.doorPressed = false;
        }

        if (playerController.isPlayer1)
        {
            if (OpenDoor.inFrontOfDoor)
            {
                iCurrentState = (int)PlayerStates.ENTERING;
                OpenDoor.inFrontOfDoor = false;
                StopCollider.choosingPosition = false;
                playerController.doorAnimeOpened = false;
            }
        }
        else
        {
            if (PlayerOpenDoor.inFrontOfDoor)
            {
                iCurrentState = (int)PlayerStates.ENTERING;
                PlayerOpenDoor.inFrontOfDoor = false;
                Stop2Collider.choosingPosition = false;
                playerController.doorAnimeOpened = false;
            }
        }

        if (calculations.currentCorrectDoor == chooseDoor.selectedDoor && iCurrentState == (int)PlayerStates.ENTERING && playerController.doorAnimeOpened)
        {
            //PASS LEVEL
            iCurrentState = (int)PlayerStates.PASS;
            levelManager.currentSectionCount++;
            levelManager.currentLevel++;

            chooseDoor.nTry = 0;


            if (levelManager.currentSectionCount == levelManager.GetNumberOfSections((int)PlayerCalculationManager.DIFFICULTIES.EASY))
            {
                iTempState = (int)TempState.NEXT_STOP_WIN_ROOM;
                levelManager.spawnWinSection = true;
                return true;
            }
            else if (levelManager.currentSectionCount >= 2)
            {
                levelManager.spawnNewSection = true;
                //StartCoroutine(TransitionToNextLevel());

                iTempState = (int)TempState.OTHER;
                playerController.doorAnimeOpened = false;
            }
            else if (levelManager.currentSectionCount == 1)
            {
                //just values no new section
                StartCoroutine(TransitionToNextLevel());

                iTempState = (int)TempState.OTHER;
                playerController.doorAnimeOpened = false;
            }
        }

        else if (calculations.currentCorrectDoor != chooseDoor.selectedDoor && iCurrentState == (int)PlayerStates.ENTERING && playerController.doorAnimeOpened)
        {
            // REPEAT CURRENT LEVEL
            //PlayrXP.iFailed++;
            iCurrentState = (int)PlayerStates.REPEAT;
            playerController.doorAnimeOpened = false;

        }
        // RETHINK
        if (playerController.isPlayer1)
        {
            if (StopCollider.choosingPosition && iCurrentState == (int)PlayerStates.REPEAT && transform.position == playerController.centerPlayerPos.position)
            {
                StartCoroutine(LookStraight());
                iCurrentState = (int)PlayerStates.THINKING;
            }
        }
        else
        {
            if (Stop2Collider.choosingPosition && iCurrentState == (int)PlayerStates.REPEAT && transform.position == playerController.centerPlayerPos.position)
            {
                StartCoroutine(LookStraight());
                iCurrentState = (int)PlayerStates.THINKING;
            }
        }
     

        return true;
    }


    IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(1f);
        //playerController.HandleValues();
        calculations.CreateEquation(calculations.currentDifficulty, levelManager.currentLevel);
        cage.enemiesSpawned = false;
    }

    IEnumerator LookStraight()
    {
        yield return new WaitForSeconds(2.6f);
        playerController.transform.LookAt(playerController.centerDoorLookPos);
    }

}
