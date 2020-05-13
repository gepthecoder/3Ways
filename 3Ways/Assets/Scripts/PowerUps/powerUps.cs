using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class powerUps : MonoBehaviour
{
    private bool effectFreezeActivated;
    private float effectFreezeTime = 0;
    protected float effectDuration = 5f;

    private int numOfFreezes;
    private int numOfBooks;
    private int numOfPergaments;

    [Space(10)]
    [Header("Text Amounts")]
    [Space(10)]
    public Text freezeAmount;
    [Space(5)]
    public Text booksAmount;
    [Space(5)]
    public Text pergamentsAmount;
    
    [Space(10)]
    [Header("Power Up Buttons")]
    [Space(10)]
    public Button freezeBtn;
    [Space(5)]
    public Button booksBtn;
    [Space(5)]
    public Button pergamentBtn;

    [Space(10)]
    [Header("Freeze Effect")]
    [Space(10)]
    public Image freezeEffectImage;
    [Space(10)]
    public Image freezeEffectImageLeft;
    [Space(5)]
    public Image freezeEffectImageRight;

    [Space(10)]
    [Header("Book Of Knowledge Effect")]
    [Space(10)]
    public PlayerControl doorFrames;
    [Space(5)]
    public CalculationManager calculations;
    [Space(5)]
    public Material bookOfKnowlegeFrameMaterial;
    [Space(3)]
    public Material defaultFrameMaterial;

    [Space(10)]
    [Header("Pergament Of Wisdom Effect")]
    [Space(10)]
    public Material pergamentEffectFrameMaterial;

    void Awake()
    {
        GetPoWerUpValues();
    }

    void Start()
    {
        SetPowerUpsText();

        effectFreezeActivated = false;

        freezeEffectImageLeft.color = new Color(freezeEffectImageLeft.color.r, freezeEffectImageLeft.color.g, freezeEffectImageLeft.color.b, 0);
        freezeEffectImageRight.color = new Color(freezeEffectImageRight.color.r, freezeEffectImageRight.color.g, freezeEffectImageRight.color.b, 0);
        HandlePowerUpButtons();

    }

    void Update()
    {
        if (effectFreezeActivated)
        {
            //start timer
            effectFreezeTime += Time.deltaTime;

            if(effectFreezeTime >= effectDuration)
            {
                Debug.Log("StopEffect-Freeze!!");
                effectFreezeActivated = false;
                GameTimer.timeHasStarted = true;
                freezeBtn.interactable = true;

                freezeEffectImage.color = new Color(freezeEffectImage.color.r, freezeEffectImage.color.g, freezeEffectImage.color.b, 0);
                freezeEffectImageLeft.color = new Color(freezeEffectImageLeft.color.r, freezeEffectImageLeft.color.g, freezeEffectImageLeft.color.b, 0);
                freezeEffectImageRight.color = new Color(freezeEffectImageRight.color.r, freezeEffectImageRight.color.g, freezeEffectImageRight.color.b, 0);

                effectFreezeTime = 0;
            }
        }
    }

    private void GetPoWerUpValues()
    {
        numOfFreezes = PlayerPrefs.GetInt("numOfFreezes", 0);
        numOfBooks = PlayerPrefs.GetInt("numOfBooks", 0);
        numOfPergaments = PlayerPrefs.GetInt("numOfPergaments", 0);
    }

    private void SetPrefsPowerUps()
    {
        PlayerPrefs.SetInt("numOfFreezes", numOfFreezes);
        PlayerPrefs.SetInt("numOfBooks", numOfBooks);
        PlayerPrefs.SetInt("numOfPergaments", numOfPergaments);
    }

    private void SetPowerUpsText()
    {
        freezeAmount.text = numOfFreezes.ToString();
        booksAmount.text = numOfBooks.ToString();
        pergamentsAmount.text = numOfPergaments.ToString();
    }


    // TO:DO -> Set Timer for power up buttons interactability
    public void UsePowerUpFreeze()
    {
        if(numOfFreezes > 0)
        {
            numOfFreezes--;
            SetPrefsPowerUps();
            SetPowerUpsText();
            //make time freeze effect (duration 5sec)
            Debug.Log("Freeze effect started!!");
            effectFreezeActivated = true;
            GameTimer.timeHasStarted = false;
            freezeBtn.interactable = false;

            freezeEffectImageLeft.color = new Color(freezeEffectImageLeft.color.r, freezeEffectImageLeft.color.g, freezeEffectImageLeft.color.b, Mathf.Lerp(0, 255, 5f));
            freezeEffectImageRight.color = new Color(freezeEffectImageRight.color.r, freezeEffectImageRight.color.g, freezeEffectImageRight.color.b, Mathf.Lerp(0, 255, 5f));

            freezeEffectImage.color = new Color(freezeEffectImage.color.r, freezeEffectImage.color.g, freezeEffectImage.color.b, Mathf.Lerp(0, 164, 5f));
            HandlePowerUpButtons();
        }
        else { Debug.Log("No power ups left!!"); }
    }

    private void SetDoorFramesEffect(int iCorrectDoor)
    {
        switch (iCorrectDoor)
        {
            case (int)ChooseDoor.Doors.DOOR0:
                //color correct door -> 0 & a random door 1 or 2
                doorFrames.door0_frame.material = bookOfKnowlegeFrameMaterial;
                int iRand = Random.Range(0, 1);
                if (iRand == 0)
                {
                    doorFrames.door1_frame.material = bookOfKnowlegeFrameMaterial;
                    doorFrames.door2_frame.material = defaultFrameMaterial;
                }
                else
                {
                    doorFrames.door1_frame.material = defaultFrameMaterial;
                    doorFrames.door2_frame.material = bookOfKnowlegeFrameMaterial;
                }

                break;

            case (int)ChooseDoor.Doors.DOOR1:
                //color correct door -> 1 & a random door 0 or 2
                doorFrames.door1_frame.material = bookOfKnowlegeFrameMaterial;
                int iRando = Random.Range(0, 1);
                if (iRando == 0)
                {
                    doorFrames.door0_frame.material = bookOfKnowlegeFrameMaterial;
                    doorFrames.door2_frame.material = defaultFrameMaterial;
                }
                else
                {
                    doorFrames.door0_frame.material = defaultFrameMaterial;
                    doorFrames.door2_frame.material = bookOfKnowlegeFrameMaterial;
                }

                break;

            case (int)ChooseDoor.Doors.DOOR2:
                //color correct door -> 2 & a random door 0 or 1
                doorFrames.door2_frame.material = bookOfKnowlegeFrameMaterial;
                int iRandom = Random.Range(0, 1);
                if (iRandom == 1)
                {
                    doorFrames.door0_frame.material = bookOfKnowlegeFrameMaterial;
                    doorFrames.door1_frame.material = defaultFrameMaterial;
                }
                else
                {
                    doorFrames.door0_frame.material = defaultFrameMaterial;
                    doorFrames.door1_frame.material = bookOfKnowlegeFrameMaterial;
                }

                break;
        }
    }

    public void UsePowerUpBook()
    {
        if (numOfBooks > 0)
        {
            numOfBooks--;
            SetPrefsPowerUps();
            SetPowerUpsText();

            //make 50 / 50 effect on doors
            int iCorrect = calculations.currentCorrectDoor;
            SetDoorFramesEffect(iCorrect);

            HandlePowerUpButtons();

        }
        else { Debug.Log("No power ups left!!"); }

    }

    private void SetPergamentFrameEffect(int correctDoor)
    {
        switch (correctDoor)
        {
            case (int)ChooseDoor.Doors.DOOR0:
                doorFrames.door0_frame.material = pergamentEffectFrameMaterial;
                break;

            case (int)ChooseDoor.Doors.DOOR1:
                doorFrames.door1_frame.material = pergamentEffectFrameMaterial;
                break;

            case (int)ChooseDoor.Doors.DOOR2:
                doorFrames.door2_frame.material = pergamentEffectFrameMaterial;
                break;
        }
    }

    public void UsePowerUpPergament()
    {
        if (numOfPergaments > 0)
        {
            numOfPergaments--;
            SetPrefsPowerUps();
            SetPowerUpsText();

            //make corrct door effect
            int iCorrect = calculations.currentCorrectDoor;
            SetPergamentFrameEffect(iCorrect);

            HandlePowerUpButtons();
        }
        else { Debug.Log("No power ups left!!"); }

    }

    private void HandlePowerUpButtons()
    {
        GetPoWerUpValues();

        if(numOfFreezes == 0)
        {
            freezeBtn.interactable = false;
        }
        if (numOfBooks == 0)
        {
            booksBtn.interactable = false;
        }
        if (numOfPergaments == 0)
        {
            pergamentBtn.interactable = false;
        }
    }
}
