using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static int XP = 0;
    public static int CROWNS = 0;
     
    public Text XP_AMOUNT_TXT;
    public Text CROWNS_AMOUNT_TXT;

    void Awake()
    {
        if (PlayerPrefs.HasKey("XP") || PlayerPrefs.HasKey("CROWNS"))
        {
            //we had a previous session
            XP = PlayerPrefs.GetInt("XP", 0);
            CROWNS = PlayerPrefs.GetInt("CROWNS", 0);
        }
        else
        {
            //Get saved values
            Save();
        }
    }

    void Start()
    {
        SetTextElements(XP, CROWNS);
    }

    private void Update()
    {
        SetTextElements(XP, CROWNS);
    }

    private void SetTextElements(int xP, int croWns)
    {
        XP_AMOUNT_TXT.text = xP.ToString();
        CROWNS_AMOUNT_TXT.text = croWns.ToString();
    }

    public static void Save()
    {
        PlayerPrefs.SetInt("XP", XP);
        PlayerPrefs.SetInt("CROWNS", CROWNS);
    }
}
