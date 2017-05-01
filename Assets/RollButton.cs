using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollButton : MonoBehaviour
{
    public List<VirtualCoin> VirtualCoins;

    public void Roll()
    {
        int amount = 0;
        foreach (VirtualCoin virtualCoin in VirtualCoins)
        {
            amount += virtualCoin.Roll();
        }
        GameController.obj.Rolled(amount);
    }

    public void Clear()
    {
        foreach (VirtualCoin virtualCoin in VirtualCoins)
        {
            virtualCoin.Clear();
        }
    }

    public void INPUT_Roll()
    {
        Roll();
    }
}
