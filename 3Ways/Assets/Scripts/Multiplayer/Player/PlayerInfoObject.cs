using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoObject : MonoBehaviour
{
    public static PlayerInfoObject PIO;

    public bool setVars;

    public int avatarSpriteValue1;
    public Image avatar1Img;

    public string player1Name;
    public Text TEXT_player1Name;

    public int player1Level;
    public Text TEXT_player1Level;

    public int avatarSpriteValue2;
    public Image avatar2Img;

    public string player2Name;
    public Text TEXT_player2Name;

    public int player2Level;
    public Text TEXT_player2Level;

    void Awake()
    {
        PIO = this;
    }

    void Update()
    {
        if (setVars)
        {
            string player1Name = GameObject.FindGameObjectWithTag("P1").GetComponent<PhotonView>().Owner.NickName;
            Debug.Log("P1 Name: " + player1Name);
            TEXT_player1Name.text = player1Name;

            string player2Name = GameObject.FindGameObjectWithTag("P2").GetComponent<PhotonView>().Owner.NickName;
            Debug.Log("P2 Name: " + player2Name);
            TEXT_player2Name.text = player2Name;


            int player1Sprite = GameObject.FindGameObjectWithTag("P1").GetComponent<PlayerSetup>().playerAvatar;
            avatar1Img.sprite = PlayerInfo.PI.allAvatarSprites[player1Sprite];
            Debug.Log("P1 sprite: " + player1Sprite);

            int player2Sprite = GameObject.FindGameObjectWithTag("P2").GetComponent<PlayerSetup>().playerAvatar;
            avatar2Img.sprite = PlayerInfo.PI.allAvatarSprites[player2Sprite];
            Debug.Log("P2 sprite: " + player2Sprite);


            int player1Level = GameObject.FindGameObjectWithTag("P1").GetComponent<PlayerSetup>().playerLevel;
            TEXT_player1Level.text = player1Level.ToString();
            Debug.Log("P1 lvl: " + player1Level);

            int player2Level = GameObject.FindGameObjectWithTag("P2").GetComponent<PlayerSetup>().playerLevel;
            TEXT_player2Level.text = player2Level.ToString();
            Debug.Log("P1 lvl: " + player2Level);

            setVars = false;
        }
    }

    //void Start()
    //{
        //PV = GetComponent<PhotonView>();

        //PV.RPC("RPC_AvatarSprite", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedAvatarSprite);
        //PV.RPC("RPC_PlayerName", RpcTarget.AllBuffered, PlayerInfo.PI.playerName);
        //PV.RPC("RPC_PlayerLevel", RpcTarget.AllBuffered, PlayerInfo.PI.playerLevel);
        
    //}

    public void LoadP1Name()
    {
        string player1Name = GameObject.FindGameObjectWithTag("P1").GetComponent<PhotonView>().Owner.NickName;
        RPC_PlayerName(player1Name, true);
    }

    public void LoadP2Name()
    {
        string player2Name = GameObject.FindGameObjectWithTag("P2").GetComponent<PhotonView>().Owner.NickName;
        RPC_PlayerName(player2Name, false);  
    }

    public void LoadP1Other()
    {
        int player1Sprite = GameObject.FindGameObjectWithTag("P1").GetComponent<PlayerSetup>().playerAvatar;
        RPC_AvatarSprite(player1Sprite, true);

        int player1Level = GameObject.FindGameObjectWithTag("P1").GetComponent<PlayerSetup>().playerLevel;
        RPC_PlayerLevel(player1Level, true);
    }



    public void LoadP2Other()
    {
        int player2Sprite = GameObject.FindGameObjectWithTag("P2").GetComponent<PlayerSetup>().playerAvatar;
        RPC_AvatarSprite(player2Sprite, false);

        int player2Level = GameObject.FindGameObjectWithTag("P2").GetComponent<PlayerSetup>().playerLevel;
        RPC_PlayerLevel(player2Level, false);
    }

    //[PunRPC]
    void RPC_AvatarSprite(int iSprite, bool p1)
    {
        if (p1)
        {
            avatarSpriteValue1 = iSprite;
            avatar1Img.sprite = PlayerInfo.PI.allAvatarSprites[iSprite];
        }
        else
        {
            avatarSpriteValue2 = iSprite;
            avatar2Img.sprite = PlayerInfo.PI.allAvatarSprites[iSprite];
        }
       
    }

    //[PunRPC]
    void RPC_PlayerName(string sName, bool p1)
    {
        if (p1)
        {
            player1Name = sName;
            TEXT_player1Name.text = sName;
        }
        else
        {
            player2Name = sName;
            TEXT_player2Name.text = sName;
        }
    }

    //[PunRPC]
    void RPC_PlayerLevel(int iLvL, bool p1)
    {
        if (p1)
        {
            player1Level = iLvL;
            TEXT_player1Level.text = iLvL.ToString();
        }
        else {
            player2Level = iLvL;
            TEXT_player2Level.text = iLvL.ToString();
        }
    }
}
