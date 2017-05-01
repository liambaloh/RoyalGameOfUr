using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Player
{
    public Color Color;
    public Color ColorPale;
    public Color ColorAccent;
    public Color ColorAccent2;
    public string Name;

    public List<Piece> Pieces;
    public Tile StartTile;
    public GameController.PlayerNumber PlayerNumber;

    public PlayerTurnIndicator PlayerTurnIndicator;
}
