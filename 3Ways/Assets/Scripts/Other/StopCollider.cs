using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCollider : MonoBehaviour
{
    public static bool choosingPosition;

    void Start()
    {
        choosingPosition = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //TO:DO
            //      -> stop the player if the door wasn't pre-selected
            //      -> think position
            //      -> waiting player to choose the right door 

            Debug.Log("Choosing door...");
            choosingPosition = true;
        }
    }

}
