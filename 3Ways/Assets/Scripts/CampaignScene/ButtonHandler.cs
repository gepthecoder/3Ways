using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public Animator fader;
    
    public void SetDifficulty(int iDifficulty)
    {
        SetPrefsDifficulty(iDifficulty);
    }

    private void SetPrefsDifficulty(int iDiff)
    {
        PlayerPrefs.SetInt("currentDifficulty", iDiff);
        PlayerPrefs.Save();
    }

    public void OpenGameRoom(int difficulty)
    {
        SetDifficulty(difficulty);
        fader.SetTrigger("fadeOut");
    }
}
