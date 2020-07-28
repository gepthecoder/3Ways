using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop2Collider : MonoBehaviour
{
    public static bool choosingPosition;

    void Start()
    {
        choosingPosition = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "P2")
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
