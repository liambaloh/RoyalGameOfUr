using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        STARTING, PLAYING, GAME_OVER
    }

    public enum PlayerNumber
    {
        ONE, TWO, UNKNOWN
    }

    public enum StartingPlayer
    {
        ONE, TWO, RANDOM, ALTERNATING
    }

    public enum TurnState
    {
        WAITING_FOR_ROLL, WAITING_FOR_MOVE, MOVING, ENDED
    }

    public enum ScreenRotate
    {
        STATIC, ROTATES
    }

    public enum DiceType
    {
        VIRTUAL, COINS, D4, D6, D20
    }

    public static GameController obj;
    public static StartingPlayer FirstPlayer = StartingPlayer.ALTERNATING;
    public static PlayerNumber LastStartingPlayer = PlayerNumber.UNKNOWN;
    public static ScreenRotate ScreenRotation = ScreenRotate.ROTATES;
    public static DiceType TypeOfDice = DiceType.VIRTUAL;
    public static bool IsRestart = false;

    public List<List<Tile>> Map;

    public Sprite TileBase;
    public Sprite TileRollAgain;
    public Sprite TileSafe;
    public Sprite TileStart;
    public Sprite TileEnd;
    public Sprite CoinHeads;
    public Sprite CoinTails;

    public Sprite SettingRotate;
    public Sprite SettingStatic;
    public Sprite SettingRNG;
    public Sprite SettingCoin;
    public Sprite SettingD4;
    public Sprite SettingD6;
    public Sprite SettingD20;
    public Sprite SettingPlayer1;
    public Sprite SettingPlayer2;
    public Sprite SettingRandom;
    public Sprite SettingAlternate;

    public GameState State;
    public TurnState Turn;

    public Player Player1;
    public Player Player2;

    public Player CurrentPlayer;
    public PlayerNumber CurrentPlayerNumber;

    public float GameStartTimer = 0.5f;

    public List<Button> RollButtons;

    public int RolledValue = 0;

    public NotificationText NotificationText;
    public GameObject SettingsScreen;
    public VictoryScreen VictoryScreen;

    public GameObject DiceVirtual;
    public GameObject DiceCoins;
    public GameObject DiceD4;
    public GameObject DiceD6;
    public GameObject DiceD20;

    public int DiceD41Value = 0;
    public int DiceD42Value = 0;
    public int DiceD61Value = 0;
    public int DiceD62Value = 0;
    public int DiceD20Value = 0;

    public List<List<System.Action>> D4Actions;
    public List<List<System.Action>> D6Actions;
    public List<System.Action> D20Actions;

    public List<RectTransform> RotationgObjects;
    public List<RollsDisplay> RollsDisplays;
    public List<RollButton> RollButtonsToReset;

    public GameObject SettingsSubmenuMain;
    public GameObject SettingsSubmenuConfirmRestart;
    public GameObject SettingsSubmenuConfirmBackToMenu;
    public List<DelayedButton> RestartButtons;
    public List<DelayedButton> BackToMenuButtons;

    public RectTransform TutorialAnchor;
    public RectTransform TutorialScreen;
    public GameObject PF_TutorialFirstScreen;

    public bool IsFirstStartup = true;

    private void Awake()
    {
        obj = this;

        Map = new List<List<Tile>>();
        for (int i = 0; i < 3; i++)
        {
            Map.Add(new List<Tile>());
            for (int j = 0; j < 8; j++)
            {
                Map[i].Add(null);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        IsFirstStartup = PlayerPrefs.GetInt("IsFirstStartup", 1) == 1;
        if (IsFirstStartup)
        {
            PlayerPrefs.SetInt("IsFirstStartup", 0);
            StartTutorial();
        }

        State = GameState.STARTING;
        List<Player> players = new List<Player> { Player1, Player2 };
        foreach (Player player in players)
        {
            foreach (Piece piece in player.Pieces)
            {
                piece.SetUp(player);
            }
            player.PlayerTurnIndicator.SetUp(player);
        }
        foreach (Button button in RollButtons)
        {
            button.interactable = false;
        }

        VictoryScreen.gameObject.SetActive(false);

        SetUpD4Actions();
        SetUpD6Actions();
        SetUpD20Actions();
        SetDiceDisplay();

        foreach (RollsDisplay display in RollsDisplays)
        {
            display.Hide();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (State == GameState.STARTING)
        {
            GameStartTimer -= Time.deltaTime;
            if (GameStartTimer <= 0)
            {
                State = GameState.PLAYING;
                PlayerNumber playerToStart = PlayerNumber.ONE;

                if (IsRestart)
                {
                    IsRestart = false;
                    if (LastStartingPlayer != PlayerNumber.UNKNOWN)
                    {
                        playerToStart = LastStartingPlayer;
                    }
                    else
                    {
                        if (Random.Range(0f, 1f) < 0.5f)
                        {
                            playerToStart = PlayerNumber.ONE;
                        }
                        else
                        {
                            playerToStart = PlayerNumber.TWO;
                        }
                    }
                }
                else
                {
                    switch (FirstPlayer)
                    {
                        case StartingPlayer.ONE:
                            playerToStart = PlayerNumber.ONE;
                            break;
                        case StartingPlayer.TWO:
                            playerToStart = PlayerNumber.TWO;
                            break;
                        case StartingPlayer.RANDOM:
                            if (Random.Range(0f, 1f) < 0.5f)
                            {
                                playerToStart = PlayerNumber.ONE;
                            }
                            else
                            {
                                playerToStart = PlayerNumber.TWO;
                            }
                            break;
                        case StartingPlayer.ALTERNATING:
                            if (LastStartingPlayer == PlayerNumber.UNKNOWN)
                            {
                                if (Random.Range(0f, 1f) < 0.5f)
                                {
                                    playerToStart = PlayerNumber.ONE;
                                }
                                else
                                {
                                    playerToStart = PlayerNumber.TWO;
                                }
                            }
                            else
                            {
                                switch (LastStartingPlayer)
                                {
                                    case PlayerNumber.ONE:
                                        playerToStart = PlayerNumber.TWO;
                                        break;
                                    case PlayerNumber.TWO:
                                        playerToStart = PlayerNumber.ONE;
                                        break;
                                }
                            }
                            break;
                    }
                }
                LastStartingPlayer = playerToStart;
                StartTurn(playerToStart);
            }
        }

        //DEBUG remove
        if (Input.GetKeyDown(KeyCode.A))
        {
            Win(PlayerNumber.ONE);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            INPUT_PlayAgain();
        }
    }

    public void NextTurn()
    {
        CheckGameOver();
        switch (CurrentPlayerNumber)
        {
            case PlayerNumber.ONE:
                StartTurn(PlayerNumber.TWO);
                break;
            case PlayerNumber.TWO:
                StartTurn(PlayerNumber.ONE);
                break;
        }
        foreach (RollsDisplay display in RollsDisplays)
        {
            display.Hide();
        }

        foreach (RollButton rollButton in RollButtonsToReset)
        {
            rollButton.Clear();
        }
    }

    public void StartTurn(PlayerNumber player)
    {
        Player1.PlayerTurnIndicator.SetOff();
        Player2.PlayerTurnIndicator.SetOff();

        switch (player)
        {
            case PlayerNumber.ONE:
                CurrentPlayerNumber = PlayerNumber.ONE;
                CurrentPlayer = Player1;
                Player1.PlayerTurnIndicator.SetOn();
                break;
            case PlayerNumber.TWO:
                CurrentPlayerNumber = PlayerNumber.TWO;
                CurrentPlayer = Player2;
                Player2.PlayerTurnIndicator.SetOn();
                break;
        }

        SetTurnState(TurnState.WAITING_FOR_ROLL);

        foreach (Button button in RollButtons)
        {
            button.image.color = CurrentPlayer.ColorPale;
        }

        AlignUIForPlayer(player);
    }

    public void Rolled(int amount)
    {
        RolledValue = amount;

        foreach (RollsDisplay display in RollsDisplays)
        {
            display.Show();
        }

        DiceD41Value = 0;
        DiceD42Value = 0;
        DiceD61Value = 0;
        DiceD62Value = 0;
        DiceD20Value = 0;

        if (amount == 0)
        {
            ShowMessage(CurrentPlayerNumber, "Rolled a 0, next turn.");
            NextTurn();
        }
        else
        {
            bool movePossible = false;
            foreach (Piece piece in CurrentPlayer.Pieces)
            {
                movePossible |= (piece.CanMoveTimes(RolledValue) != null);
            }

            if (!movePossible)
            {
                ShowMessage(CurrentPlayerNumber, "No move possible, next turn.");
                NextTurn();
            }
            else
            {
                SetTurnState(TurnState.WAITING_FOR_MOVE);
            }
        }
    }

    public void INPUT_Rolled(int amount)
    {
        Rolled(amount);
    }

    public void SetTurnState(TurnState turnState)
    {
        Turn = turnState;
        switch (Turn)
        {
            case TurnState.WAITING_FOR_ROLL:

                foreach (RollsDisplay display in RollsDisplays)
                {
                    display.Hide();
                }
                foreach (Button button in RollButtons)
                {
                    button.interactable = true;
                }
                break;
            case TurnState.WAITING_FOR_MOVE:
                foreach (Button button in RollButtons)
                {
                    button.interactable = false;
                }
                foreach (Piece piece in CurrentPlayer.Pieces)
                {
                    piece.SetActiveIfCanMove();
                }
                break;
            case TurnState.MOVING:
                foreach (Button button in RollButtons)
                {
                    button.interactable = false;
                }
                foreach (Piece piece in CurrentPlayer.Pieces)
                {
                    piece.SetInactive();
                }
                break;
            case TurnState.ENDED:
                foreach (Button button in RollButtons)
                {
                    button.interactable = false;
                }
                NextTurn();
                break;
        }
    }

    public void ShowMessage(PlayerNumber recipientPlayer, string message)
    {
        NotificationText.gameObject.SetActive(true);
        NotificationText.ShowMessage(recipientPlayer, message);
    }

    public void CheckGameOver()
    {
        List<Player> players = new List<Player> { Player1, Player2 };
        foreach (Player player in players)
        {
            bool hasWon = true;
            foreach (Piece piece in player.Pieces)
            {
                hasWon &= piece.IsDone;
            }

            if (hasWon)
            {
                Win(player.PlayerNumber);
            }
        }
    }

    public void Win(PlayerNumber playerNumber)
    {
        VictoryScreen.gameObject.SetActive(true);
        State = GameState.GAME_OVER;
        switch (playerNumber)
        {
            case PlayerNumber.ONE:
                VictoryScreen.OnWin(Player1);
                break;
            case PlayerNumber.TWO:
                VictoryScreen.OnWin(Player2);
                break;
        }

        foreach (Button button in RollButtons)
        {
            button.interactable = false;
        }
    }

    public void INPUT_PlayAgain()
    {
        SceneManager.LoadScene("scene");
    }

    public void INPUT_ShowBoard()
    {
        VictoryScreen.gameObject.SetActive(false);
    }

    public void INPUT_BackToMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void AlignUIForPlayer(PlayerNumber playerNumber)
    {
        if (ScreenRotation == ScreenRotate.STATIC)
        {
            playerNumber = PlayerNumber.TWO;
        }

        switch (playerNumber)
        {
            case PlayerNumber.ONE:
                foreach (RectTransform rt in RotationgObjects)
                {
                    rt.localRotation = Quaternion.Euler(0, 0, 180);
                }
                break;
            case PlayerNumber.TWO:
                foreach (RectTransform rt in RotationgObjects)
                {
                    rt.localRotation = Quaternion.Euler(0, 0, 0);
                }
                break;
        }
    }

    public void INPUT_RestartMatch()
    {
        IsRestart = true;
        SceneManager.LoadScene("scene");
    }

    public void INPUT_OpenSettings()
    {
        SettingsScreen.SetActive(true);
    }

    public void INPUT_CloseSettings()
    {
        SettingsScreen.SetActive(false);
    }

    public void SetDiceDisplay()
    {
        DiceVirtual.SetActive(false);
        DiceCoins.SetActive(false);
        DiceD4.SetActive(false);
        DiceD6.SetActive(false);
        DiceD20.SetActive(false);

        switch (TypeOfDice)
        {
            case DiceType.VIRTUAL:
                DiceVirtual.SetActive(true);
                break;
            case DiceType.COINS:
                DiceCoins.SetActive(true);
                break;
            case DiceType.D4:
                DiceD4.SetActive(true);
                break;
            case DiceType.D6:
                DiceD6.SetActive(true);
                break;
            case DiceType.D20:
                DiceD20.SetActive(true);
                break;
        }
    }

    public void INPUT_RolledD4Dice1(int value)
    {
        DiceD41Value = value;
        CheckD4();
    }

    public void INPUT_RolledD4Dice2(int value)
    {
        DiceD42Value = value;
        CheckD4();
    }

    public void CheckD4()
    {
        if (DiceD41Value > 0 && DiceD42Value > 0)
        {
            if (D4Actions.Count >= DiceD41Value)
            {
                if (D4Actions[DiceD41Value - 1].Count >= DiceD42Value)
                {
                    D4Actions[DiceD41Value - 1][DiceD42Value - 1]();
                }
            }
        }
    }

    public void INPUT_RolledD6Dice1(int value)
    {
        DiceD61Value = value;
        CheckD6();
    }

    public void INPUT_RolledD6Dice2(int value)
    {
        DiceD62Value = value;
        CheckD6();
    }

    public void CheckD6()
    {
        if (DiceD61Value > 0 && DiceD62Value > 0)
        {
            if (D6Actions.Count >= DiceD61Value)
            {
                if (D6Actions[DiceD61Value - 1].Count >= DiceD62Value)
                {
                    D6Actions[DiceD61Value - 1][DiceD62Value - 1]();
                }
            }
        }
    }

    public void INPUT_RolledD20(int value)
    {
        DiceD20Value = value;
        CheckD20();
    }

    public void CheckD20()
    {
        if (DiceD20Value > 0 && D20Actions.Count >= DiceD20Value)
        {
            D20Actions[DiceD20Value - 1]();
        }
    }

    public void SetUpD4Actions()
    {
        D4Actions = new List<List<System.Action>>();
        D4Actions.Add(new List<System.Action>());
        D4Actions.Add(new List<System.Action>());
        D4Actions.Add(new List<System.Action>());
        D4Actions.Add(new List<System.Action>());
        D4Actions.Add(new List<System.Action>());
        D4Actions.Add(new List<System.Action>());

        D4Actions[0].Add(Rolled0);  //11
        D4Actions[0].Add(Rolled1);  //12
        D4Actions[0].Add(Rolled1);  //13
        D4Actions[0].Add(Rolled2);  //14

        D4Actions[1].Add(Rolled1);  //21
        D4Actions[1].Add(Rolled2);  //22
        D4Actions[1].Add(Rolled2);  //23
        D4Actions[1].Add(Rolled3);  //24

        D4Actions[2].Add(Rolled1);  //31
        D4Actions[2].Add(Rolled2);  //32
        D4Actions[2].Add(Rolled2);  //33
        D4Actions[2].Add(Rolled3);  //34

        D4Actions[3].Add(Rolled2);  //41
        D4Actions[3].Add(Rolled3);  //42
        D4Actions[3].Add(Rolled3);  //43
        D4Actions[3].Add(Rolled4);  //44
    }

    public void SetUpD6Actions()
    {
        D6Actions = new List<List<System.Action>>();
        D6Actions.Add(new List<System.Action>());
        D6Actions.Add(new List<System.Action>());
        D6Actions.Add(new List<System.Action>());
        D6Actions.Add(new List<System.Action>());
        D6Actions.Add(new List<System.Action>());
        D6Actions.Add(new List<System.Action>());

        D6Actions[0].Add(NotificationReroll);   //11
        D6Actions[0].Add(Rolled0);  //12
        D6Actions[0].Add(Rolled1);  //13
        D6Actions[0].Add(Rolled1);  //14
        D6Actions[0].Add(Rolled1);  //15
        D6Actions[0].Add(Rolled1);  //16

        D6Actions[1].Add(Rolled0);  //21
        D6Actions[1].Add(NotificationReroll);   //22
        D6Actions[1].Add(Rolled2);  //23
        D6Actions[1].Add(Rolled2);  //24
        D6Actions[1].Add(Rolled2);  //25
        D6Actions[1].Add(Rolled3);  //26

        D6Actions[2].Add(Rolled1);  //31
        D6Actions[2].Add(Rolled2);  //32
        D6Actions[2].Add(NotificationReroll);   //33
        D6Actions[2].Add(Rolled2);  //34
        D6Actions[2].Add(Rolled2);  //35
        D6Actions[2].Add(Rolled3);  //36

        D6Actions[3].Add(Rolled1);  //41
        D6Actions[3].Add(Rolled2);  //42
        D6Actions[3].Add(Rolled2);  //43
        D6Actions[3].Add(NotificationReroll);   //44
        D6Actions[3].Add(Rolled2);  //45
        D6Actions[3].Add(Rolled3);  //46

        D6Actions[4].Add(Rolled1);  //51
        D6Actions[4].Add(Rolled2);  //52
        D6Actions[4].Add(Rolled2);  //53
        D6Actions[4].Add(Rolled2);  //54
        D6Actions[4].Add(Rolled4);  //55
        D6Actions[4].Add(Rolled3);  //56

        D6Actions[5].Add(Rolled1);  //61
        D6Actions[5].Add(Rolled3);  //62
        D6Actions[5].Add(Rolled3);  //63
        D6Actions[5].Add(Rolled3);  //64
        D6Actions[5].Add(Rolled3);  //65
        D6Actions[5].Add(Rolled4);  //66
    }

    public void SetUpD20Actions()
    {
        D20Actions = new List<System.Action>();
        D20Actions.Add(NotificationReroll); //1
        D20Actions.Add(NotificationReroll); //2
        D20Actions.Add(NotificationReroll); //3
        D20Actions.Add(NotificationReroll); //4
        D20Actions.Add(Rolled0);            //5
        D20Actions.Add(Rolled1);            //6
        D20Actions.Add(Rolled1);            //7
        D20Actions.Add(Rolled1);            //8
        D20Actions.Add(Rolled1);            //9
        D20Actions.Add(Rolled2);            //10
        D20Actions.Add(Rolled2);            //11
        D20Actions.Add(Rolled2);            //12
        D20Actions.Add(Rolled2);            //13
        D20Actions.Add(Rolled2);            //14
        D20Actions.Add(Rolled2);            //15
        D20Actions.Add(Rolled3);            //16
        D20Actions.Add(Rolled3);            //17
        D20Actions.Add(Rolled3);            //18
        D20Actions.Add(Rolled3);            //19
        D20Actions.Add(Rolled4);            //20
    }

    public void NotificationReroll()
    {
        ShowMessage(CurrentPlayerNumber, "Reroll");
        DiceD41Value = 0;
        DiceD42Value = 0;
        DiceD61Value = 0;
        DiceD62Value = 0;
        DiceD20Value = 0;
    }

    public void Rolled0()
    {
        Rolled(0);
    }
    public void Rolled1()
    {
        Rolled(1);
    }
    public void Rolled2()
    {
        Rolled(2);
    }
    public void Rolled3()
    {
        Rolled(3);
    }
    public void Rolled4()
    {
        Rolled(4);
    }

    public void INPUT_SettingsCancelConfirmation()
    {
        SettingsSubmenuMain.SetActive(true);
        SettingsSubmenuConfirmRestart.SetActive(false);
        SettingsSubmenuConfirmBackToMenu.SetActive(false);
    }

    public void INPUT_ConfirmRestart()
    {
        SettingsSubmenuMain.SetActive(false);
        SettingsSubmenuConfirmRestart.SetActive(true);
        SettingsSubmenuConfirmBackToMenu.SetActive(false);
        foreach (DelayedButton delayedButton in RestartButtons)
        {
            delayedButton.SetUp();
        }
    }

    public void INPUT_ConfirmBackToMenu()
    {
        SettingsSubmenuMain.SetActive(false);
        SettingsSubmenuConfirmRestart.SetActive(false);
        SettingsSubmenuConfirmBackToMenu.SetActive(true);
        foreach (DelayedButton delayedButton in BackToMenuButtons)
        {
            delayedButton.SetUp();
        }
    }

    public void EndTutorial()
    {
        TutorialScreen.gameObject.SetActive(false);
    }

    public void StartTutorial()
    {
        TutorialAnchor.gameObject.SetActive(true);
        GameObject go = Instantiate(PF_TutorialFirstScreen);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.SetParent(TutorialAnchor);
        rt.localScale = Vector3.one;
        rt.localPosition = Vector3.zero;
    }

    public void INPUT_StartTutorial()
    {
        StartTutorial();
    }

    public void INPUT_EndTutorial()
    {
        EndTutorial();
    }
}
