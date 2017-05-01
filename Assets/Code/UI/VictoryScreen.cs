using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public void OnWin(Player player)
    {
        Text.text = player.Name + " wins!";
    }
}
