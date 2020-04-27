using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getStar : MonoBehaviour
{
    public Animator star_anime;
    public CoinManager coinManager;

    public void GET_STAR()
    {
        CoinManager.XP++;
        CoinManager.Save();
        coinManager.DisplayTextElement(CoinManager.XP);
    }

    public void SpinCoin()
    {
        star_anime.SetTrigger("spin");
    }
}
