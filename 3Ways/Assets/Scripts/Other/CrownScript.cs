using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("<color=yellow>Yeeeeey I got a coin!!</color>");
            //Increase num of coins
            CoinManager.CROWNS++;
            CoinManager.Save();
            //Destroy
            Destroy(gameObject, 0.05f);
        }
    }
}
