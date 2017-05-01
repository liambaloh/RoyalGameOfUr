using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject ScreenMain;
    public GameObject ScreenOrientation;
    public GameObject ScreenFirst;
    public GameObject ScreenDice;

    // Use this for initialization
    void Start()
    {
        OpenScreen(ScreenMain);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void INPUT_Play()
    {
        OpenScreen(ScreenOrientation);
    }

    public void INPUT_Screen(string type)
    {
        switch (type)
        {
            case "ROTATE":
                GameController.ScreenRotation = GameController.ScreenRotate.ROTATES;
                break;
            case "STATIC":
                GameController.ScreenRotation = GameController.ScreenRotate.STATIC;
                break;
        }
        OpenScreen(ScreenFirst);
    }

    public void INPUT_FirstPlayer(string type)
    {
        switch (type)
        {
            case "PLAYER_1":
                GameController.FirstPlayer = GameController.StartingPlayer.ONE;
                break;
            case "PLAYER_2":
                GameController.FirstPlayer = GameController.StartingPlayer.TWO;
                break;
            case "RANDOM":
                GameController.FirstPlayer = GameController.StartingPlayer.RANDOM;
                break;
            case "ALTERNATE":
                GameController.FirstPlayer = GameController.StartingPlayer.ALTERNATING;
                break;
        }
        OpenScreen(ScreenDice);
    }

    public void INPUT_Dice(string type)
    {
        switch (type)
        {
            case "RNG":
                GameController.TypeOfDice = GameController.DiceType.VIRTUAL;
                break;
            case "COIN":
                GameController.TypeOfDice = GameController.DiceType.COINS;
                break;
            case "D4":
                GameController.TypeOfDice = GameController.DiceType.D4;
                break;
            case "D6":
                GameController.TypeOfDice = GameController.DiceType.D6;
                break;
            case "D20":
                GameController.TypeOfDice = GameController.DiceType.D20;
                break;
        }
        SceneManager.LoadScene("scene");
    }

    public void OpenScreen(GameObject screen)
    {
        ScreenMain.SetActive(false);
        ScreenOrientation.SetActive(false);
        ScreenFirst.SetActive(false);
        ScreenDice.SetActive(false);

        screen.SetActive(true);
    }

    public void INPUT_OpenLL()
    {
        Application.OpenURL("http://liamlime.com");
    }
}
