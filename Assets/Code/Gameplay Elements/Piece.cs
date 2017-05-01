using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    public Image Img;
    public Button Btn;
    public Player Owner;

    public RectTransform RT;
    public Vector2 TargetPos;
    public Tile TargetTile;
    public Tile CurrentTile;
    public bool Moving = false;

    public const float TILE_LERP_SPEED = 8f;
    public const float PIECE_ARRIVE_THRESHOLD = 1f;
    public RectTransform InitialParent;
    public Vector2 InitialPosition;

    public bool IsDone = false;
    public bool IsBlinking = false;
    public float BlinkTimer = 0f;

    // Use this for initialization
    void Start()
    {
        RT = this.GetComponent<RectTransform>();
        TargetPos = RT.position;

        InitialParent = (RectTransform)RT.parent;
        InitialPosition = RT.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetPos != null && Moving)
        {
            RT.position = Vector2.Lerp(RT.position, TargetPos, TILE_LERP_SPEED * Time.deltaTime);
            if (Vector2.Distance(RT.position, TargetPos) <= PIECE_ARRIVE_THRESHOLD)
            {
                OnArrive(TargetTile);
            }
        }

        if (IsBlinking)
        {
            BlinkTimer += Time.deltaTime;
            if (BlinkTimer % 1 < 0.5f)
            {
                Img.color = Owner.ColorAccent;
            }
            else
            {
                Img.color = Owner.ColorAccent2;
            }
        }
    }

    public void SetUp(Player owner)
    {
        Img = this.GetComponent<Image>();
        Owner = owner;

        Img.color = owner.Color;
    }

    public void MoveToTile(Tile tile)
    {
        if (tile == null)
        {
            TargetTile = null;
            TargetPos = InitialPosition;
        }
        else
        {
            TargetTile = tile;
            TargetPos = TargetTile.RT.position;
        }
        GameController.obj.SetTurnState(GameController.TurnState.MOVING);
        Moving = true;
    }

    public void OnArrive(Tile targetTile)
    {
        Moving = false;
        if (CurrentTile != null && CurrentTile.PieceOnTile == this)
        {
            CurrentTile.PieceOnTile = null;
        }
        CurrentTile = targetTile;
        TargetTile = null;

        if (CurrentTile == null)
        {
            return;
        }

        //Eat piece on tile
        if (CurrentTile.PieceOnTile != null)
        {
            if (CurrentTile.PieceOnTile.Owner.PlayerNumber != Owner.PlayerNumber)
            {
                if (CurrentTile.Type == Tile.TileType.SAFE)
                {
                    Debug.LogError("Trying to arrive on safe tile with enemy on");
                }
                CurrentTile.PieceOnTile.MoveToTile(null);
            }
        }

        CurrentTile.PieceOnTile = this;
        switch (CurrentTile.Type)
        {
            case Tile.TileType.START:
                GameController.obj.SetTurnState(GameController.TurnState.ENDED);
                break;
            case Tile.TileType.SAFE:
                GameController.obj.SetTurnState(GameController.TurnState.WAITING_FOR_ROLL);
                break;
            case Tile.TileType.ROLL_AGAIN:
                GameController.obj.SetTurnState(GameController.TurnState.WAITING_FOR_ROLL);
                break;
            case Tile.TileType.NORMAL:
                GameController.obj.SetTurnState(GameController.TurnState.ENDED);
                break;
            case Tile.TileType.END:
                OnDone();
                GameController.obj.SetTurnState(GameController.TurnState.ENDED);
                break;
        }
    }

    public bool CanMove()
    {
        if (!IsDone &&
            GameController.obj.State == GameController.GameState.PLAYING &&
            GameController.obj.Turn == GameController.TurnState.WAITING_FOR_MOVE &&
            GameController.obj.CurrentPlayerNumber == Owner.PlayerNumber)
        {
            return true;
        }
        return false;
    }

    public void INPUT_OnClick()
    {
        if (CanMove())
        {
            MoveTimes(GameController.obj.RolledValue);
        }
    }

    public void MoveTimes(int times)
    {
        Tile targetTile = CanMoveTimes(times);
        if (targetTile != null)
        {
            MoveToTile(targetTile);
        }
    }

    public Tile CanMoveTimes(int times)
    {
        if (IsDone)
        {
            return null;
        }

        int timesLeft = times;
        Tile tile = this.CurrentTile;
        if (tile == null)
        {
            tile = Owner.StartTile;
        }
        while (timesLeft > 0)
        {
            switch (Owner.PlayerNumber)
            {
                case GameController.PlayerNumber.ONE:
                    tile = tile.NextP1;
                    if (tile == null)
                    {
                        return null;
                    }
                    break;
                case GameController.PlayerNumber.TWO:
                    tile = tile.NextP2;
                    if (tile == null)
                    {
                        return null;
                    }
                    break;
            }
            timesLeft--;
        }

        if (tile.PieceOnTile != null)
        {
            if (tile.Type == Tile.TileType.SAFE)
            {
                return null;
            }
            if (tile.PieceOnTile.Owner.PlayerNumber == this.Owner.PlayerNumber)
            {
                return null;
            }
        }

        return tile;
    }

    public void OnDone()
    {
        IsDone = true;
        this.CurrentTile.PieceOnTile = null;
        this.CurrentTile = null;
    }

    public void SetActiveIfCanMove()
    {
        if (this.CanMoveTimes(GameController.obj.RolledValue) != null)
        {
            Img.color = GameController.obj.CurrentPlayer.ColorAccent;
            Btn = this.GetComponent<Button>();
            Btn.interactable = true;
            IsBlinking = true;
            BlinkTimer = 0;
        }
        else
        {
            SetInactive();
        }
    }

    public void SetInactive()
    {
        Img.color = GameController.obj.CurrentPlayer.Color;
        Btn = this.GetComponent<Button>();
        Btn.interactable = false;
        IsBlinking = false;
        BlinkTimer = 0;
    }
}
