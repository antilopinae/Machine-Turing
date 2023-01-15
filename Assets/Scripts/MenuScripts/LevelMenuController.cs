using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelMenuController : MonoBehaviour
{
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] GameObject[] panels;
    private int numberActivePanel=0;
    private int lengthPanels;
    void Start()
    {
        lengthPanels = panels.Length;
        SetActivePanel(numberActivePanel);
        leftButton.onClick.AddListener(() => { goLeft(); });
        rightButton.onClick.AddListener(() => { goRight(); });
    }
    private void goRight()
    {
        if (numberActivePanel + 1 < lengthPanels)
        {
            numberActivePanel++;
            SetActivePanel(numberActivePanel);
            Debug.Log(numberActivePanel);
        }
    }
    private void goLeft()
    {
        if (numberActivePanel - 1 >= 0)
        {
            numberActivePanel--;
            SetActivePanel(numberActivePanel);
            Debug.Log(numberActivePanel);
        }
    }
    private void SetActivePanel(int k)
    {
        for (int i = 0; i < lengthPanels; i++)
        {
            panels[i].SetActive(i==k);
        }
    }
}
