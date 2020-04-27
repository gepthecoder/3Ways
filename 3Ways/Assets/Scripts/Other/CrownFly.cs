using UnityEngine;
using UnityEngine.UI;

public class CrownFly : MonoBehaviour
{
    public Animator uiCrownAnime;
    private CrownScript crowns;

    void Start()
    {
        crowns = GetComponentInParent<CrownScript>();
    }

    public void IncreaseCrownValue()
    {
        uiCrownAnime.GetComponentInChildren<Text>().text = CoinManager.CROWNS.ToString();
    }

    public void SpinCrown()
    {
        uiCrownAnime.SetTrigger("spin");
    }
}
