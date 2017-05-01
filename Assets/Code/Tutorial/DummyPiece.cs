using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyPiece : MonoBehaviour
{
    public GameController.PlayerNumber PlayerNumber;
    public Image Img;
    public List<RectTransform> FollowPath;
    public List<float> Delays;
    public int CurrentPosition = 0;

    public const float MOVE_SPEED = 300;
    public float InitialDelay = 0;

    // Use this for initialization
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        InitialDelay -= Time.deltaTime;
        if (InitialDelay > 0)
        {
            return;
        }

        if (CurrentPosition < FollowPath.Count)
        {
            this.transform.position = Vector2.MoveTowards((Vector2)this.transform.position, (Vector2)FollowPath[CurrentPosition].position, MOVE_SPEED * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, FollowPath[CurrentPosition].position) < 0.01f)
            {
                CurrentPosition++;
                if (CurrentPosition < Delays.Count)
                {
                    InitialDelay = Delays[CurrentPosition];
                }
            }
        }
    }
}
