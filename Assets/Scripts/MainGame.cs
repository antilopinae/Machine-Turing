using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    //обработка игровых движений из скриптов Station Content и Controller Manager
    [SerializeField] StationContent stationcontent;
    [SerializeField] ControllerManager controllerManager;
    [SerializeField] Button buttonPause;
    [SerializeField] Button buttonContinue;
    [SerializeField] Button buttonOneStep;
    public static bool IsPlaying = false;
    public static bool SetLevel = true;
    private StationContent.Base Base;
    private int ind_state=0;
    private int ind_symbol=0;
    private void GetTableStates(StationContent.Base @base)
    {
        Base= @base;
    }
    private void Start()
    {
        buttonPause.onClick.AddListener(()=> { Pause(); });
        buttonOneStep.onClick.AddListener(() => {OneStep(0,"Base"); });
        buttonContinue.onClick.AddListener(()=> { Continue(); });
    }

    private void OnApplicationPause(bool pause)
    {
        Pause();
    }
    private void OnEnable()
    {
        StationContent.onBase += GetTableStates;
    }
    private void OnDisable()
    {
        StationContent.onBase -= GetTableStates;
    }
    private void OneStep(int VectorMachine, string searchedState)
    {
        string next_state;
        int newVectorMachine;
        string overwrite_symbol;
        string name_symbol = this.controllerManager.GetCell().GetObject().transform.GetChild(0).GetComponent<TextMeshPro>().text;
        int ind_state = this.ind_state;
        int ind_symbol = this.ind_symbol;
        StationContent.Base.Table table = Base.SearchSymbol(searchedState, name_symbol, ind_state, ind_symbol);
        next_state=table.ReturnStateByIndex(table.state_index);
        overwrite_symbol = table.ReturnSymbolByIndex(table.symbol_index)[0];
        switch (table.ReturnSymbolByIndex(table.symbol_index)[1]) {
            case "L": newVectorMachine=-1; break;
            case "R": newVectorMachine=1; break;
            case "!": newVectorMachine=0; break;
        }

    }
    private IEnumerator IEOneStep()
    {

        yield return null;
    }
    private void Pause()
    {

    }
    private void Continue()
    {

    }
    private void PlayGame()
    {

    }
    
}
