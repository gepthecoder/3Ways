using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class inGameAudio : MonoBehaviour
{
    [Header("AUDIO MIXER")]
    [Space(10)]
    public AudioMixer audioMixer;
    [Space(10)]
    public Image IMG_SOUND;
    [Space(5)]
    public Sprite soundON_sprite;
    [Space(5)]
    public Sprite soundOFF_sprite;
    [Space(3)]
    [Header("****************************************")]

    [Space(10)]
    [Header("IN-GAME AUDIO SETTINGS")]
    [Space(10)]
    public Image IMG_MUSIC;
    [Space(5)]
    public Sprite musicON_sprite;
    [Space(5)]
    public Sprite musicOFF_sprite;

    private int soundON;
    private int musicON;
    
    public enum SoundSetting { OFF=0,ON, }

    private float currentMusicVolume;
    private float currentSoundVolume;
    
    void Awake()
    {
        if(PlayerPrefs.HasKey("soundON") || PlayerPrefs.HasKey("musicON"))
        {
            //we had a previou session
            GetSoundPrefs();
        }
        else
        {
            SaveSoundPrefs();
        }
    }

    void Start()
    {
        GetCurrentVolumes();
    }
    
    /// <summary>
    ///                 FUNCTIONS
    /// </summary>

    private void GetCurrentVolumes()
    {
        currentMusicVolume = PlayerPrefs.GetFloat("musicVolume", 0);
        currentSoundVolume = PlayerPrefs.GetFloat("sxfVolume", 0);
    }

    private void GetSoundPrefs()
    {
        soundON = PlayerPrefs.GetInt("soundON", 1);
        musicON = PlayerPrefs.GetInt("musicON", 1);
    }

    private void SaveSoundPrefs()
    {
        PlayerPrefs.SetInt("soundON", soundON);
        PlayerPrefs.SetInt("musicON", musicON);
        PlayerPrefs.Save();
    }

    public void SET_MUSICFX()
    {
        GetSoundPrefs();

        if (musicON == (int)SoundSetting.ON)
        {
            musicON = (int)SoundSetting.OFF;
            SaveSoundPrefs();

            SET_MUSIC_SPRITE(true);

            SET_MUSIC_VOLUME((int)SoundSetting.OFF);
        }
        else
        {
            musicON = (int)SoundSetting.ON;
            SaveSoundPrefs();

            SET_MUSIC_SPRITE(false);

            SET_MUSIC_VOLUME((int)SoundSetting.ON);
        }
    }

    public void SET_SOUNDFX()
    {
        GetSoundPrefs();

        if (soundON == (int)SoundSetting.ON)
        {
            soundON = (int)SoundSetting.OFF;
            SaveSoundPrefs();

            SET_SOUND_SPRITE(true);

            SET_SFX_VOLUME((int)SoundSetting.OFF);
        }
        else
        {
            soundON = (int)SoundSetting.ON;
            SaveSoundPrefs();

            SET_SOUND_SPRITE(false);

            SET_SFX_VOLUME((int)SoundSetting.ON);
        }
    }

    private void SET_SOUND_SPRITE(bool on)
    {
        if (on)
        {
            IMG_SOUND.sprite = soundOFF_sprite;
        }
        else { IMG_SOUND.sprite = soundON_sprite; }
    }


    private void SET_MUSIC_SPRITE(bool on)
    {
        if (on)
        {
            IMG_MUSIC.sprite = musicOFF_sprite;
        }
        else { IMG_MUSIC.sprite = musicON_sprite; }
    }

    private void SET_SFX_VOLUME(int vol)
    {
        if (vol == (int)SoundSetting.ON)
        {
            PlayerPrefs.SetFloat("sfxVolume", 0);
        }
        else
        {
            PlayerPrefs.SetFloat("sfxVolume", -80);
        }

    }

    private void SET_MUSIC_VOLUME(int vol)
    {
        if (vol == (int)SoundSetting.ON)
        {
            PlayerPrefs.SetFloat("musicVolume", 0);
        }
        else
        {
            PlayerPrefs.SetFloat("musicVolume", -80);
        }

    }
}
