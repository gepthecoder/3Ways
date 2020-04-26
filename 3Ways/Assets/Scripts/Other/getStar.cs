using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getStar : MonoBehaviour
{
    public void GET_STAR()
    {
        CoinManager.XP++;
        CoinManager.Save();
    }
}
