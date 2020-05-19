using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerXPbar : MonoBehaviour
{
    public static bool ShowGainedXP;
    
    [SerializeField]
    private Text txt1;
    
    [SerializeField]
    private Text txt2;
    
    [SerializeField]
    private Image bar_fill;

    [SerializeField]
    private Image bar_outline;


    [SerializeField]
    private Image circle_1;
    
    [SerializeField]
    private Image circle_2;


    [SerializeField]
    private Color color;

    [SerializeField]
    private Color background_color;

    private int level = 1;
    private float currentAmount = 0;

    private float amountNeeded = 100.0f;
    private float startValue = 0f;


    private Coroutine routine;

    public Text TXT_XP_INFO;

    [SerializeField]
    private Slider levelXPbar;


    private void SetTextElements_XP_INFO()
    {
       TXT_XP_INFO.text = PlayrXP.XPoints + " / " + PlayerPrefs.GetFloat("amountNeeded");
    }

    private void SET_MIN_MAX_SLIDER(float min, float max)
    {
        levelXPbar.minValue = min;
        levelXPbar.maxValue = max;
    }

    void Awake()
    {

        if (PlayerPrefs.HasKey("amountNeeded"))
        {
            // we had a previous session
            amountNeeded = PlayerPrefs.GetFloat("amountNeeded", 100);
            startValue = PlayerPrefs.GetFloat("startValue", 0);
        }
        else
        {
            Save();
        }

        InitColor();

        currentAmount = PlayerPrefs.GetInt("XPoints", 0); /*/ PlayerPrefs.GetFloat("amountNeeded", 100) * 1.0f*/
        Debug.Log("currentAmount = " + currentAmount);

        SET_MIN_MAX_SLIDER(startValue, amountNeeded);
        levelXPbar.value = currentAmount;

    }

    void Start()
    {
        InitPrefs();
        UpdateLevel(level);
        SetTextElements_XP_INFO();

        Debug.Log("Amount Needed: " + amountNeeded);

    }

    void Update()
    {
        SetTextElements_XP_INFO();


        if (ShowGainedXP)
        {
            Debug.Log("gainedXP = " + PlayrXP.gainedXP);

            PlayrXP.XPoints += PlayrXP.gainedXP;
            PlayrXP.Save();
            Debug.Log("PlayrXP.XPoints= " + PlayrXP.XPoints);

            float increase = PlayrXP.gainedXP /*/ amountNeeded*/;
            Debug.Log("Increase Value = " + increase);
            
            UpdateProgress(increase);
            ShowGainedXP = false;

            if (GameTimer.playerHasBeatRecord)
            {
                UIManager.NEW_RECORD = true;
            }
        }
    }

    public void EXTRA_XP()
    {

        PlayrXP.XPoints += 100;
        PlayrXP.Save();

        float increase = 100;
        Debug.Log("Increase Value = " + increase);

        UpdateProgress(increase);
    }

    void InitColor()
    {
        circle_1.color = color;
        circle_2.color = color;

        bar_fill.color = color;
        bar_outline.color = color;

        txt1.color = background_color;
        txt2.color = color;
    }

    void InitPrefs()
    {
        level = PlayrXP.currentLevel;
    }

    public void UpdateProgress(float amount, float duration = 1f, bool lvlUP=false)
    {
        if(routine != null)
        {
            StopCoroutine(routine);
        }

        float target = currentAmount + amount;

        routine = StartCoroutine(FillRoutine(target, duration));
    }

    private IEnumerator FillRoutine(float target, float duration)
    {
        float time = 0;
        float tempAmount = currentAmount;
        float diff = target - tempAmount;
        currentAmount = target;

        while(time < duration)
        {
            time += Time.deltaTime;
            float percent = time / duration;
            levelXPbar.value = tempAmount + diff * percent;
            yield return null;
        }

        if(currentAmount >= amountNeeded)
        {
            UIManager.LEVEL_UP = true;
            LevelUp();
        }
    }

    private void LevelUp() {
        UpdateLevel(level + 1);
        UpdateAmountNeeded();
        SET_MIN_MAX_SLIDER(startValue, amountNeeded);
        Debug.Log("DECREASE SLIDER FOR: " + -(currentAmount - startValue));
        levelXPbar.value = currentAmount;
    }

    private void UpdateAmountNeeded()
    {
        startValue = amountNeeded;
        amountNeeded *= 2;
        Save();
    }

    private void UpdateLevel(int level)
    {
        this.level = level;
        txt1.text = this.level.ToString();
        txt2.text = (this.level + 1).ToString();
        PlayrXP.currentLevel = level;
        PlayrXP.Save();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("amountNeeded", amountNeeded);
        PlayerPrefs.SetFloat("startValue", startValue);
        PlayerPrefs.Save();
    }
}
