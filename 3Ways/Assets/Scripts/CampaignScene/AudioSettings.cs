using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Space(10)]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Space(10)]
    public Animator settingsAnime;

    private bool settingsOpen;

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
    }

    private void Start()
    {
        settingsOpen = false;

        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0);
    }

    private void OnDisable()
    {
        float musicVolume = 0;
        float sfxVolume = 0;

        audioMixer.GetFloat("musicVolume", out musicVolume);
        audioMixer.GetFloat("sfxVolume", out sfxVolume);

        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public void ShowHideSettings()
    {
        if (settingsOpen)
        {
            //close
            settingsAnime.SetTrigger("hideSettings");
            settingsOpen = false;
        }
        else
        {
            //open
            settingsAnime.SetTrigger("showSettings");
            settingsOpen = true;
        }
    }
}
