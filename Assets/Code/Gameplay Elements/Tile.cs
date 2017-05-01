using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        NORMAL, SAFE, ROLL_AGAIN, START, END
    }

    public Image Img;
    public TileType Type;
    public RectTransform RT;
    public Tile NextP1;
    public Tile NextP2;
    public Tile PreviousP1;
    public Tile PreviousP2;
    public Piece PieceOnTile;

    public int X;
    public int Y;

    // Use this for initialization
    void Start()
    {
        RT = this.GetComponent<RectTransform>();
        Img = this.GetComponent<Image>();
        switch (Type)
        {
            case TileType.NORMAL:
                Img.sprite = GameController.obj.TileBase;
                break;
            case TileType.SAFE:
                Img.sprite = GameController.obj.TileSafe;
                break;
            case TileType.ROLL_AGAIN:
                Img.sprite = GameController.obj.TileRollAgain;
                break;
            case TileType.START:
                Img.sprite = GameController.obj.TileStart;
                break;
            case TileType.END:
                Img.sprite = GameController.obj.TileEnd;
                break;
        }

        GameController.obj.Map[Y][X] = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
