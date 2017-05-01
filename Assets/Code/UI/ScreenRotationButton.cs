using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenRotationButton : MonoBehaviour
{
    public Sprite SpriteRotate;
    public List<RectTransform> ObjectsToRotate;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void INPUT_OnClick()
    {
        foreach (RectTransform rt in ObjectsToRotate)
        {
            rt.Rotate(0, 0, 180);
        }
    }
}
