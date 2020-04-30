using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public static bool inFrontOfDoor = false;

    public void OnTriggerEnter(Collider other)
    {
        inFrontOfDoor = true;
    }
}
