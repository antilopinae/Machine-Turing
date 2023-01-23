using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishWinLose : MonoBehaviour
{
    [SerializeField] private ControllerManager controller;
    [SerializeField] private GameObject PanelFinish;
    [SerializeField] private Sprite panelFinishSucess;
    [SerializeField] private Sprite panelFinishLose;
    private Button but_panel;
    private Image im_panel;

    private void Start()
    {
        PanelFinish.SetActive(false);
        but_panel = PanelFinish.transform.GetChild(0).GetComponent<Button>();
        im_panel = PanelFinish.transform.GetChild(0).GetComponent<Image>();
    }
    private void Finish(GameMode _event)
    {
        if (_event== GameMode.Finish) 
        {
            if (controller.CheckCorrectWord())
            {
                but_panel.onClick.RemoveAllListeners();
                im_panel.sprite = panelFinishSucess;
                PanelFinish.SetActive(true);
                but_panel.onClick.AddListener(() => { but_panel.onClick.RemoveAllListeners(); PanelFinish.SetActive(false); });
            }
            else
            {
                but_panel.onClick.RemoveAllListeners();
                im_panel.sprite = panelFinishLose;
                PanelFinish.SetActive(true);
                but_panel.onClick.AddListener(() => { but_panel.onClick.RemoveAllListeners(); PanelFinish.SetActive(false); });
            }

        }
        else if (_event == GameMode.Ecxeption)
        {
            but_panel.onClick.RemoveAllListeners();
            im_panel.sprite = panelFinishLose;
            PanelFinish.SetActive(true);
            but_panel.onClick.AddListener(() => { but_panel.onClick.RemoveAllListeners(); PanelFinish.SetActive(false); });
        }
        else
        {
            PanelFinish.SetActive(false);
        }
    }

    private void OnEnable()
    {
        MainGame.MainGameMode += Finish;
        PanelFinish.SetActive(false);
    }
    private void OnDisable()
    {
        MainGame.MainGameMode -= Finish;
        PanelFinish.SetActive(false);
    }
}
