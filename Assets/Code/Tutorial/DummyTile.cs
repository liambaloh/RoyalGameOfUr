using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyTile : MonoBehaviour
{

    public Tile ReferenceTile;
    public Image Img;
    public int X;
    public int Y;


    // Use this for initialization
    void Start()
    {
        ReferenceTile = GameController.obj.Map[Y][X];
        Img = this.GetComponent<Image>();
        Img.sprite = ReferenceTile.Img.sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
