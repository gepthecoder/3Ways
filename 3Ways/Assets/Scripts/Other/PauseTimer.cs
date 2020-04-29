using UnityEngine;
using UnityEngine.UI;

public class PauseTimer : MonoBehaviour
{
    public bool pauseMenuOpened;

    private Text pauseTimer;
    private float pT;

    public bool bForce;
    
    void Start()
    {
        pauseTimer = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (pauseMenuOpened)
        {
            bForce = true;
            pT += Time.unscaledDeltaTime;

            string minutes = ((int)pT / 60).ToString("00");
            string seconds = (pT % 60).ToString("00");
            string miliseconds = ((int)(pT * 100f) % 100).ToString("00");

            pauseTimer.text = minutes + ":" + seconds + ":" + miliseconds;
        }
        else {
            if (bForce)
            {
                Debug.Log("Pause timer to zero!");
                ForceSetCurrentTime(0);
                bForce = false;
                return;
            }
          }
        
    }

    public float GetCurrentTime()
    {
        return pT;
    }

    public void ForceSetCurrentTime(float t)
    {
        pT = 0;
        pT = t;
    }
}

