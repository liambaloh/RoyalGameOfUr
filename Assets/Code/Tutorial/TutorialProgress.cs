using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialProgress : MonoBehaviour
{
    public GameObject ThisScreen;
    public GameObject PF_NextScreen;
    public const float DELAY_TIMER = 1f;
    public float timer = DELAY_TIMER;
    public Button Btn;
    public GameObject Player1Text;

    // Use this for initialization
    void Start()
    {
        Btn = this.GetComponent<Button>();
        SetUp();
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

    public void INPUT_OnClick()
    {
        if (PF_NextScreen == null)
        {
            GameController.obj.EndTutorial();
            Destroy(ThisScreen);
        }
        else
        {
            GameObject go = Instantiate(PF_NextScreen);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(GameController.obj.TutorialAnchor);
            rt.localScale = Vector3.one;
            rt.localPosition = Vector3.zero;
            Destroy(ThisScreen);
        }
    }

    public void SetUp()
    {
        timer = DELAY_TIMER;
        Btn.interactable = false;
        if(GameController.ScreenRotation != GameController.ScreenRotate.ROTATES){
            Player1Text.SetActive(false);
        }
    }
}
