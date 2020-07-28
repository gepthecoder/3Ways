using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDanceMoves : MonoBehaviourPunCallbacks
{
    public int iCurrentDanceMove;

    private Animator anime;

    private void Awake()
    {
        GetDanceMove();
    }

    void Start()
    {
        anime = GetComponent<Animator>();
    }

    private void GetDanceMove()
    {
        iCurrentDanceMove = PlayerInfo.PI.playerDanceMove;
    }

    public void PlayDanceAnime(int iDance, Animator controller)
    {
        switch (iDance)
        {
            case 0:
                controller.SetTrigger("dance0");
                break;

            case 1:
                controller.SetTrigger("dance1");
                break;

            case 2:
                controller.SetTrigger("dance2");
                break;

            case 3:
                controller.SetTrigger("dance3");
                break;

            case 4:
                controller.SetTrigger("dance4");
                break;

            case 5:
                controller.SetTrigger("dance5");
                break;

            default:
                controller.SetTrigger("dance0");

                break;
        }
    }

    [PunRPC]
    public void PLAY_RPC_ANIME(int iDanceMove)
    {
        anime.SetTrigger("dance" + iDanceMove);
    }

    public void PLAY_DAMCE_MOVE(int dMove)
    {
        photonView.RPC("PLAY_RPC_ANIME", RpcTarget.All, dMove);
    }

}
