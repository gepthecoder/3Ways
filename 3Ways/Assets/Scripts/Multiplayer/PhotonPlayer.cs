using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviourPunCallbacks
{
    private PhotonView PV;

    public GameObject myAvatar;
    private Animator animator;

    void Start()
    {
        PV = GetComponent<PhotonView>();

        int spawnPoint = PhotonNetwork.IsMasterClient ? 0 : 1;
        
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), 
                GameSetup.GS.spawnPoints[spawnPoint].position, GameSetup.GS.spawnPoints[spawnPoint].rotation, 0);
            animator = myAvatar.GetComponent<Animator>();
        }
    }

}
