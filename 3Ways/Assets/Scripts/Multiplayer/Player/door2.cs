using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door2 : MonoBehaviour
{
    public static bool slide;
    //public static bool transition;
    public Animator anime;

    public static bool updateChecks;

    void Start()
    {
        //transition = false;
        updateChecks = false;
        slide = false;
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "P2")
        {
            anime.SetTrigger("door" + obj.gameObject.GetComponent<PlayerChooseDoor>().selectedDoor);
            //transition = true;
            updateChecks = true;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "P2")
        {
            Debug.Log("SLIDE = TRUE");
            slide = true;
        }
    }
}
