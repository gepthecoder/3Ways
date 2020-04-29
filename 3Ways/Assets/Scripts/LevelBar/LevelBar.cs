using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    private Slider      sLevelBar;

    public  Transform   player1;

    private float       fSectionDistanceZ   = 56f;
    private int         iNumOfSection       = 10;

    private Vector3     StartPos;
    private Vector3     EndPos;

    private float       totalDistance;
    private float       playerDistance;
    private float       playerProgress;

    private float       levelBarWidth = 560f;

    public  Sprite          levelDoneSprite;
    private GameObject[]    lvlUIs;

    
    void OnEnable()
    {
        GetTotalDistance();
    }

    void Awake()
    {
        sLevelBar = GetComponent<Slider>();
        lvlUIs = GameObject.FindGameObjectsWithTag("levelImg");
    }

    void Start()
    {
        GetStartEndPoint();
    }

    void Update()
    {
        playerDistance = player1.position.z - StartPos.z;
        playerProgress = playerDistance / totalDistance * 100;

        sLevelBar.value = playerProgress / 100 * levelBarWidth;

        HandleUI_Levels(sLevelBar.value);
    }

    private IEnumerator ReplaceSprites(int lvl)
    {
        playAnime(lvl);
        yield return new WaitForSeconds(.5f);
        Image img = lvlUIs[lvl].GetComponent<Image>();
        img.sprite = levelDoneSprite;
        if(lvl != 10)
        {
            DisableChild(lvl);
        }
    }

    private void playAnime(int lvl)
    {
        Animator anime = lvlUIs[lvl].GetComponent<Animator>();
        anime.SetTrigger("checkpoint");
    }

    private void DisableChild(int lvl)
    {
        Text txt = lvlUIs[lvl].GetComponentInChildren<Text>();
        txt.enabled = false;
    }

    private void HandleUI_Levels(float barValue)
    {
        /*
         lvl 1  -> 56
         lvl 2  -> 112
         lvl 3  -> 168
         lvl 4  -> 224
         lvl 5  -> 280
         lvl 6  -> 336
         lvl 7  -> 392
         lvl 8  -> 444
         lvl 9  -> 500
         lvl 10 -> 556
         */

        if(barValue > 55.25 && barValue < 56)
        {
            StartCoroutine(ReplaceSprites(0));
        }
        else if(barValue > 111.25 && barValue < 112)
        {
            StartCoroutine(ReplaceSprites(1));
        }
        else if (barValue > 167.25 && barValue < 168)
        {
            StartCoroutine(ReplaceSprites(2));
        }
        else if (barValue > 223.25 && barValue < 224)
        {
            StartCoroutine(ReplaceSprites(3));
        }
        else if (barValue > 279.25 && barValue < 280)
        {
            StartCoroutine(ReplaceSprites(4));
        }
        else if (barValue > 335.25 && barValue < 336)
        {
            StartCoroutine(ReplaceSprites(5));
        }
        else if (barValue > 391.25 && barValue < 392)
        {
            StartCoroutine(ReplaceSprites(6));
        }
        else if (barValue > 443.25 && barValue < 444)
        {
            StartCoroutine(ReplaceSprites(7));
        }
        else if (barValue > 499.25 && barValue < 500)
        {
            StartCoroutine(ReplaceSprites(8));
        }
        else if (barValue > 555.25 && barValue < 556)
        {
            StartCoroutine(ReplaceSprites(9));
        }
    }

    private void GetTotalDistance()
    {
        totalDistance = fSectionDistanceZ * iNumOfSection; // 560f
    }

    private void GetStartEndPoint()
    {
        StartPos = new Vector3(player1.position.x, player1.position.y, player1.position.z - 3.6f);
        EndPos = new Vector3(player1.position.x, player1.position.y, player1.position.z + totalDistance);
    }


}
