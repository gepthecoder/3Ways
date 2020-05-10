using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameShop : MonoBehaviour
{
    private int iCurrentCharacter;

    protected const int CHARACTER_PRICE_OG = 100000;
    protected const int CHARACTER_PRICE_EX = 750000;

    private int OG_BOUGHT = 0;
    private int EX_BOUGHT = 0;

    private int iCurrentCrowns;
    public Text crownsAmountTxt;

    public GameObject PRICE_GUI_OG;
    public GameObject USE_BUTTON_OG;
    public GameObject BUY_BUTTON_OG;


    public GameObject PRICE_GUI_EXPLORER;
    public GameObject USE_BUTTON_EXPLORER;
    public GameObject BUY_BUTTON_EXPLORER;

    public GameObject FRAME_TRIPPING_ROBOT;
    public GameObject FRAME_TRIPPING_OG;
    public GameObject FRAME_TRIPPING_EXPLORER;

    public Animator ANIME_SHOP;

    void Awake()
    {
        GetPrefsChar();
        GetPrefsCharBought();
        GetPrefsCrowns();
    }

    void Start()
    {
        SetTextCrowns();
        SetFrame(iCurrentCharacter);
        HandleGUIs(OG_BOUGHT, EX_BOUGHT);
    }

    // PREFS

    private void GetPrefsChar()
    {
        if (PlayerPrefs.HasKey("iCurrentCharacter"))
        {
            //we had a previous session
            iCurrentCharacter = PlayerPrefs.GetInt("iCurrentCharacter", 0);
        }
        else
        {
            SetPrefsChar();
        }
    }

    private void SetPrefsChar()
    {
        PlayerPrefs.SetInt("iCurrentCharacter", iCurrentCharacter);
    }


    private void GetPrefsCharBought()
    {
        if (PlayerPrefs.HasKey("OG_BOUGHT") || PlayerPrefs.HasKey("EX_BOUGHT"))
        {
            //we had a previous session
            OG_BOUGHT = PlayerPrefs.GetInt("OG_BOUGHT", 0);
            EX_BOUGHT = PlayerPrefs.GetInt("EX_BOUGHT", 0);
        }
        else
        {
            SetPrefsCharBought();
        }
    }

    private void SetPrefsCharBought()
    {
        PlayerPrefs.SetInt("OG_BOUGHT", OG_BOUGHT);
        PlayerPrefs.SetInt("EX_BOUGHT", EX_BOUGHT);
    }


    private void GetPrefsCrowns()
    {
        if (PlayerPrefs.HasKey("CROWNS"))
        {
            //we had a previous session
            iCurrentCrowns = PlayerPrefs.GetInt("CROWNS", 0);
        }
        else
        {
            SetPrefsCrowns();
        }
    }

    private void SetPrefsCrowns()
    {
        PlayerPrefs.SetInt("CROWNS", iCurrentCrowns);
    }

    private void SetTextCrowns()
    {
        crownsAmountTxt.text = iCurrentCrowns.ToString();
    }

    //

    // BUTTON EVENTS

    public void OpenShop()
    {
        ANIME_SHOP.SetTrigger("showShop");
    }

    public void CloseShop()
    {
        ANIME_SHOP.SetTrigger("hideShop");

    }

    public void BuyCharacterOG()
    {
        if(iCurrentCrowns >= CHARACTER_PRICE_OG)
        {
            //buy item
            iCurrentCrowns -= CHARACTER_PRICE_OG;
            SetPrefsCrowns();
            SetTextCrowns();

            OG_BOUGHT = 1;
            SetPrefsCharBought();

            Destroy(PRICE_GUI_OG);
            Destroy(BUY_BUTTON_OG);
            USE_BUTTON_OG.SetActive(true);
        }
        else { Debug.Log("Declined buy! -> no resources"); }
    }

    public void BuyCharacterEX()
    {
        if (iCurrentCrowns >= CHARACTER_PRICE_EX)
        {
            //buy item
            iCurrentCrowns -= CHARACTER_PRICE_EX;
            SetPrefsCrowns();
            SetTextCrowns();

            EX_BOUGHT = 1;
            SetPrefsCharBought();

            Destroy(PRICE_GUI_EXPLORER);
            Destroy(BUY_BUTTON_EXPLORER);
            USE_BUTTON_EXPLORER.SetActive(true);
        }
        else { Debug.Log("Declined buy! -> no resources"); }
    }

    public void UseCharacter(int iChoosen)
    {
        iCurrentCharacter = iChoosen;
        SetPrefsChar();
        SetFrame(iChoosen);
    }

    //

    // HANDLERS

    private void SetFrame(int iChoosen)
    {
        switch (iChoosen)
        {
            case 0:
                FRAME_TRIPPING_ROBOT.SetActive(true);
                FRAME_TRIPPING_OG.SetActive(false);
                FRAME_TRIPPING_EXPLORER.SetActive(false);
                break;
            case 1:
                FRAME_TRIPPING_ROBOT.SetActive(false);
                FRAME_TRIPPING_OG.SetActive(true);
                FRAME_TRIPPING_EXPLORER.SetActive(false);
                break;
            case 2:
                FRAME_TRIPPING_ROBOT.SetActive(false);
                FRAME_TRIPPING_OG.SetActive(false);
                FRAME_TRIPPING_EXPLORER.SetActive(true);
                break;
            default:
                FRAME_TRIPPING_ROBOT.SetActive(true);
                FRAME_TRIPPING_OG.SetActive(false);
                FRAME_TRIPPING_EXPLORER.SetActive(false);
                break;
        }
    }

    private void HandleGUIs(int OG, int EX)
    {
        if(BUY_BUTTON_OG != null)
        {
            if(OG == 1)
            {
                //OG is bought
                //remove price tag
                //substitute buy with use btn

                Destroy(PRICE_GUI_OG);
                Destroy(BUY_BUTTON_OG);
                USE_BUTTON_OG.SetActive(true);
            }
        }

        if (BUY_BUTTON_EXPLORER != null)
        {
            if (EX == 1)
            {
                //OG is bought
                //remove price tag
                //substitute buy with use btn

                Destroy(PRICE_GUI_EXPLORER);
                Destroy(BUY_BUTTON_EXPLORER);
                USE_BUTTON_EXPLORER.SetActive(true);
            }
        }
    }



}
