using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayrXP : MonoBehaviour
{
    public static bool wonNowCalcualteGainedXP;

    public static int iFailed;
    public static int iStars;

    public static int XPoints = 0;

    public static int currentLevel = 1;

    public static int gainedXP;

    private int BonusBestTime;
    private int BonusGotAllStars;


    public Text TXT_XP_INFO;

    public Text TXT_XP_GAINED;

    void Awake()
    {
        if (PlayerPrefs.HasKey("XPoints"))
        {
            // we had a previous session
            XPoints = PlayerPrefs.GetInt("XPoints", 0);
            currentLevel = PlayerPrefs.GetInt("currentLevel", 1);
        }
        else
        {
            Save();
        }
    }

    void Update()
    {
        if (wonNowCalcualteGainedXP)
        {
            BonusBestTime = GameTimer.playerHasBeatRecord ? 100 : 0;
            BonusGotAllStars = LevelManager.iEASY_SECTIONS == iStars ? 100 : 0;
            int multiplier = BonusGotAllStars == 100 ? 8 : 6;
            int MinusTimeToLong = GameTimer.timeToLong ? -100 : 0;
            gainedXP = ((iStars) * multiplier) + BonusBestTime + BonusGotAllStars - MinusTimeToLong;
            if(gainedXP < 0) { gainedXP = 0; }
            Debug.Log("Gained XP: " + gainedXP);

            TXT_XP_INFO.text = XPoints + " / " + XPBar.xp_Needed;
            SET_TXT_GAINED_XP();
            wonNowCalcualteGainedXP = false;

        }
    }

    public static void Save()
    {
        PlayerPrefs.SetInt("XPoints", XPoints);
        PlayerPrefs.SetInt("currentLevel", currentLevel);

        PlayerPrefs.Save();
    }

    private void SET_TXT_GAINED_XP()
    {
        TXT_XP_GAINED.text = "You earned " + gainedXP.ToString() + " XP";
    }



}
