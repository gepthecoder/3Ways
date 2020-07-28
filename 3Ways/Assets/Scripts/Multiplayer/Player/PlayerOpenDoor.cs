using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class PlayerOpenDoor : MonoBehaviourPunCallbacks
{
    public static bool inFrontOfDoor = false;

    public Animator anime;

    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.name.Contains("0"))
        { // 1st door
            photonView.RPC("OpenDoor0", RpcTarget.All, null);
        }
        else if (gameObject.name.Contains("1"))
        { // 2nd door
            photonView.RPC("OpenDoor1", RpcTarget.All, null);
        }
        else if (gameObject.name.Contains("2"))
        { // 3rd door
            photonView.RPC("OpenDoor2", RpcTarget.All, null);
        }

        inFrontOfDoor = true;
    }

    [PunRPC]
    public void OpenDoor0()
    {
        if (anime == null)
        {
            anime = GetComponentInParent<Animator>();
        }
        anime.SetBool("openDoor00", true);
    }

    [PunRPC]
    public void OpenDoor1()
    {
        if (anime == null)
        {
            anime = GetComponentInParent<Animator>();
        }
        anime.SetBool("openDoor01", true);
    }

    [PunRPC]
    public void OpenDoor2()
    {
        if (anime == null)
        {
            anime = GetComponentInParent<Animator>();
        }
        anime.SetBool("openDoor02", true);
    }
}
