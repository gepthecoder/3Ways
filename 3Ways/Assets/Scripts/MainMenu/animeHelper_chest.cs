using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animeHelper_chest : MonoBehaviour
{
    // SLOT
    [Header("SLOT HELPER")]
    public Slot slotMachineScript;

    public void GetCrownReward()
    {
        CoinManager.CROWNS += Slot.iCurrentRewardAmount;
        CoinManager.Save();

        slotMachineScript.JackpotVFX.SetActive(false);
        slotMachineScript.NormalVFX.SetActive(false);
    }

    public void SetSpinSlot()
    {
        slotMachineScript.bSpinSlot = false;
    }
}
