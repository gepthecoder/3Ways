using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameShop : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////////

    // CHARACTERS + CROWNS
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

    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////

    // DANCE MOVES
    private int iCurrentDanceMove;

    protected const int DANCE_MOVE_1 = 25000;
    protected const int DANCE_MOVE_2 = 50000;
    protected const int DANCE_MOVE_3 = 75000;
    protected const int DANCE_MOVE_4 = 100000;
    protected const int DANCE_MOVE_5 = 1000000;

    private int D1_BOUGHT = 0;
    private int D2_BOUGHT = 0;
    private int D3_BOUGHT = 0;

    private int D4_BOUGHT = 0;
    private int D5_BOUGHT = 0;


    public GameObject PRICE_GUI_D1;
    public GameObject USE_BUTTON_D1;
    public GameObject BUY_BUTTON_D1;
    
    public GameObject PRICE_GUI_D2;
    public GameObject USE_BUTTON_D2;
    public GameObject BUY_BUTTON_D2;

    public GameObject PRICE_GUI_D3;
    public GameObject USE_BUTTON_D3;
    public GameObject BUY_BUTTON_D3;

    public GameObject PRICE_GUI_D4;
    public GameObject USE_BUTTON_D4;
    public GameObject BUY_BUTTON_D4;

    public GameObject PRICE_GUI_D5;
    public GameObject USE_BUTTON_D5;
    public GameObject BUY_BUTTON_D5;


    public GameObject FRAME_TRIPPING_D0;
    public GameObject FRAME_TRIPPING_D1;
    public GameObject FRAME_TRIPPING_D2;
    public GameObject FRAME_TRIPPING_D3;
    public GameObject FRAME_TRIPPING_D4;
    public GameObject FRAME_TRIPPING_D5;

    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////

    // GUI HANDLER -> ALL CATEGORIES
    public GameObject NodeChar;

    public GameObject NodeDance;
    public GameObject NodeDancePageOne;
    public GameObject NodeDancePageTwo;

    public GameObject NodePowerUps;

    public void SetShopNode(int node)
    {
        switch (node)
        {
            case 0:
                NodeChar.SetActive(true);
                NodeDance.SetActive(false);
                NodePowerUps.SetActive(false);
                break;

            case 1:
                NodeChar.SetActive(false);
                NodeDance.SetActive(true);
                NodePowerUps.SetActive(false);
                break;

            case 2:
                NodeChar.SetActive(false);
                NodeDance.SetActive(false);
                NodePowerUps.SetActive(true);
                break;
        }
    }

    public void SecondDancePage()
    {
        NodeDancePageTwo.SetActive(true);
        NodeDancePageOne.SetActive(false);
    }
    public void FirstDancePage()
    {
        NodeDancePageTwo.SetActive(false);
        NodeDancePageOne.SetActive(true);
    }
    ///////////////////////////////////////////////////////////////////////////////////



    void Awake()
    {
        // CHARACTERS + CROWNS
        GetPrefsChar();
        GetPrefsCharBought();
        GetPrefsCrowns();
        //

        // DANCE MOVES
        GetPrefsDance();
        GetPrefsDancesBought();
        //
    }

    void Start()
    {
        // CHARACTERS + CROWNS
        SetTextCrowns();
        SetFrame(iCurrentCharacter);
        HandleGUIs(OG_BOUGHT, EX_BOUGHT);
        //

        // DANCE MOVES
        SetDanceFrame(iCurrentDanceMove);
        HandleDanceGUIs(D1_BOUGHT, D2_BOUGHT, D3_BOUGHT, D4_BOUGHT, D5_BOUGHT);
        //
    }

    ///////////////////////////////////////////////////////////////////////////////////

    // PREFS -> CHARACTER SHOP

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

    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////


    // PREFS -> DANCE MOVES SHOP

    private void GetPrefsDance()
    {
        if (PlayerPrefs.HasKey("iCurrentDanceMove"))
        {
            //we had a previous session
            iCurrentDanceMove = PlayerPrefs.GetInt("iCurrentDanceMove", 0);
        }
        else
        {
            SetPrefsDance();
        }
    }

    private void SetPrefsDance()
    {
        PlayerPrefs.SetInt("iCurrentDanceMove", iCurrentDanceMove);
    }


    private void GetPrefsDancesBought()
    {
        if (PlayerPrefs.HasKey("D1_BOUGHT") ||  PlayerPrefs.HasKey("D2_BOUGHT") ||
            PlayerPrefs.HasKey("D3_BOUGHT") ||  PlayerPrefs.HasKey("D4_BOUGHT") ||
                                                PlayerPrefs.HasKey("D5_BOUGHT"))
        {
            //we had a previous session
            D1_BOUGHT = PlayerPrefs.GetInt("D1_BOUGHT", 0);
            D2_BOUGHT = PlayerPrefs.GetInt("D2_BOUGHT", 0);
            D3_BOUGHT = PlayerPrefs.GetInt("D3_BOUGHT", 0);
            D4_BOUGHT = PlayerPrefs.GetInt("D4_BOUGHT", 0);
            D5_BOUGHT = PlayerPrefs.GetInt("D5_BOUGHT", 0);
        }
        else
        {
            SetPrefsDanceBought();
        }
    }

    private void SetPrefsDanceBought()
    {
        PlayerPrefs.SetInt("D1_BOUGHT", D1_BOUGHT);
        PlayerPrefs.SetInt("D2_BOUGHT", D2_BOUGHT);
        PlayerPrefs.SetInt("D3_BOUGHT", D3_BOUGHT);
        PlayerPrefs.SetInt("D4_BOUGHT", D4_BOUGHT);
        PlayerPrefs.SetInt("D5_BOUGHT", D5_BOUGHT);
    }

    //

    // BUTTON EVENTS

    public void BuyDanceMove1()
    {
        if (iCurrentCrowns >= DANCE_MOVE_1)
        {
            //buy item
            iCurrentCrowns -= DANCE_MOVE_1;
            SetPrefsCrowns();
            SetTextCrowns();

            D1_BOUGHT = 1;
            SetPrefsDanceBought();

            Destroy(PRICE_GUI_D1);
            Destroy(BUY_BUTTON_D1);
            USE_BUTTON_D1.SetActive(true);
        }
        else { Debug.Log("Declined buy! -> no resources"); }
    }

    public void BuyDanceMove2()
    {
        if (iCurrentCrowns >= DANCE_MOVE_2)
        {
            //buy item
            iCurrentCrowns -= DANCE_MOVE_2;
            SetPrefsCrowns();
            SetTextCrowns();

            D2_BOUGHT = 1;
            SetPrefsDanceBought();

            Destroy(PRICE_GUI_D2);
            Destroy(BUY_BUTTON_D2);
            USE_BUTTON_D2.SetActive(true);
        }
        else { Debug.Log("Declined buy! -> no resources"); }
    }

    public void BuyDanceMove3()
    {
        if (iCurrentCrowns >= DANCE_MOVE_3)
        {
            //buy item
            iCurrentCrowns -= DANCE_MOVE_3;
            SetPrefsCrowns();
            SetTextCrowns();

            D3_BOUGHT = 1;
            SetPrefsDanceBought();

            Destroy(PRICE_GUI_D3);
            Destroy(BUY_BUTTON_D3);
            USE_BUTTON_D3.SetActive(true);
        }
        else { Debug.Log("Declined buy! -> no resources"); }
    }

    public void BuyDanceMove4()
    {
        if (iCurrentCrowns >= DANCE_MOVE_4)
        {
            //buy item
            iCurrentCrowns -= DANCE_MOVE_4;
            SetPrefsCrowns();
            SetTextCrowns();

            D4_BOUGHT = 1;
            SetPrefsDanceBought();

            Destroy(PRICE_GUI_D4);
            Destroy(BUY_BUTTON_D4);
            USE_BUTTON_D4.SetActive(true);
        }
        else { Debug.Log("Declined buy! -> no resources"); }
    }

    public void BuyDanceMove5()
    {
        if (iCurrentCrowns >= DANCE_MOVE_5)
        {
            //buy item
            iCurrentCrowns -= DANCE_MOVE_5;
            SetPrefsCrowns();
            SetTextCrowns();

            D5_BOUGHT = 1;
            SetPrefsDanceBought();

            Destroy(PRICE_GUI_D5);
            Destroy(BUY_BUTTON_D5);
            USE_BUTTON_D5.SetActive(true);
        }
        else { Debug.Log("Declined buy! -> no resources"); }
    }
    
    public void UseDanceMove(int iChoosen)
    {
        iCurrentDanceMove = iChoosen;
        SetPrefsDance();
        SetDanceFrame(iChoosen);
    }

    //

    // HANDLERS

    private void SetDanceFrame(int iChoosen)
    {
        switch (iChoosen)
        {
            case 0:
                FRAME_TRIPPING_D0.SetActive(true);
                FRAME_TRIPPING_D1.SetActive(false);
                FRAME_TRIPPING_D2.SetActive(false);
                FRAME_TRIPPING_D3.SetActive(false);
                FRAME_TRIPPING_D4.SetActive(false);
                FRAME_TRIPPING_D5.SetActive(false);
                break;
            case 1:
                FRAME_TRIPPING_D0.SetActive(false);
                FRAME_TRIPPING_D1.SetActive(true);
                FRAME_TRIPPING_D2.SetActive(false);
                FRAME_TRIPPING_D3.SetActive(false);
                FRAME_TRIPPING_D4.SetActive(false);
                FRAME_TRIPPING_D5.SetActive(false);
                break;
            case 2:
                FRAME_TRIPPING_D0.SetActive(false);
                FRAME_TRIPPING_D1.SetActive(false);
                FRAME_TRIPPING_D2.SetActive(true);
                FRAME_TRIPPING_D3.SetActive(false);
                FRAME_TRIPPING_D4.SetActive(false);
                FRAME_TRIPPING_D5.SetActive(false);
                break;
            case 3:
                FRAME_TRIPPING_D0.SetActive(false);
                FRAME_TRIPPING_D1.SetActive(false);
                FRAME_TRIPPING_D2.SetActive(true);
                FRAME_TRIPPING_D3.SetActive(false);
                FRAME_TRIPPING_D4.SetActive(false);
                FRAME_TRIPPING_D5.SetActive(false);
                break;
            case 4:
                FRAME_TRIPPING_D0.SetActive(false);
                FRAME_TRIPPING_D1.SetActive(false);
                FRAME_TRIPPING_D2.SetActive(false);
                FRAME_TRIPPING_D3.SetActive(true);
                FRAME_TRIPPING_D4.SetActive(false);
                FRAME_TRIPPING_D5.SetActive(false);
                break;
            case 5:
                FRAME_TRIPPING_D0.SetActive(false);
                FRAME_TRIPPING_D1.SetActive(false);
                FRAME_TRIPPING_D2.SetActive(false);
                FRAME_TRIPPING_D3.SetActive(false);
                FRAME_TRIPPING_D4.SetActive(true);
                FRAME_TRIPPING_D5.SetActive(false);
                break;
            default:
                FRAME_TRIPPING_D0.SetActive(false);
                FRAME_TRIPPING_D1.SetActive(false);
                FRAME_TRIPPING_D2.SetActive(false);
                FRAME_TRIPPING_D3.SetActive(false);
                FRAME_TRIPPING_D4.SetActive(false);
                FRAME_TRIPPING_D5.SetActive(true);
                break;
        }
    }

    private void HandleDanceGUIs(int D1, int D2, int D3, int D4, int D5)
    {
        if (BUY_BUTTON_D1 != null)
        {
            if (D1 == 1)
            {
                //remove price tag
                //substitute buy with use btn

                Destroy(PRICE_GUI_D1);
                Destroy(BUY_BUTTON_D1);
                USE_BUTTON_D1.SetActive(true);
            }
        }

        if (BUY_BUTTON_D2 != null)
        {
            if (D2 == 1)
            {
                //remove price tag
                //substitute buy with use btn

                Destroy(PRICE_GUI_D2);
                Destroy(BUY_BUTTON_D2);
                USE_BUTTON_D2.SetActive(true);
            }
        }
        if (BUY_BUTTON_D3 != null)
        {
            if (D3 == 1)
            {
                //remove price tag
                //substitute buy with use btn

                Destroy(PRICE_GUI_D3);
                Destroy(BUY_BUTTON_D3);
                USE_BUTTON_D3.SetActive(true);
            }
        }
        if (BUY_BUTTON_D4 != null)
        {
            if (D4 == 1)
            {
                //remove price tag
                //substitute buy with use btn

                Destroy(PRICE_GUI_D4);
                Destroy(BUY_BUTTON_D4);
                USE_BUTTON_D4.SetActive(true);
            }
        }
        if (BUY_BUTTON_D5 != null)
        {
            if (D5 == 1)
            {
                //remove price tag
                //substitute buy with use btn

                Destroy(PRICE_GUI_D5);
                Destroy(BUY_BUTTON_D5);
                USE_BUTTON_D5.SetActive(true);
            }
        }
    }
}
