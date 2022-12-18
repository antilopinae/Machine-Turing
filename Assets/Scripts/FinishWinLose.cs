using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishWinLose : MonoBehaviour
{
    [SerializeField] ControllerManager controller;
    [SerializeField] GameObject PanelStat;
    private GameObject panel_win;
    private GameObject panel_lose;
    private void Start()
    {
        PanelStat.SetActive(false);
        panel_win= PanelStat.transform.GetChild(0).gameObject;
        panel_lose = PanelStat.transform.GetChild(1).gameObject;
    }
    private void Finish(GameMode _event)
    {
        if (_event== GameMode.Finish) 
        {
            if (controller.CheckCorrectWord())
            {
                PanelStat.SetActive(true);
                panel_lose.SetActive(false);
                panel_win.SetActive(true);
            }
            else
            {
                PanelStat.SetActive(true);
                panel_lose.SetActive(true);
                panel_win.SetActive(false);
            }

        }
        else
        {
            PanelStat.SetActive(false);
        }
    }
    private void OnEnable()
    {
        MainGame.MainGameMode += Finish;
    }
    private void OnDisable()
    {
        MainGame.MainGameMode -= Finish;
    }
}
