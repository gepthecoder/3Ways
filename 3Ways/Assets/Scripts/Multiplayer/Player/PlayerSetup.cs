using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    private PhotonView PV;

    public GameObject myCharacter;
    public int characterValue;

    public string playerName;
    public int playerLevel;
    public int playerAvatar;


    public Camera myCamera;
    public AudioListener myAL;

    private Camera tempP1Cam;
    private Camera tempP2Cam;

    bool startProcedure;

    void Update()
    {
        if(GameSetup.GS.numOfPlayers == 2 && startProcedure)
        {
            PlayerInfoObject.PIO.setVars = true;

            startProcedure = false;
        }
    }

    void Start()
    {
        GameSetup.GS.numOfPlayers++;
        Debug.Log("Number of Players: " + GameSetup.GS.numOfPlayers);

        PV = GetComponent<PhotonView>();

        tempP1Cam = GameObject.Find("CamP1").GetComponent<Camera>();
        tempP2Cam = GameObject.Find("CamP2").GetComponent<Camera>();

        playerLevel = PlayerInfo.PI.playerLevel;
        playerAvatar = PlayerInfo.PI.mySelectedAvatarSprite;

        if (PV.IsMine)
        {
            playerName = PhotonNetwork.NickName;

            startProcedure = true;
            //PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedCharacter);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Camera 1 -> player 1 master client");
                myCamera.rect = new Rect(0f, 0f, .5f, 1f);
                Destroy(tempP1Cam);

                gameObject.tag = "P1";
            }
            else
            {
                myCamera.rect = new Rect(.5f, 0f, .5f, 1f);
                Destroy(tempP2Cam);

                gameObject.tag = "P2";
            }
        }
        else
        {
            playerName = PV.Owner.NickName;

            if (PhotonNetwork.IsMasterClient)
            {
                gameObject.tag = "P2";
                myCamera.rect = new Rect(.5f, 0f, .5f, 1f);
                Destroy(tempP2Cam);
            }
            else
            {
                myCamera.rect = new Rect(0f, 0f, .5f, 1f);
                gameObject.tag = "P1";
                Destroy(tempP1Cam);
            }
            //Destroy(myCamera);

            Destroy(myAL);
        }


        //PlayerInfoObject.PIO.LoadP1Name();
        //PlayerInfoObject.PIO.LoadP2Name();

        //PlayerInfoObject.PIO.LoadP1Other();
        //PlayerInfoObject.PIO.LoadP2Other();

        PlayerLevelBar.PLB.Load();
    }

    [PunRPC]
    void RPC_AddCharacter(int iCurrentChar)
    {
        characterValue = iCurrentChar;
        myCharacter = Instantiate(PlayerInfo.PI.allCharacters[iCurrentChar], transform.position, transform.rotation, transform);
    }


}
