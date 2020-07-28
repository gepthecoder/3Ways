using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class GameSetup : MonoBehaviourPun
{
    public static GameSetup GS;
    public static bool bPLAYER_WON;

    public Transform[] spawnPoints;

    public int numOfPlayers;
    public bool bSTART_COUNTDOWN;
    public bool bSTART_GAME;
    
    public Animator COUNT_DOWN_ANIME;

    public Animator WIN_ANIME;
    public Text playerName;

    private float timerToStartGame;


    private void OnEnable()
    {
        if(GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    void Start()
    {
        bSTART_GAME = false;
        bSTART_COUNTDOWN = false;
    }

    void Update()
    {
        if(numOfPlayers == 2 && bSTART_COUNTDOWN == false)
        {
            timerToStartGame += Time.deltaTime;

            if(timerToStartGame >= 3f)
            {
                PLAY_COUNTDOWN_ANIME();
                StartCoroutine(START_GAME());
                bSTART_COUNTDOWN = true;
                timerToStartGame = 0;
            }
          
        }

        if (bPLAYER_WON)
        {
            string WPlayerName = PlayerPrefs.GetString("WINNING_PLAYER_NAME");
            Debug.Log("Winning player name = " + WPlayerName);
            SET_WINNING_PLAYER_NAME(WPlayerName);
            StartCoroutine(PLAYER_WIN_GUI());
            bPLAYER_WON = false;
        }
    }

    public void PLAY_COUNTDOWN_ANIME()
    {
        if(COUNT_DOWN_ANIME != null)
        {
            COUNT_DOWN_ANIME.SetTrigger("321go");
        }
    }

    private IEnumerator START_GAME()
    {
        yield return new WaitForSeconds(5f);
        bSTART_GAME = true;
    }

    public IEnumerator PLAYER_WIN_GUI()
    {
        yield return new WaitForSeconds(6f);
        Debug.Log("set win gui trigger");
        photonView.RPC("DisplayWin", RpcTarget.All);
    }

    [PunRPC]
    public void DisplayWin()
    {
        WIN_ANIME.SetTrigger("won");
    }

    private void SET_WINNING_PLAYER_NAME(string name)
    {
        playerName.text = name;
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }

    public Animator animeBackToLobby;

    public void ShowGUI()
    {
        animeBackToLobby.SetTrigger("showHome");
    }

}
