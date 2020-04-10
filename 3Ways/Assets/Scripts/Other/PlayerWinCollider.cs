﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinCollider : MonoBehaviour
{
    public static bool PlayerWon;

    void Start()
    {
        PlayerWon = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //stop the player and celebrate if won.. single player -> won always | multiplayer -> won anime only for winning player
            PlayerWon = true;
        }
    }
}
