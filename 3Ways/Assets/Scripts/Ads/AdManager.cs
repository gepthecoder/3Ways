using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public static bool AdIsPlayingStopGameplay;

    private string playstore_id = "3607588";
    private string video_ad = "video";
    private string rewardedVideo_ad = "rewardedVideo";

    public bool isTestAd = true;

    public Slot slotMachine;
    public PlayerXPbar xp;
    
    private bool extraXP;
    private bool slotSpin;

    void Start()
    {
        InitializeMonetization();
    }

    private void InitializeMonetization()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(playstore_id, isTestAd);
    }

    public void DisplayInterstitial()
    {
        if(Advertisement.IsReady())
            Advertisement.Show(video_ad);
    }

    public void DisplayRewardedExtraAD()
    {
        if (Advertisement.IsReady())
        {
            extraXP = true;
            Advertisement.Show(rewardedVideo_ad);
        }
    }

    public void DisplayRewardedAD()
    {
        if (Advertisement.IsReady())
        {
            AdIsPlayingStopGameplay = true;
            slotSpin = true;
            Advertisement.Show(rewardedVideo_ad);
        }
    }

    //public void DisplayVideoAD() { Advertisement.Show(playstore_id); }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            if (slotSpin)
            {
                // Reward the user for watching the ad to completion.
                Debug.LogWarning("You get the reward!!");
                AdIsPlayingStopGameplay = false;
                StartCoroutine(spinTheSlot());
                slotSpin = false;
            }

            if (extraXP)
            {
                Debug.LogWarning("You get the extra points!!");
                xp.EXTRA_XP();
                extraXP = false;
            }
          
            
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == playstore_id)
        {

        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    private IEnumerator spinTheSlot()
    {
        yield return new WaitForSeconds(1.5f);
        slotMachine.SPIN_SLOT();
    }

    private IEnumerator GetExtraXP()
    {
        yield return new WaitForSeconds(1f);

    }

}
