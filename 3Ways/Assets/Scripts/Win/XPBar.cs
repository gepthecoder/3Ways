using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public static bool ShowGainedXP;

    public Slider xp_Slider;
    public static int xp_Needed = 250;

    private float levelBarWidth = 250f;
    
    /// TEXT ELEMENTS ///
    
    public Text TXT_XP_INFO;
    public Text TXT_CURRENT_LVL;
    public Text TXT_NEXT_LVL;


    void Awake()
    {
        if (PlayerPrefs.HasKey("xp_Needed"))
        {
            // we had a previous session
            xp_Needed = PlayerPrefs.GetInt("xp_Needed", 250);
        }
        else
        {
            Save();
        }
    }

    void Start()
    {
        SET_MAX_XP_VALUE_ON_SLIDER();
        SET_XP_VALUE_ON_SLIDER(PlayrXP.XPoints, xp_Needed);
    }

    void Update()
    {
        if (ShowGainedXP)
        {
            PlayrXP.XPoints += PlayrXP.gainedXP;
            PlayrXP.Save();
            SET_XP_VALUE_ON_SLIDER(PlayrXP.XPoints, xp_Needed);
            HandleXP_Bar();
            TXT_XP_INFO.text = PlayrXP.XPoints + " / " + xp_Needed;

            ShowGainedXP = false;
            Debug.Log("my xp: " + PlayrXP.XPoints + " need: " + xp_Needed);

        }
    }

    private void SET_XP_NEEDED()
    {
        xp_Needed = xp_Needed * 2;
        Save();

        SET_MAX_XP_VALUE_ON_SLIDER();
    }

    public void HandleXP_Bar()
    {
        if (PlayrXP.XPoints >= xp_Needed)
        {
            // level up
            PlayrXP.currentLevel++;
            PlayrXP.Save();

            // reset slider
            SET_XP_NEEDED();
            SET_MAX_XP_VALUE_ON_SLIDER();
            SET_XP_VALUE_ON_SLIDER(PlayrXP.XPoints, xp_Needed);

            TXT_XP_INFO.text = PlayrXP.XPoints + " / " + xp_Needed;
            TXT_CURRENT_LVL.text = PlayrXP.currentLevel.ToString();
            TXT_NEXT_LVL.text = PlayrXP.currentLevel+1.ToString();
        }
    }

  
    public void SET_MAX_XP_VALUE_ON_SLIDER()
    {
        xp_Slider.maxValue = xp_Needed;
        levelBarWidth = xp_Needed;
    }

    public void SET_XP_VALUE_ON_SLIDER(int currentXP, int XPneeded)
    {
        Debug.Log("XP NEEDED: " + XPneeded);
        Debug.Log("XP current: " + currentXP);

        //float progress = currentXP / XPneeded * 100;
        //Debug.Log("XP Slider val: " + progress / 100 * levelBarWidth);
        //Debug.Log("XP progress: " + progress);
        //Debug.Log("XP progress/100: " + progress/100);

        xp_Slider.value = currentXP /*progress / 100 * levelBarWidth*/;
    }
    

    public static void Save()
    {
        PlayerPrefs.SetInt("xp_Needed", xp_Needed);

        PlayerPrefs.Save();
    }
}
