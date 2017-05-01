using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTurnIndicator : MonoBehaviour
{

    public Image Img;
    public Player Owner;
    public TextMeshProUGUI Text;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUp(Player player)
    {
        Owner = player;
    }

    public void SetOn()
    {
        Img.color = Owner.Color;
        Text.color = Color.white;
    }

    public void SetOff()
    {
        Img.color = new Color(0x5E / (float)0xFF, 0x5E / (float)0xFF, 0x5E / (float)0xFF);
        Text.color = new Color(0x85 / (float)0xFF, 0x85 / (float)0xFF, 0x85 / (float)0xFF);
    }
}
