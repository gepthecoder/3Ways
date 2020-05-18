using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignManager : MonoBehaviour
{

    //PLAYER NAME

    private string PlayerName;
    public Text playerNameTxt;

    private void SetPlayerName()
    {
        PlayerName = PlayerPrefs.GetString("playerName", "Unknown Player");
    }

    private void SetText_PlayerName()
    {
        playerNameTxt.text = PlayerName;
    }

    //

    //PLAYER AVATAR

    private int currentAvatar;
    public Image playerAvatar;
    public Sprite[] allAvatars;

    private void SetPlayerAvatar()
    {
        currentAvatar = PlayerPrefs.GetInt("iCurrentAvatar", 5);
    }

    private void SetSpriteImage(int iAvatar)
    {
        playerAvatar.sprite = allAvatars[iAvatar];
    }

    //

    //XP SLIDER + LEVELS

    private float currentSliderValue;
    private int currentXP = 0;
    private int currentLevel = 1;

    private float amountNeeded = 100.0f;
    private float startValue = 0f;

    public Text xProgressText;
    public Text firstNodeText;
    public Text secondNodeText;

    public Slider xpSlider;

    private void SetMinMaxValue(float min, float max)
    {
        xpSlider.minValue = min;
        xpSlider.maxValue = max;
    }

    private void SetPrefsSlider()
    {
        if (PlayerPrefs.HasKey("amountNeeded") || PlayerPrefs.HasKey("XPoints"))
        {
            // we had a previous session
            amountNeeded = PlayerPrefs.GetFloat("amountNeeded", 100);
            startValue = PlayerPrefs.GetFloat("startValue", 0);

            currentXP = PlayerPrefs.GetInt("XPoints");
            Debug.Log("<color=yellow>GET PREF</color> " + currentXP);
            currentLevel = PlayerPrefs.GetInt("currentLevel", 1);
        }
        else
        {
            SavePrefsSlider();
        }
    }

    private void SetSliderValue()
    {
        xpSlider.value = currentXP;
        Debug.Log("<color=blue>Set Slider Value:</color> " + currentXP);

    }

    private void SetTextElementsForPlayerInfo()
    {
        Debug.Log("<color=green>SET TEXT:</color> " + currentXP);
        xProgressText.text = xpSlider.value + " / " + amountNeeded;
        firstNodeText.text = currentLevel.ToString();
        secondNodeText.text = (currentLevel + 1).ToString();
    }

    
    public void SavePrefsSlider()
    {
        PlayerPrefs.SetFloat("amountNeeded", amountNeeded);
        PlayerPrefs.SetFloat("startValue", startValue);

        PlayerPrefs.SetInt("XPoints", currentXP);
        Debug.Log("<color=red>SET PREF</color> " + currentXP);

        PlayerPrefs.SetInt("currentLevel", currentLevel);
    }

    //

    //STAR COINS

    public int currentStarCoins = 0;

    public Text Coins_text;

    private void SetPrefsCoin()
    {
        if (PlayerPrefs.HasKey("XP"))
        {
            //we had a previous session
            currentStarCoins = PlayerPrefs.GetInt("XP", 0);
        }
        else
        {
            //Get saved values
            SaveCoinPrefs();
        }
    }

    private void SaveCoinPrefs()
    {
        PlayerPrefs.SetInt("XP", currentStarCoins);

    }

    private void SetTextCoins()
    {
        Coins_text.text = currentStarCoins.ToString();
    }

    //

    //DIFFICULTIES + BUTTON HANDLER

    public Text Medium_Txt;
    public Text Hard_Txt;
    public Text Genious_Txt;

    public Image lockSignMed;
    public Image lockSignHard;
    public Image lockSignGen;

    private int MedLimit = 50;
    private int HardLimit = 200;
    private int GenLimit = 750;

    private void SetTextValuesForDifficulties()
    {
        Medium_Txt.text = currentStarCoins.ToString() + " / " + MedLimit.ToString();
        Hard_Txt.text = currentStarCoins.ToString() + " / " + HardLimit.ToString();
        Genious_Txt.text = currentStarCoins.ToString() + " / " + GenLimit.ToString();
    }

    public Button EASY_BUTTON;
    public Button MEDIUM_BUTTON;
    public Button HARD_BUTTON;
    public Button GENIOUS_BUTTON;

    public GameObject STAR_MED;
    public GameObject STAR_HARD;
    public GameObject STAR_GEN;

    private void SetInteractabilityOfButtons()
    {
        if(currentStarCoins < 50)
        {
            //ALL DISABLED BUT EASY
            EASY_BUTTON.interactable = true;
            MEDIUM_BUTTON.interactable = false;
            HARD_BUTTON.interactable = false;
            GENIOUS_BUTTON.interactable = false;

        }else if(currentStarCoins >= MedLimit && currentStarCoins < HardLimit)
        {
            //HARD & GENIOUS DISABLED
            Destroy(lockSignMed.gameObject);
            Destroy(STAR_MED);

            EASY_BUTTON.interactable = true;
            MEDIUM_BUTTON.interactable = true;
            HARD_BUTTON.interactable = false;
            GENIOUS_BUTTON.interactable = false;

        }else if(currentStarCoins >= HardLimit && currentStarCoins < GenLimit)
        {
            //HARD & GENIOUS DISABLED
            Destroy(lockSignMed.gameObject);
            Destroy(STAR_MED);

            Destroy(lockSignHard.gameObject);
            Destroy(STAR_HARD);

            EASY_BUTTON.interactable = true;
            MEDIUM_BUTTON.interactable = true;
            HARD_BUTTON.interactable = true;
            GENIOUS_BUTTON.interactable = false;
        }

        else if (currentStarCoins >= GenLimit)
        {
            //HARD & GENIOUS DISABLED
            Destroy(lockSignMed.gameObject);
            Destroy(STAR_MED);

            Destroy(lockSignHard.gameObject);
            Destroy(STAR_HARD);

            Destroy(lockSignGen.gameObject);
            Destroy(STAR_GEN);

            EASY_BUTTON.interactable = true;
            MEDIUM_BUTTON.interactable = true;
            HARD_BUTTON.interactable = true;
            GENIOUS_BUTTON.interactable = true;
        }
    }
    //

    void Awake()
    {
        SetPlayerName();
        SetPlayerAvatar();
        SetPrefsSlider();
        SetPrefsCoin();
    }

    void Start()
    {
        SetText_PlayerName();
        SetSpriteImage(currentAvatar);
        SetMinMaxValue(startValue, amountNeeded);
        SetSliderValue();
        SetTextElementsForPlayerInfo();
        SetTextCoins();

        SetTextValuesForDifficulties();

        SetInteractabilityOfButtons();
    }
}
