using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class mainMenuScript : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    //                                  B U T T O N S  H A N D L E R                                 //
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    public Fader fade;
    public Animator darkenCreditsQuit;


    public Animator CREDITS_ANIME;
    public Animator QUIT_ANIME;

    //CAMPAING
    public void OpenCampaignScene()
    {
        fade.FadeOut_Campaign();
    }

    //

    //MULTIPLAYER
    public void OpenMultiplayerScene()
    {
        fade.FadeOut_Multiplayer();
    }

    //

    //INFO + darken
    
    public void ShowCredits()
    {
        darkenCreditsQuit.SetTrigger("darken");
        CREDITS_ANIME.SetTrigger("showCredits");
    }

    public void CloseCredits()
    {
        darkenCreditsQuit.SetTrigger("undarken");
        CREDITS_ANIME.SetTrigger("hideCredits");
    }
    
    //

    //EXIT

    public void ShowQuit()
    {
        darkenCreditsQuit.SetTrigger("darken");
        QUIT_ANIME.SetTrigger("showQuit");
    }

    public void HideQuit()
    {
        darkenCreditsQuit.SetTrigger("undarken");
        QUIT_ANIME.SetTrigger("hideQuit");
    }

    //activate when release
    public void ExitApplication()
    {
        Debug.Log("Quiting application!!");
        //Application.Quit();
    }

    //


    ///////////////////////////////////////////////////////////////////////////////////////////////////
    //                                            A V A T A R S                                      //
    ///////////////////////////////////////////////////////////////////////////////////////////////////


    private Animator AVATAR_ANIME;

   
    public void ShowChooseAvtarGUI()
    {
        AVATAR_ANIME.SetTrigger("showAvatars");
    }

    public void HideChooseAvtarGUI()
    {
        AVATAR_ANIME.SetTrigger("hideAvatars");
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////
    //                                            P L A Y E R  N A M E                               //
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    public InputField inputField;
    
    public Animator SHOW_TYPE_NAME_ANIME;

    public string playerName = "Unknown Player";

    public Text PlayerNameText;

    private int defaultFontSize_PlayerName = 61;


    public void ShowTypeName()
    {
        SHOW_TYPE_NAME_ANIME.SetTrigger("showTypeName");
    }

    public void HideTypeName()
    {
        SHOW_TYPE_NAME_ANIME.SetTrigger("hideTypeName");
        //GET TYPED NAME
        playerName = inputField.text;
        if(playerName == string.Empty) { playerName = "Unknown Player"; }
        if(playerName.Length > 7)
        {
            PlayerNameText.fontSize = 39;
        }
        else
        {
            PlayerNameText.fontSize = defaultFontSize_PlayerName;
        }
        PlayerNameText.text = playerName;
        SavePlayerName();
    }

    public void SavePlayerName()
    {
        PlayerPrefs.SetString("playerName", playerName);
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////
    //                                            P L A Y E R  L E V E L                             //
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    private int playerLvl = 1;
    public Text txtPlayerLvl;

    private void SetPlayerLvlPref()
    {
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            playerLvl = PlayerPrefs.GetInt("currentLevel");
        }
        else
        {
            PlayerPrefs.SetInt("currentLevel", playerLvl);
        }
    }

    private void SetTextLvl()
    {
        txtPlayerLvl.text = playerLvl.ToString();
    }


    void Awake()
    {
        if (PlayerPrefs.HasKey("playerName"))
        {
            //we had a previous session
            playerName = PlayerPrefs.GetString("playerName", "Unknown Player");

        }
        else
        {
            // save
            SavePlayerName();
        }

        SetPlayerLvlPref();

        AVATAR_ANIME = GetComponent<Animator>();

    }
    void Start()
    {
        if (playerName.Length > 7)
        {
            PlayerNameText.fontSize = 39;
        }
        else
        {
            PlayerNameText.fontSize = defaultFontSize_PlayerName;
        }
        PlayerNameText.text = playerName;
        SetTextLvl();
    }


}
