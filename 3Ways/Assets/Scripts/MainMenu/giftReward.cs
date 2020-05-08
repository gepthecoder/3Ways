using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class giftReward : MonoBehaviour
{
    public int numOfCrowns = 0;

    public Button chestButton;
    public static ulong lastChestOpen = 0;

    public float msToWait = 5000.0f;

    public Text chestTimer;
    public Text claimGiftTxt;

    public Animator frameAnime;

    void Awake()
    {
        if (PlayerPrefs.HasKey("CROWNS"))
        {
            //we had a previous session
            numOfCrowns = PlayerPrefs.GetInt("CROWNS", 0);
        }
        else
        {
            SaveCrowns();
        }
    }

    public void SaveCrowns()
    {
        PlayerPrefs.SetInt("CROWNS", numOfCrowns);
    }

    void Start()
    {
        lastChestOpen = ulong.Parse(PlayerPrefs.GetString("LastChestOpen", "0"));

        if (!IsChestReady())
        {
            chestButton.interactable = false;
        }
    }

    void Update()
    {
        if (!chestButton.IsInteractable())
        {
            if (IsChestReady())
            {
                chestButton.interactable = true;
                return;
            }

            // Set The Timer
            ulong diff = ((ulong)DateTime.Now.Ticks - lastChestOpen);
            ulong m = diff / TimeSpan.TicksPerMillisecond;

            float secondsLeft = (float)(msToWait - m) / 1000.0f;

            string t = "";
            // Hours
            t += (((int)secondsLeft / 3600).ToString("00") + ":");
            secondsLeft -= ((int)secondsLeft / 3600) * 3600;
            // Minutes
            t += (((int)secondsLeft / 60).ToString("00") + ":");
            secondsLeft -= ((int)secondsLeft / 3600) * 3600;
            // Seconds
            t += (secondsLeft % 60).ToString("00");
            chestTimer.text = t;

            claimGiftTxt.text = "FREE CHEST REWARD!";
            frameAnime.SetBool("trip", false);
        }
    }

   public void OpenChest()
    {
        chestButton.interactable = false;
        lastChestOpen = (ulong)DateTime.Now.Ticks;
        PlayerPrefs.SetString("LastChestOpen", DateTime.Now.Ticks.ToString());
        openChest.Open = true;
        numOfCrowns += 1000;
        SaveCrowns();
    }

    private bool IsChestReady()
    {
        ulong diff = ((ulong)DateTime.Now.Ticks - lastChestOpen);
        ulong m = diff / TimeSpan.TicksPerMillisecond;

        float secondsLeft = (float)(msToWait - m) / 1000.0f;

        if (secondsLeft < 0)
        {
            chestTimer.text = "00:00:00";
            claimGiftTxt.text = "CLAIM GIFT!";
            frameAnime.SetBool("trip", true);
            return true;

        }

        return false;
        
    }
}
