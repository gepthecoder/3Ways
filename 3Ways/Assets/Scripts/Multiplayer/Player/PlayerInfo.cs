using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;

    public int mySelectedCharacter;
    public GameObject[] allCharacters;

    public int mySelectedAvatarSprite;
    public Sprite[] allAvatarSprites;

    public int playerLevel;
    public string playerName;

    public int playerDanceMove;

    public int gameRoomDifficulty;

    private void OnEnable()
    {
        if(PlayerInfo.PI == null){
            PlayerInfo.PI = this;
        }
        else
        {
            if(PlayerInfo.PI != this)
            {
                // reset singelton
                Destroy(PlayerInfo.PI.gameObject);
                PlayerInfo.PI = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        HandlePrefs_Character();
        HandlePrefs_AvatarSprites();
        HandlePrefs_PlayerLevel();
        HandlePrefs_PlayerName();
        HandlePrefs_DanceMove();
    }

    private void HandlePrefs_Character()
    {
        if (PlayerPrefs.HasKey("iCurrentCharacter"))
        {
            mySelectedCharacter = PlayerPrefs.GetInt("iCurrentCharacter", 0);
            mySelectedCharacter = 0;
        }
        else
        {
            mySelectedCharacter = 0;
            Debug.Log("Creating key!");
            PlayerPrefs.SetInt("iCurrentCharacter", mySelectedCharacter);
        }
    }

    private void HandlePrefs_AvatarSprites()
    {

        if (PlayerPrefs.HasKey("iCurrentAvatar"))
        {
            mySelectedAvatarSprite = PlayerPrefs.GetInt("iCurrentAvatar", 5);
        }
        else
        {
            mySelectedAvatarSprite = 5;
            PlayerPrefs.SetInt("iCurrentAvatar", mySelectedAvatarSprite);
        }
    }

    private void HandlePrefs_PlayerLevel()
    {
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            playerLevel = PlayerPrefs.GetInt("currentLevel", 1);
        }
        else
        {
            playerLevel = 1;
            PlayerPrefs.SetInt("currentLevel", playerLevel);
        }

    }

    private void HandlePrefs_PlayerName()
    {
        playerName = PlayerPrefs.GetString("playerName", "Unknown Player");
        PhotonNetwork.NickName = playerName;
        Debug.Log("player name is: " + playerName);
    }

    private void HandlePrefs_DanceMove()
    {
        if (PlayerPrefs.HasKey("iCurrentDanceMove"))
        {
            playerDanceMove = PlayerPrefs.GetInt("iCurrentDanceMove", 0);
        }else
        {
            playerDanceMove = 0;
            PlayerPrefs.SetInt("iCurrentDanceMove", playerDanceMove);
        }
        
    }

    private void HandlePrefs_GameRoomDifficulty()
    {
        if (PlayerPrefs.HasKey("currentMultiplayerDifficulty"))
        {
            // we had a previous session
            gameRoomDifficulty = PlayerPrefs.GetInt("currentMultiplayerDifficulty", 0);
        }
        else
        {
            PlayerPrefs.SetInt("currentMultiplayerDifficulty", gameRoomDifficulty);
        }
    }
}
