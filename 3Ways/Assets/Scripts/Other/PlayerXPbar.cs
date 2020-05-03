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

    private float amountNeeded = 250.0f;

    private Coroutine routine;

    public Text TXT_XP_INFO;


    private void SetTextElements_XP_INFO()
    {
        TXT_XP_INFO.text = TXT_XP_INFO.text = PlayrXP.XPoints + " / " + amountNeeded;
    }

    void OnEnable()
    {
        InitColor();

        currentAmount = PlayerPrefs.GetInt("XPoints", 0) / PlayerPrefs.GetFloat("amountNeeded", 250) * 1.0f;
        Debug.Log("currentAmount = " + currentAmount);

        bar_fill.fillAmount = currentAmount;

        SetTextElements_XP_INFO();
    }

    void Awake()
    {
        if (PlayerPrefs.HasKey("amountNeeded"))
        {
            // we had a previous session
            amountNeeded = PlayerPrefs.GetFloat("amountNeeded", 250);
        }
        else
        {
            Save();
        }
    }

    void Start()
    {
        InitPrefs();
    }

    void Update()
    {
        if (ShowGainedXP)
        {
            Debug.Log("gainedXP = " + PlayrXP.gainedXP);

            PlayrXP.XPoints += PlayrXP.gainedXP;
            PlayrXP.Save();
            Debug.Log("PlayrXP.XPoints= " + PlayrXP.XPoints);

            float increase = PlayrXP.XPoints / amountNeeded;
            Debug.Log("Increase Value = " + increase);


            UpdateProgress(increase);
                
            ShowGainedXP = false;

        }
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

    public void UpdateProgress(float amount, float duration = 0.1f)
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
            bar_fill.fillAmount = tempAmount + diff * percent;
            yield return null;
        }

        if(currentAmount >= 1)
        {
            LevelUp();
        }
    }

    private void LevelUp() {
        UpdateLevel(level + 1);
        UpdateProgress(-1f, 0.2f);
        UpdateAmountNeeded();
    }

    private void UpdateAmountNeeded()
    {
        amountNeeded *= 2;
        Save();
        SetTextElements_XP_INFO();
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

        PlayerPrefs.Save();
    }
}
