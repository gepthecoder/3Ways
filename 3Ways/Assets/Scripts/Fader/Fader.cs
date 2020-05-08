using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour
{
    public Animator anime;

    public void FadeIn()
    {
        // end
        anime.SetTrigger("fadeIn");
    }

    public void FadeOut_Campaign()
    {
        anime.SetTrigger("fadeOutCampaign");
    }

    public void FadeOut_Multiplayer()
    {
        Debug.Log("No multiplayer jet :(.. somming soon :*");
        //anime.SetTrigger("fadeOutMultiplayer");
    }

    public void FadeOut()
    {
        // start
        anime.SetTrigger("fadeOut");
    }

    public void FadeToScene()
    {
        // LOAD GAME SCENE
        SetDefaultValues();
        SceneManager.LoadScene("GameScene");
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    private void SetDefaultValues()
    {
        // BRING BACK DEFAULT STATE
        StateMachine.bIsWaiting = true;
        StateMachine.iCurrentState = (int)StateMachine.PlayerStates.SPAWNING;
        CameraFollow.Win = false;

        door.slide = false;

        CageScript.playerHasBeenAttacked = false;
        CageScript.enemiesSpawned = false;

        GameTimer.won = false;
        GameTimer.timeHasStarted = false;
        GameTimer.playerHasBeatRecord = false;
        GameTimer.timeToLong = false;

        LevelManager.currentSectionCount = 0;
        LevelManager.currentLevel = 1;
        LevelManager.spawnNewSection = false;
        LevelManager.spawnWinSection = false;
        LevelManager.addNewSection = false;

        OpenDoor.inFrontOfDoor = false;

        PlayerWinCollider.PlayerWon = false;
        PlayerWinCollider.jump = false;

        PlayerXPbar.ShowGainedXP = false;

        StopCollider.choosingPosition = false;

        //timeadditionscript

        UIManager.GET_STAR = false;
        UIManager.LEVEL_UP = false;
        UIManager.NEW_RECORD = false;
        UIManager.WIN = false;

        WinStars.iStars = 0;

        ChooseDoor.nTry = 0;
        ChooseDoor.doorChoosen = false;

        PlayerControl.doorAnimeOpened = false;
        PlayerControl.passLevel = false;
        PlayerControl.canChooseDoor = false;
        PlayerControl.transition = false;
        PlayerControl.changeData = false;
        PlayerControl.isWinningSection = false;

        PlayrXP.iStars = 0;
        PlayrXP.iFailed = 0;


    }
}
