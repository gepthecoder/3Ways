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

    void Start()
    {
        InitializeMonetization();
    }

    void Update()
    {
        if (AdIsPlayingStopGameplay)
        {
            Time.timeScale = 0;
        }
        else { Time.timeScale = 1; }
    }

    private void InitializeMonetization()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(playstore_id, isTestAd);
    }

    public void DisplayInterstitialAD()
    {
        Advertisement.Show(video_ad);
    }

    public void DisplayRewardedAD()
    {
        AdIsPlayingStopGameplay = true;

        Advertisement.Show(rewardedVideo_ad);
    }

    //public void DisplayVideoAD() { Advertisement.Show(playstore_id); }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            Debug.LogWarning("You get the reward!!");
            AdIsPlayingStopGameplay = false;
            StartCoroutine(spinTheSlot());
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

}
