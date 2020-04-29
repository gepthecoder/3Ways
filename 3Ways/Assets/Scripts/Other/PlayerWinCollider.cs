using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinCollider : MonoBehaviour
{
    public static bool PlayerWon;
    public static bool jump;

    public GameObject fireWorks;
    public Transform fireWorksPos;

    public GameObject coinsFly;
    public Transform coinsFlyPos;

    void Start()
    {
        PlayerWon = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("<color=red>PLAYER YOU WON... STOP AND DANCE!</color>");
            //stop the player and celebrate if won.. single player -> won always | multiplayer -> won anime only for winning player
            PlayerWon = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("<color=red>PLAYER YOU WON... STOP AND DANCE!</color>");
            //stop the player and celebrate if won.. single player -> won always | multiplayer -> won anime only for winning player
            jump = true;
            StartCoroutine(waitAndStopTime());
            SpawnFireWorks();
            SpawnCoins();
        }
    }

    IEnumerator waitAndStopTime()
    {
        yield return new WaitForSeconds(1f);
        GameTimer.timeHasStarted = false;
        yield return new WaitForSeconds(1f);
        CameraFollow.Win = true;
    }

    private void SpawnFireWorks()
    {
        Instantiate(fireWorks, fireWorksPos.position, fireWorksPos.rotation);
    }

    private void SpawnCoins()
    {
        Instantiate(coinsFly, coinsFlyPos.position, coinsFlyPos.rotation);
    }
}
