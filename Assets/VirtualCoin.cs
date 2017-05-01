using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualCoin : MonoBehaviour
{
    public Image Img;

    // Use this for initialization
    void Start()
    {
        Img = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int Roll()
    {
        Img = this.GetComponent<Image>();
        int roll = Random.Range(0, 2);
        switch (roll)
        {
            case 0:
                Img.sprite = GameController.obj.CoinTails;
                break;
            case 1:
                Img.sprite = GameController.obj.CoinHeads;
                break;
        }
        return roll;
    }

    public void Clear()
    {
        Img = this.GetComponent<Image>();
        Img.sprite = GameController.obj.CoinTails;
    }
}
