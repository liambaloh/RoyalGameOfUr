using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayedButton : MonoBehaviour
{
    public const float DELAY_TIMER = 1f;
    public float timer = DELAY_TIMER;
    public Button Btn;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && Btn.interactable == false)
        {
            Btn.interactable = true;
        }
    }

    public void SetUp()
    {
        timer = DELAY_TIMER;
        Btn.interactable = false;
    }
}
