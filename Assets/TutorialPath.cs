using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPath : MonoBehaviour
{
    public enum Axis
    {
        X, Y
    }

    public bool StartActive = false;
    public bool Active;
    public float TargetSize;
    public Axis GrowthAxis;
    public RectTransform RT;
    public TutorialPath NextPath;
    public Image Img;
    public GameController.PlayerNumber PlayerNumber;
    public bool Animated = true;

    public const float LINE_WIDTH = 20;
    public const float SPREAD_SPEED = 200;


    // Use this for initialization
    void Start()
    {
        RT = this.GetComponent<RectTransform>();
        Img = this.GetComponent<Image>();

        switch (PlayerNumber)
        {
            case GameController.PlayerNumber.ONE:
                Img.color = GameController.obj.Player1.ColorAccent;
                break;
            case GameController.PlayerNumber.TWO:
                Img.color = GameController.obj.Player2.ColorAccent;
                break;
        }

        if (Animated)
        {
            switch (GrowthAxis)
            {
                case Axis.X:
                    TargetSize = RT.sizeDelta.x;
                    break;
                case Axis.Y:
                    TargetSize = RT.sizeDelta.y;
                    break;
            }

            RT.sizeDelta = Vector2.zero;

            if (StartActive)
            {
                Activate();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            switch (GrowthAxis)
            {
                case Axis.X:
                    RT.sizeDelta = RT.sizeDelta + new Vector2(SPREAD_SPEED * Time.deltaTime, 0);
                    if (RT.sizeDelta.x >= TargetSize)
                    {
                        if (NextPath != null)
                        {
                            NextPath.Activate();
                        }
                        Active = false;
                    }
                    break;
                case Axis.Y:
                    RT.sizeDelta = RT.sizeDelta + new Vector2(0, SPREAD_SPEED * Time.deltaTime);
                    if (RT.sizeDelta.y >= TargetSize)
                    {
                        if (NextPath != null)
                        {
                            NextPath.Activate();
                        }
                        Active = false;
                    }
                    break;
            }
        }
    }

    public void Activate()
    {
        Active = true;
        switch (GrowthAxis)
        {
            case Axis.X:
                RT.sizeDelta = new Vector2(0, LINE_WIDTH);
                break;
            case Axis.Y:
                RT.sizeDelta = new Vector2(LINE_WIDTH, 0);
                break;
        }
    }
}
