using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    [SerializeField] StationContent stationcontent;
    [SerializeField] ControllerManager controllerManager;
    [SerializeField] Button buttonPauseContinue;
    [SerializeField] Button buttonPlayGame;
    [SerializeField] Button buttonOneStep;
    [SerializeField] GameObject partishion;
    public static bool IsPlaying = false;
    public static bool SetLevel = true;
    private int amountCellStateAnimations=0;
    public static Action<int> MovingCells;
    private StationContent.Base Base;
    private int ind_state=0;
    private int ind_symbol=0;
    private string stationChosenName="Q1";
    private ControllerManager.Cell cell;
    private bool getcell=false;
    private void GetTableStates(StationContent.Base @base)
    {
        Base= @base;
    }


    private void Start()
    {
        buttonPauseContinue.onClick.AddListener(()=> { Pause(); });
        buttonOneStep.onClick.AddListener(() => { Continue(); stationcontent.OnReceivedStations(); StationContent.ActGame(); IsPlaying = true;  if(!SetLevel) OneStep(stationChosenName); });
        buttonPlayGame.onClick.AddListener(()=> { Continue(); /*if (stationcontent.gameObject.activeSelf) partishion.SetActive(true); */});
    }

    private void OnApplicationPause(bool pause)
    {
        Pause();
    }
    private void OnEnable()
    {
        StationContent.onBase += GetTableStates;
        OnAnimationOver.AnimationOver += AnimationOver;
        ControllerManager.GetCell += GetCell;
    }
    private void OnDisable()
    {
        StationContent.onBase -= GetTableStates;
        OnAnimationOver.AnimationOver -= AnimationOver;
        ControllerManager.GetCell -= GetCell;
    }
    private void GetCell(ControllerManager.Cell cell)
    {
        this.cell = cell;
        getcell = true;
    }
    private void AnimationOver(States cellAnimationState)
    {
        if (cellAnimationState == States.cell_rename)
        {
            if (overwrite_symbol!="")
            cell.CellRename(overwrite_symbol);
            cell.GoCellAnimation(States.cell_state);
        }
        if (cellAnimationState == States.cell_state)
        {
            amountCellStateAnimations++;
            if (amountCellStateAnimations == 1)
            {
                cell.GoCellAnimation(States.cell_rename);
            }
            if (amountCellStateAnimations == 2)
            {
                amountCellStateAnimations = 0;
                switch (symbolTable[2])
                {
                    case "L": MoveCells(-1); break;
                    case "R": MoveCells(1); break;
                    case "!": FinishGame(); break;
                }
                this.ind_state = next_stateIndex;
                this.ind_state = overwrite_symbolIndex;
                stationChosenName = next_state;
                getcell = false;
            }
        }
    }
    int next_stateIndex;
    string next_state;
    string overwrite_symbol;
    int overwrite_symbolIndex;
    string name_symbol;
    string[] symbolTable;

    private void OneStep(string searchedState)
    {
        if (getcell)
        {
            name_symbol = this.cell.GetObject().transform.GetChild(0).GetComponent<TextMeshPro>().text;
            StationContent.Base.Table table = Base.SearchSymbol(searchedState, name_symbol, ind_state, ind_symbol);
            if (table != null)
            {
                next_stateIndex = table.state_index;
                next_state = table.ReturnStateByIndex(next_stateIndex);
                overwrite_symbolIndex = table.symbol_index;
                symbolTable = table.ReturnSymbolByIndex(overwrite_symbolIndex);
                overwrite_symbol = symbolTable[1];
                this.cell.GoCellAnimation(States.cell_state);
            }
            else
            {
                stationcontent.OnReceivedStations();
            }
        }
    }
    private void MoveCells(int vector)
    {
        MovingCells?.Invoke(vector);
    }
    private void FinishGame()
    {
        Debug.Log("Finish Game");
    }
    private void Pause()
    {
        Debug.Log("Pause");
    }
    private void Continue()
    {
        Debug.Log("Continue");
    }
    private void PlayGame()
    {
        Debug.Log("Play Game");
    }
    
}
