using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public Animator gameInfoAnime;

    public GameObject leftArrow;
    public GameObject rightArrow;

    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;

    private int iCurrentPage = 1;

    void Start() {

        ShowPage();
        HandleArrows();
    }



    public void ShowPage(int pageNum = 1)
    {
        switch (pageNum)
        {
            case 1:
                page1.SetActive(true);
                page2.SetActive(false);
                page3.SetActive(false);
                page4.SetActive(false);

                iCurrentPage = 1;
                break;
            case 2:
                page1.SetActive(false);
                page2.SetActive(true);
                page3.SetActive(false);
                page4.SetActive(false);

                iCurrentPage = 2;
                break;
            case 3:
                page1.SetActive(false);
                page2.SetActive(false);
                page3.SetActive(true);
                page4.SetActive(false);

                iCurrentPage = 3;
                break;
            case 4:
                page1.SetActive(false);
                page2.SetActive(false);
                page3.SetActive(false);
                page4.SetActive(true);

                iCurrentPage = 4;
                break;
            default:
                page1.SetActive(true);
                page2.SetActive(false);
                page3.SetActive(false);
                page4.SetActive(false);

                iCurrentPage = 1;
                break;
        }

        HandleArrows();
    }

    public void NextPage()
    {
        ShowPage(iCurrentPage + 1);
    }

    public void PreviousPage()
    {
        ShowPage(iCurrentPage - 1);
    }

    public void HandleArrows()
    {
        if(iCurrentPage == 1)
        {
            leftArrow.SetActive(false);
        }
        else { leftArrow.SetActive(true); }

        if (iCurrentPage == 4)
        {
            rightArrow.SetActive(false);
        }
        else { rightArrow.SetActive(true); }
    }


    public void ShowGameInfo()
    {
        gameInfoAnime.SetTrigger("showInfo");
    }

    public void HideGameInfo()
    {
        gameInfoAnime.SetTrigger("hideInfo");
    }


}
