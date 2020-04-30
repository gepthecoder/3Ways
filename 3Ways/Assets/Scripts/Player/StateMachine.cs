using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public static int iCurrentState;
    public static int iTempState;
    public static bool bIsWaiting = true;

    //OBJECTS
    private CalculationManager calcucaltions;
    private PlayerControl playerControl;
    private LevelManager levelManager;

    void Start() {  calcucaltions   = GetComponent<CalculationManager>();
                    playerControl   = GetComponent<PlayerControl>();
                    levelManager    = GetComponent<LevelManager>();
                 }

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
        OTHER=0 ,
            NEXT_STOP_WIN_ROOM,
    }

    void Update()
    {
        HandlePlayerStates();
    }

    protected bool HandlePlayerStates()
    {
       
        // MOVE TO NEXT LEVEL
        if (PlayerControl.transition)
        {

            playerControl.passCagePos = GetSelectedCageDoorTransform(ChooseDoor.selectedDoor);
            transform.LookAt(playerControl.passCagePos);

            Debug.Log("<color=brown>LOOK ATT</color>");
            iCurrentState = (int)PlayerStates.TRANSITION;
            PlayerControl.transition = false;
        }
        else
        {
            // SPAWNING
            if (bIsWaiting)
            {
                iCurrentState = (int)PlayerStates.SPAWNING;
            }
            else
            {
                // RUNNING
                if (!StopCollider.choosingPosition && iCurrentState == (int)PlayerStates.SPAWNING)
                {
                    iCurrentState = (int)PlayerStates.RUNNING;
                }
                // STOP PLAYER
                else if (StopCollider.choosingPosition && iCurrentState == (int)PlayerStates.RUNNING)
                {
                    // PLAYER REACHED THINKING ZONE -> STOP PLAYER
                    iCurrentState = (int)PlayerStates.THINKING;
                }

            }

            // Move To Position -> then to door -> then check for door validity
            if (ChooseDoor.doorPressed)
            {
                iCurrentState = (int)PlayerStates.UNLOCKING;
                ChooseDoor.doorPressed = false;
            }

            // FACE THE MONSTER OR GO TO NEXT LEVEL
            if (OpenDoor.inFrontOfDoor)
            {
                iCurrentState = (int)PlayerStates.ENTERING;
                OpenDoor.inFrontOfDoor = false;
                StopCollider.choosingPosition = false;
                PlayerControl.doorAnimeOpened = false;
            }

            if (calcucaltions.currentCorrectDoor == ChooseDoor.selectedDoor && iCurrentState == (int)PlayerStates.ENTERING && PlayerControl.doorAnimeOpened)
            {
                // PASS LEVEL
                iCurrentState = (int)PlayerStates.PASS;
                LevelManager.currentSectionCount++;
                Debug.Log("<color=blue>CURRENT SECTION = </color>" + LevelManager.currentSectionCount);
                LevelManager.currentLevel++;

                ChooseDoor.nTry = 0;
                //PlayerControl.changeData = true;
                
                if (LevelManager.currentSectionCount == levelManager.GetNumberOfSections((int)CalculationManager.DIFFICULTIES.EASY))
                {
                    Debug.Log("Next section is win room!");
                    iTempState = (int)TempState.NEXT_STOP_WIN_ROOM;
                    LevelManager.spawnWinSection = true;
                    return true;
                }
                else if (LevelManager.currentSectionCount >= 2)
                {
                    Debug.LogWarning("Spawn!");
                    LevelManager.spawnNewSection = true;
                    //StartCoroutine(TransitionToNextLevel());
                    Debug.Log("<color=red>iTempState Other</color>");

                    iTempState = (int)TempState.OTHER;
                    PlayerControl.doorAnimeOpened = false;
                }
                else if (LevelManager.currentSectionCount == 1)
                {
                    //just values no new section
                    Debug.Log("<color=red>current section is 1</color>");

                    StartCoroutine(TransitionToNextLevel());
                    Debug.Log("<color=red>iTempState Other</color>");

                    iTempState = (int)TempState.OTHER;
                    PlayerControl.doorAnimeOpened = false;
                }
               
            }
            else if (calcucaltions.currentCorrectDoor != ChooseDoor.selectedDoor && iCurrentState == (int)PlayerStates.ENTERING && PlayerControl.doorAnimeOpened)
            {
                // REPEAT CURRENT LEVEL
                iCurrentState = (int)PlayerStates.REPEAT;
                PlayerControl.doorAnimeOpened = false;

            }
            // RETHINK
            else if (StopCollider.choosingPosition && iCurrentState == (int)PlayerStates.REPEAT && transform.position == playerControl.centerPlayerPos.position)
            {
                StartCoroutine(LookStraight());
                iCurrentState = (int)PlayerStates.THINKING;
            }
        }
   

        return true;
    }

    IEnumerator LookStraight()
    {
        yield return new WaitForSeconds(2.6f);
        playerControl.transform.LookAt(playerControl.centerDoorLookPos);
        Debug.Log("<color=red>LOOK STRAIGHT</color>");
        //PlayerControl.canChooseDoor = true;
    }

    IEnumerator TransitionToNextLevel()
    {
        Debug.Log("<color=red>Waiiiitiiiing to transition</color>");
        yield return new WaitForSeconds(1f);
        Debug.Log("<color=red>Call create equation</color>");
        playerControl.HandleValues();
        calcucaltions.CreateEquation((int)CalculationManager.DIFFICULTIES.EASY, LevelManager.currentLevel);
        CageScript.enemiesSpawned = false;

    }

    IEnumerator JustWaitASec(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }

    private Transform GetSelectedCageDoorTransform(int selectedDoor)
    {
        Transform cageDoorPos;

        switch (selectedDoor)
        {
            case (int)ChooseDoor.Doors.DOOR0:
                cageDoorPos = playerControl.cageDoor0Pos;
                break;
            case (int)ChooseDoor.Doors.DOOR1:
                cageDoorPos = playerControl.cageDoor1Pos;
                break;
            case (int)ChooseDoor.Doors.DOOR2:
                cageDoorPos = playerControl.cageDoor2Pos;
                break;

            default:
                cageDoorPos = playerControl.cageDoor0Pos;
                Debug.LogError("Failed to request door.. door 0 choosen");
                break;
        }
        Debug.Log(cageDoorPos.name + " GET SELECTED TRANSFORM");
        return cageDoorPos;
    }
}
          
