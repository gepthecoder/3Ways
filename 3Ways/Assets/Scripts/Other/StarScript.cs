using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("<color=green>Yeeeeey I got XP COIN aka star!!</color>");
            //Increase num of coins
            UIManager.GET_STAR = true;
            //Destroy
            Destroy(gameObject, 0.05f);
        }
    }
}
