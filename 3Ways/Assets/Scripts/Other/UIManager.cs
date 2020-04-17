using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Animator CanvasAnimator;
    private Animator settingsUIanime;


    void Start()
    {
        CanvasAnimator = GetComponent<Animator>();
        settingsUIanime = settingsUI.GetComponent<Animator>();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////
    //                                  P A U S E   M E N U                                          //
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    
    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        CanvasAnimator.SetTrigger("showPauseMenu");   
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        CanvasAnimator.SetTrigger("closePauseMenu");
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////
    //                                  I N - G A M E  S E T T I N G S                               //
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject settingsUI;

    public void ShowSettings()
    {
        settingsUIanime.SetTrigger("showSettings");
    }

    public void HideSettings()
    {
        settingsUIanime.SetTrigger("hideSettings");
    }
}
