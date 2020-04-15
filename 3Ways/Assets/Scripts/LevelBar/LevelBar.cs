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

   void ReplaceLevelImage(int lvl)
    {
        Sprite lvlSprite = null;
        
        switch (lvl)
        {
            case 1:
                lvlSprite = lvlUIs[0].GetComponent<Image>().sprite;
                break;
            case 2:
                lvlSprite = lvlUIs[1].GetComponent<Image>().sprite;
                break;
            case 3:
                lvlSprite = lvlUIs[2].GetComponent<Image>().sprite;
                break;
            case 4:
                lvlSprite = lvlUIs[3].GetComponent<Image>().sprite;
                break;
            case 5:
                lvlSprite = lvlUIs[4].GetComponent<Image>().sprite;
                break;
            case 6:
                lvlSprite = lvlUIs[5].GetComponent<Image>().sprite;
                break;
            case 7:
                lvlSprite = lvlUIs[6].GetComponent<Image>().sprite;
                break;
            case 8:
                lvlSprite = lvlUIs[7].GetComponent<Image>().sprite;
                break;
            case 9:
                lvlSprite = lvlUIs[8].GetComponent<Image>().sprite;
                break;
            case 10:
                lvlSprite = lvlUIs[9].GetComponent<Image>().sprite;
                break;
            default:
                Debug.LogWarning("Dude the parameter lvl is out of range.. Check LevelBar.cs line 54");
                Debug.Log("Choosing first lvl image..");
                lvlSprite = lvlUIs[0].GetComponent<Image>().sprite;
                break;
        }

        lvlSprite = levelDoneSprite;
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
            ReplaceLevelImage(1);
        }
        else if(barValue > 111.25 && barValue < 112)
        {
            ReplaceLevelImage(2);
        }
        else if (barValue > 167.25 && barValue < 168)
        {
            ReplaceLevelImage(3);
        }
        else if (barValue > 223.25 && barValue < 224)
        {
            ReplaceLevelImage(4);
        }
        else if (barValue > 279.25 && barValue < 280)
        {
            ReplaceLevelImage(5);
        }
        else if (barValue > 335.25 && barValue < 336)
        {
            ReplaceLevelImage(6);
        }
        else if (barValue > 391.25 && barValue < 392)
        {
            ReplaceLevelImage(7);
        }
        else if (barValue > 443.25 && barValue < 444)
        {
            ReplaceLevelImage(8);
        }
        else if (barValue > 499.25 && barValue < 500)
        {
            ReplaceLevelImage(9);
        }
        else if (barValue > 55.25 && barValue < 556)
        {
            ReplaceLevelImage(10);
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
