using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class chooseAvatar : MonoBehaviour
{

    public Sprite[] AVATARS_SPRITES;

    public int iCurrentAvatar = 5;

    public Outline[] AVATARS_OUTLINES;

    public Image PlayerAvatarImage;

    void Awake()
    {
        if (PlayerPrefs.HasKey("iCurrentAvatar"))
        {
            // we had a previous session
            iCurrentAvatar = PlayerPrefs.GetInt("iCurrentAvatar", 5);
        }
        else
        {
            SaveCurrentAvatar();
        }
    }

    void Start()
    {
        SetOutlineAlphaToVisible(iCurrentAvatar);
        SetPlayerAvatarImage(iCurrentAvatar);
    }

    private void SaveCurrentAvatar()
    {
        PlayerPrefs.SetInt("iCurrentAvatar", iCurrentAvatar);
        PlayerPrefs.Save();
    }

    public void SetAvatar(int choosenAvatar)
    {
        SetOutlineAlphaToZero(iCurrentAvatar);
        iCurrentAvatar = choosenAvatar;
        SaveCurrentAvatar();
        SetOutlineAlphaToVisible(iCurrentAvatar);
        SetPlayerAvatarImage(iCurrentAvatar);
    }

    private void SetOutlineAlphaToZero(int iAvatar)
    {
        AVATARS_OUTLINES[iAvatar].effectColor = new Color(AVATARS_OUTLINES[iAvatar].effectColor.r, AVATARS_OUTLINES[iAvatar].effectColor.g, AVATARS_OUTLINES[iAvatar].effectColor.b, 0);
    }

    private void SetOutlineAlphaToVisible(int iAvatar)
    {
        AVATARS_OUTLINES[iAvatar].effectColor = new Color(AVATARS_OUTLINES[iAvatar].effectColor.r, AVATARS_OUTLINES[iAvatar].effectColor.g, AVATARS_OUTLINES[iAvatar].effectColor.b, 128);
    }

    public void SetPlayerAvatarImage(int iAvatar)
    {
        PlayerAvatarImage.sprite = AVATARS_SPRITES[iAvatar];
    }
}
