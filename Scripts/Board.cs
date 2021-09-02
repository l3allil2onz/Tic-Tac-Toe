using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public static Board instance;
    public bool isCircleTurn = true;
    public bool isBoardCreated = false;
    public byte turnCount = 0;
    public GameObject cirClePrefab, crossPrefab;
    public byte[,] symbolsContainer = new byte[3, 3];
    int sizeOfBoard = 3;
    int findingWinnerTurn = 5;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        CreateBoard();
    }
    public void GetWinner(int row, int column, byte symbol)
    {
        turnCount++;
        symbolsContainer[row, column] = symbol;

        if (turnCount >= findingWinnerTurn)
        {
            GetWinner(symbol);
        }

        isCircleTurn = !isCircleTurn;
    }
    private void GetWinner(byte symbol)
    {
        if(GetWinnerAlongWithDiagonal(symbol) || 
           GetWinnerAlongWithRows(symbol) ||
           GetWinnerAlongWithColumns(symbol))
        {
            UpdateScore();
        }
    }
    private bool GetWinnerAlongWithColumns(byte symbol)
    {
        if (symbolsContainer[0, 0] == symbol &&
            symbolsContainer[1, 0] == symbol &&
            symbolsContainer[2, 0] == symbol)
        {
            return true;
        }
        else if (symbolsContainer[0, 1] == symbol &&
                 symbolsContainer[1, 1] == symbol &&
                 symbolsContainer[2, 1] == symbol)
        {
            return true;
        }
        else if (symbolsContainer[0, 2] == symbol &&
                 symbolsContainer[1, 2] == symbol &&
                 symbolsContainer[2, 2] == symbol)
        {
            return true;
        }
        return false;
    }
    private bool GetWinnerAlongWithRows(byte symbol)
    {
        if (symbolsContainer[0, 0] == symbol &&
            symbolsContainer[0, 1] == symbol &&
            symbolsContainer[0, 2] == symbol)
        {
            return true;
        }
        else if (symbolsContainer[1, 0] == symbol &&
                 symbolsContainer[1, 1] == symbol &&
                 symbolsContainer[1, 2] == symbol)
        {
            return true;
        }
        else if (symbolsContainer[2, 0] == symbol &&
                 symbolsContainer[2, 1] == symbol &&
                 symbolsContainer[2, 2] == symbol)
        {
            return true;
        }
        return false;
    }
    private bool GetWinnerAlongWithDiagonal(byte symbol)
    {
        if (symbolsContainer[0, 0] == symbol &&
            symbolsContainer[1, 1] == symbol &&
            symbolsContainer[2, 2] == symbol)
        {
            return true;
        }
        else if (symbolsContainer[0, 2] == symbol &&
                 symbolsContainer[1, 1] == symbol &&
                 symbolsContainer[2, 0] == symbol)
        {
            return true;
        }
        return false;
    }
    /*
    private void GetBoard(byte symbol)
    {
        bool haveWinner;

        haveWinner = GetAlongWithAxis(symbol);

        if(!haveWinner)
            GetAlongWithDiagonal(symbol);
     
        if(haveWinner)
            UpdateScore();
    }
    private void GetAlongWithDiagonal(byte symbol)
    {

    }
    private bool GetWinnerAlongWithAxis(byte symbol)
    {
        byte tempSymbolCount = 0;
        byte axisCount = 2;

        for (int i = 0; i < axisCount; i++)
        {
            for (int j = 0; j < sizeOfBoard; j++)
            {
                tempSymbolCount = 0;
                print(tempSymbolCount);
                for (int k = 0; k < sizeOfBoard; k++)
                {
                    if (symbolsContainer[j, k] != 0 || symbolsContainer[k, j] != 0)
                    {
                        switch (i)
                        {
                            case 0:
                                if (symbolsContainer[j, k] == symbol)
                                {
                                    tempSymbolCount++;
                                }
                                break;
                            case 1:
                                if (symbolsContainer[k, j] == symbol)
                                {
                                    tempSymbolCount++;
                                }
                                break;
                            default: break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (tempSymbolCount >= 3)
                {
                    return true;
                }
            }
        }
        return false;
    }
    */
    private byte GetSymbol()
    {
        if (isCircleTurn)
            return 1;
        else
            return 2;
    }
    private void SetAllButtonInteractable(bool b)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject thisSlot = transform.GetChild(i).gameObject;
            thisSlot.GetComponent<Button>().interactable = b;
        }
    }
    public void ClearBoard()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject thisSlot = transform.GetChild(i).gameObject;
            if (thisSlot.transform.childCount > 0)
            {
                GameObject thisSymbol = thisSlot.transform.GetChild(0).gameObject;
                Destroy(thisSymbol);
            }
        }
    }
    public void CreateBoard()
    {
        if (!isBoardCreated)
        {
            int pivot = 0;
            int tempRowOuter = 0;
            int tempColumnOuter = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                int tempIndex = i;
                int tempRowInner = tempRowOuter;
                int tempColumnInner = tempColumnOuter;
                GameObject thisSlot = transform.GetChild(i).gameObject;
                Button thisBtn = thisSlot.GetComponent<Button>();
                GameManager.instance.slots.Add(thisSlot);
                if (pivot >= sizeOfBoard)
                {
                    tempColumnOuter = 0;
                    tempColumnInner = tempColumnOuter;
                    tempRowOuter++;
                    tempRowInner = tempRowOuter;
                    pivot = 0;
                }
                thisBtn.onClick.AddListener(() =>
                {
                    CreateSymbolByTurn(tempIndex);
                    GetWinner(tempRowInner, tempColumnInner, GetSymbol());
                });
                pivot++;
                tempColumnOuter++;
            }
            isBoardCreated = true;
        }
        else
        {
            SetAllButtonInteractable(true);
        }
    }
    private void UpdateScore()
    {
        if (isCircleTurn)
        {
            GameManager.instance.circleScore++;
            GameManager.instance.scoreFields[0].text = GameManager.instance.circleScore.ToString();
        }
        else
        {
            GameManager.instance.crossScore++;
            GameManager.instance.scoreFields[1].text = GameManager.instance.crossScore.ToString();
        }
        SetAllButtonInteractable(false);
        symbolsContainer = new byte[3, 3];
        turnCount = 0;
    }
    public void CreateSymbolByTurn(int index)
    {
        if(isCircleTurn)
            CreateCircleSymbol(index);
        else
            CreateCrossSymbol(index);
    }
    private void CreateCircleSymbol(int index)
    {
        GameObject thisObj = Instantiate(cirClePrefab,transform.GetChild(index).transform);
        thisObj.transform.parent.GetComponent<Button>().interactable = false;
    }
    private void CreateCrossSymbol(int index)
    {
        GameObject thisObj = Instantiate(crossPrefab, transform.GetChild(index).transform);
        thisObj.transform.parent.GetComponent<Button>().interactable = false;
    }

}
