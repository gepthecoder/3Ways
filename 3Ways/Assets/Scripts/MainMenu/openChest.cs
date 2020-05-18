using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class openChest : MonoBehaviour
{
    public static bool Open = false;
    public Animator chestAnime;
    
    public Transform target;
    public Transform cam;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0.08f, -0.5f);

    public GameObject coinDrop;
    public Animator getCoinRewardAnime;


    void Start()
    {
        Open = false;
        coinDrop.SetActive(false);

    }

    void Update()
    {
        if (Open)
        {
            chestAnime.enabled = true;

            chestAnime.SetTrigger("OpenChest");
            Open = false;

            smoothSpeed = 0.0250f;
            offset = new Vector3(0, 0.36f, -0.83f);

            StartCoroutine(GetRewards());

            Vector3 desiredPos = target.position + offset;
            Vector3 smoothPos = Vector3.Lerp(cam.transform.position, desiredPos, smoothSpeed);
            cam.transform.position = smoothPos;
            Debug.Log("offset OPENED: " + offset);

            cam.transform.LookAt(target);
        }
        else
        {
            Vector3 desiredPos = target.position + offset;
            Vector3 smoothPos = Vector3.Lerp(cam.transform.position, desiredPos, smoothSpeed);
            cam.transform.position = smoothPos;
            cam.transform.LookAt(target);
        }
    }

    IEnumerator GetRewards()
    {
        yield return new WaitForSeconds(1.4f);

        getCoinRewardAnime.SetTrigger("getGiftReward");
        coinDrop.SetActive(true);
        yield return new WaitForSeconds(3f);
        coinDrop.SetActive(false);
        chestAnime.SetTrigger("CloseChest");
        offset = new Vector3(0, 0.08f, -0.5f);

    }






}
