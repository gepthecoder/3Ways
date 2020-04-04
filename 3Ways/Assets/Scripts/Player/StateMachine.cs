using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public static int iCurrentState;
    public static bool bIsWaiting = true;

    //OBJECTS
    private CalculationManager calcucaltions;
    private PlayerControl playerControl;

    void Start() {  calcucaltions = GetComponent<CalculationManager>();
                    playerControl = GetComponent<PlayerControl>();
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

                if (LevelManager.currentSectionCount >= 2)
                {
                    LevelManager.spawnNewSection = true;
                }
                PlayerControl.doorAnimeOpened = false;
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
        yield return new WaitForSeconds(3f);
        playerControl.transform.LookAt(playerControl.centerDoorLookPos);
        PlayerControl.canChooseDoor = true;
    }


}
          
