using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Space(10)]
    [Header("SLOT")]
    [Space(10)]
    public Animator getCrownRewardAnime;
    [Space(5)]
    public Text crownAmountText;
    [Space(5)]
    public GameObject JackpotVFX;
    [Space(5)]
    public GameObject NormalVFX;
    [Space(5)]
    public bool bSpinSlot;

    private int iCurrentReward;
    public static int iCurrentRewardAmount;

    private Animation slotRollAnime;
    protected bool bJackpot;

    [Space(10)]
    [Header("CROWNS")]
    [Space(10)]
    public Text CrownsAmountTxt;

    

    [Space(10)]
    [Header("Camera Settings")]
    [Space(10)]
    public Transform camLookAt;
    [Space(5)]
    public Transform camTarget;
    [Space(5)]
    [Range(0.050f, 1f)]
    public float smoothSpeed = 0.125f;
    [Space(5)]
    public Vector3 camOffset;
    [Space(5)]
    public Transform cam;

    private Vector3 defaultCamPos;

    public enum REWARDS {
        JACKPOT = 0,             // 10000crowns
            HEARTS,             // 750crowns
                ONES,          // 350crowns
                   QUESTIONS, // 100crowns 
                        NONE,
    }

    void Start()
    {
        slotRollAnime = GetComponent<Animation>();

        bSpinSlot = false;
        bJackpot = false;

        iCurrentReward = (int)REWARDS.NONE;
        iCurrentRewardAmount = 0;

        defaultCamPos = cam.position;
    }

    void Update()
    {
        if (bSpinSlot)
        {
            PresentWIN();
        }
        else
        {

            smoothSpeed = 0.0125f;
            camOffset = new Vector3(20, 1, 0.5f);
            Vector3 desiredPos = camTarget.position + camOffset;
            Vector3 smoothPos = Vector3.Lerp(cam.position, desiredPos, smoothSpeed);
            cam.position = smoothPos;

            transform.LookAt(camLookAt);
        }
    }

    protected void SET_SLOT_OUTCOME()
    {
        int iRand = Random.Range(0, slotRollAnime.GetClipCount());
        switch (iRand)
        {
            // ONES
            case 0:
                iCurrentReward = (int)REWARDS.ONES;
                slotRollAnime.clip = slotRollAnime.GetClip("Slotmachine_roll_01");
                break;
            // ??
            case 1:
                iCurrentReward = (int)REWARDS.QUESTIONS;
                slotRollAnime.clip = slotRollAnime.GetClip("Slotmachine_roll_02");
                break;
            // STARS
            case 2:
                iCurrentReward = (int)REWARDS.JACKPOT;
                slotRollAnime.clip = slotRollAnime.GetClip("Slotmachine_roll_03");
                bJackpot = true;
                break;
            // HEARTS
            case 3:
                iCurrentReward = (int)REWARDS.HEARTS;
                slotRollAnime.clip = slotRollAnime.GetClip("Slotmachine_roll_04");
                break;

            default:
                iCurrentReward = (int)REWARDS.ONES;
                slotRollAnime.clip = slotRollAnime.GetClip("Slotmachine_roll_01");
                break;
        }
    }

    private void PresentWIN()
    {
        camOffset = new Vector3(10, 1, 0.5f);

        Vector3 desiredPos = camTarget.position + camOffset;
        Vector3 smoothPos = Vector3.Lerp(cam.position, desiredPos, smoothSpeed);
        cam.position = smoothPos;

        transform.LookAt(camLookAt);
    }

    private void SetReward()
    {
        switch (iCurrentReward)
        {
            case (int)REWARDS.JACKPOT:
                iCurrentRewardAmount = 10000;
                break;
            case (int)REWARDS.HEARTS:
                iCurrentRewardAmount = 750;
                break;
            case (int)REWARDS.ONES:
                iCurrentRewardAmount = 350;
                break;
            case (int)REWARDS.QUESTIONS:
                iCurrentRewardAmount = 100;
                break;
           default:
                iCurrentRewardAmount = 100;
                break;
        }

        crownAmountText.text = "+" + iCurrentRewardAmount.ToString();
    }

    private IEnumerator GetReward()
    {
        yield return new WaitForSeconds(1.3f);
        //reward vfx
        bool bJP = iCurrentReward == (int)REWARDS.JACKPOT;
        if (bJP)
        {
            JackpotVFX.SetActive(true);
        }
        else { NormalVFX.SetActive(true); }
        //animate get crowns text
        getCrownRewardAnime.SetTrigger("getSpinReward");
        Debug.Log("How many times??");
        //give player crowns
        StartCoroutine(crownsUpdater(bJP));
        yield break;
    }

    private IEnumerator crownsUpdater(bool JP)
    {
        int CurrentCrowns = CoinManager.CROWNS;
        int desiredAmount = CoinManager.CROWNS + iCurrentRewardAmount;

        while (CurrentCrowns != desiredAmount)
        {

            if (CurrentCrowns < desiredAmount)
            {
                if (JP)
                {
                    CurrentCrowns += 5;
                }
                else { CurrentCrowns++; }
                CrownsAmountTxt.text = CurrentCrowns.ToString();
            }
          
            yield return new WaitForSeconds(0.01f);
            
        }
       
    }

    public void SPIN_SLOT()
    {
        SET_SLOT_OUTCOME();
        // MOVE CAMERA POSITION & COIN EXPLOSION || PRESENT JACKPOT
        bSpinSlot = true;

        // SPIN
        slotRollAnime.Play();
        SetReward();
        StartCoroutine(GetReward()); 
    }
}
