using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class multiplayerMenu : MonoBehaviour
{

    void Awake()
    {
        SetPlayerName();
        SetPlayerAvatar();
        SetPrefsSlider();
    }

    void Start()
    {
        SetMinMaxValue(startValue, amountNeeded);
        SetText_PlayerName();
        SetSpriteImage(currentAvatar);
        SetTextElementsForPlayerInfo();
        SetSliderValue();
    }

    void Update()
    {
        HANDLE_CLOSEBTN();
        ListOnlinePlayers();
        NumOfActiveRooms();
    }


    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


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


    public GameObject MatchMakingMenu;
    public GameObject EscButton;

    private void HANDLE_CLOSEBTN()
    {
        if (MatchMakingMenu.activeSelf)
        {
            EscButton.SetActive(true);
        }else
        {
            EscButton.SetActive(false);
        }
    }


    [SerializeField]
    private Text numOfOnlinePlayer;
    [SerializeField]
    private Text numOfOnlineRooms;

    public void ListOnlinePlayers()
    {
        numOfOnlinePlayer.text = PhotonNetwork.CountOfPlayers.ToString();

    }

    public void NumOfActiveRooms()
    {
        numOfOnlineRooms.text = PhotonNetwork.CountOfRooms.ToString();

    }

}
