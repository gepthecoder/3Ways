using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinStars : MonoBehaviour
{
    public static int iStars;
    private Animator anime;

    public Animator youEarnedAnime;

    void Start()
    {
        iStars = 0;
        anime = GetComponent<Animator>();
    }
    
    public int NumberOfStarsToShow()
    {
        bool medianFor3stars = PlayrXP.iStars >= GetNumberOfSections((int)CalculationManager.DIFFICULTIES.EASY);
        bool medianFor2stars = (PlayrXP.iStars < GetNumberOfSections((int)CalculationManager.DIFFICULTIES.EASY) &&
                                PlayrXP.iStars >= GetNumberOfSections((int)CalculationManager.DIFFICULTIES.EASY) - 3);

        if (medianFor3stars)
        {
            iStars = 3;
        }
        else if (medianFor2stars)
        {
            iStars = 2;
        }
        else
        {
            iStars = 1;
        }

        return iStars;
    }

    public void PlayAnime(int numOfStars)
    {
        switch (numOfStars)
        {
            case 1:
                anime.SetTrigger("star1");
                break;
            case 2:
                anime.SetTrigger("star2");
                break;
            case 3:
                anime.SetTrigger("star3");
                break;
            default:
                anime.SetTrigger("star1");
                break;

        }
    }


    public int GetNumberOfSections(int difficulty)
    {
        int numOfSections = 0;

        switch (difficulty)
        {
            case (int)CalculationManager.DIFFICULTIES.EASY:
                numOfSections = LevelManager.iEASY_SECTIONS;
                break;
            case (int)CalculationManager.DIFFICULTIES.MEDIUM:
                numOfSections = LevelManager.iMEDIUM_SECTIONS;
                break;
            case (int)CalculationManager.DIFFICULTIES.HARD:
                numOfSections = LevelManager.iHARD_SECTIONS;
                break;
            case (int)CalculationManager.DIFFICULTIES.GENIOUS:
                numOfSections = LevelManager.iGENIOUS_SECTIONS;
                break;
            default:
                numOfSections = LevelManager.iEASY_SECTIONS;
                break;
        }

        return numOfSections;
    }

    public void ShowXPearned()
    {
        if (youEarnedAnime != null)
        {
            youEarnedAnime.SetTrigger("youEarned");
            StartCoroutine(XPbar());

        }
    }

    IEnumerator XPbar()
    {
        yield return new WaitForSeconds(1f);
        PlayerXPbar.ShowGainedXP = true;
    }
}
