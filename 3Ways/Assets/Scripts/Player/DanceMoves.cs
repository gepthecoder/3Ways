using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceMoves : MonoBehaviour
{
    public int iCurrentDanceMove;
    
    private void Awake()
    {
        GetDanceMove();
    }

    private void GetDanceMove()
    {
        iCurrentDanceMove = PlayerPrefs.GetInt("iCurrentDanceMove", 0);
    }

    public void PlayDanceAnime(int iDance, Animator controller)
    {
        switch(iDance)
        {
            case 0:
                controller.SetTrigger("dance0");
                break;

            case 1:
                controller.SetTrigger("dance1");
                break;

            case 2:
                controller.SetTrigger("dance2");
                break;
                
            case 3:
                controller.SetTrigger("dance3");
                break;
                
            case 4:
                controller.SetTrigger("dance4");
                break;
                
            case 5:
                controller.SetTrigger("dance5");
                break;

            default:
                controller.SetTrigger("dance0");

                break;
        }
    }
}
