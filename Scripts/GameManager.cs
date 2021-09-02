using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isCircleFirst = false;
    public GameObject selectPlayerPanel;
    //  SCORE
    public int circleScore = 0;
    public int crossScore = 0;
    public List<InputField> scoreFields = new List<InputField>();
    //  SLOT
    public List<GameObject> slots = new List<GameObject>();


    private void Awake()
    {
        instance = this;
    }
    public void SelectCircle()
    {
        Board.instance.isCircleTurn = true;
        isCircleFirst = true;
        selectPlayerPanel.SetActive(false);
    }
    public void SelectCross()
    {
        Board.instance.isCircleTurn = false;
        isCircleFirst = false;
        selectPlayerPanel.SetActive(false);
    }
    public void ResetGame()
    {
        Board.instance.ClearBoard();
        Board.instance.CreateBoard();
        Board.instance.isCircleTurn = isCircleFirst;
    }
    public void MenuGame()
    {
        Board.instance.ClearBoard();
        Board.instance.CreateBoard();
        selectPlayerPanel.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
