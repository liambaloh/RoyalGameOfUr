using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingToggleButton : MonoBehaviour
{

    public enum SettingType
    {
        SCREEN_ROTATION, PLAYER_ORDER, DICE
    }

    public Image Img;
    public SettingType Type;

    // Use this for initialization
    void Start()
    {
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateSprite()
    {
        switch (Type)
        {
            case SettingType.DICE:
                switch (GameController.TypeOfDice)
                {
                    case GameController.DiceType.VIRTUAL:
                        Img.sprite = GameController.obj.SettingRNG;
                        break;
                    case GameController.DiceType.COINS:
                        Img.sprite = GameController.obj.SettingCoin;
                        break;
                    case GameController.DiceType.D4:
                        Img.sprite = GameController.obj.SettingD4;
                        break;
                    case GameController.DiceType.D6:
                        Img.sprite = GameController.obj.SettingD6;
                        break;
                    case GameController.DiceType.D20:
                        Img.sprite = GameController.obj.SettingD20;
                        break;
                }
                break;
            case SettingType.PLAYER_ORDER:
                switch (GameController.FirstPlayer)
                {
                    case GameController.StartingPlayer.ONE:
                        Img.sprite = GameController.obj.SettingPlayer1;
                        break;
                    case GameController.StartingPlayer.TWO:
                        Img.sprite = GameController.obj.SettingPlayer2;
                        break;
                    case GameController.StartingPlayer.RANDOM:
                        Img.sprite = GameController.obj.SettingRandom;
                        break;
                    case GameController.StartingPlayer.ALTERNATING:
                        Img.sprite = GameController.obj.SettingAlternate;
                        break;
                }
                break;
            case SettingType.SCREEN_ROTATION:
                switch (GameController.ScreenRotation)
                {
                    case GameController.ScreenRotate.ROTATES:
                        Img.sprite = GameController.obj.SettingRotate;
                        break;
                    case GameController.ScreenRotate.STATIC:
                        Img.sprite = GameController.obj.SettingStatic;
                        break;
                }
                break;
        }
    }

    public void INPUT_OnClick()
    {
        switch (Type)
        {
            case SettingType.DICE:
                switch (GameController.TypeOfDice)
                {
                    case GameController.DiceType.VIRTUAL:
                        GameController.TypeOfDice = GameController.DiceType.COINS;
                        break;
                    case GameController.DiceType.COINS:
                        GameController.TypeOfDice = GameController.DiceType.D4;
                        break;
                    case GameController.DiceType.D4:
                        GameController.TypeOfDice = GameController.DiceType.D6;
                        break;
                    case GameController.DiceType.D6:
                        GameController.TypeOfDice = GameController.DiceType.D20;
                        break;
                    case GameController.DiceType.D20:
                        GameController.TypeOfDice = GameController.DiceType.VIRTUAL;
                        break;
                }
                GameController.obj.SetDiceDisplay();
                break;
            case SettingType.PLAYER_ORDER:
                switch (GameController.FirstPlayer)
                {
                    case GameController.StartingPlayer.ONE:
                        GameController.FirstPlayer = GameController.StartingPlayer.TWO;
                        break;
                    case GameController.StartingPlayer.TWO:
                        GameController.FirstPlayer = GameController.StartingPlayer.RANDOM;
                        break;
                    case GameController.StartingPlayer.RANDOM:
                        GameController.FirstPlayer = GameController.StartingPlayer.ALTERNATING;
                        break;
                    case GameController.StartingPlayer.ALTERNATING:
                        GameController.FirstPlayer = GameController.StartingPlayer.ONE;
                        break;
                }
                break;
            case SettingType.SCREEN_ROTATION:
                switch (GameController.ScreenRotation)
                {
                    case GameController.ScreenRotate.ROTATES:
                        GameController.ScreenRotation = GameController.ScreenRotate.STATIC;
                        break;
                    case GameController.ScreenRotate.STATIC:
                        GameController.ScreenRotation = GameController.ScreenRotate.ROTATES;
                        break;
                }
                GameController.obj.AlignUIForPlayer(GameController.obj.CurrentPlayerNumber);
                break;
        }
        UpdateSprite();
    }
}
