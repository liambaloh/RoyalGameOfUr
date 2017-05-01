using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyCoin : MonoBehaviour
{

    public Image Img;
    public float Timer;
    public const float FLIP_TIME = 2f;

    // Use this for initialization
    void Start()
    {
        Img = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Timer += FLIP_TIME;
            if (Random.Range(0f, 1f) < 0.5f)
            {
                Img.sprite = GameController.obj.CoinHeads;
            }
            else
            {
                Img.sprite = GameController.obj.CoinTails;
            }
        }
    }
}
