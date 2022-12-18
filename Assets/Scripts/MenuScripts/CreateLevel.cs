using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    [SerializeField] StartManager startManager;
    [SerializeField] GameObject main_menu;
    [SerializeField] GameObject settings_menu;
    [SerializeField] GameObject levels_menu;
    private void Start()
    {
        main_menu.SetActive(true);
        settings_menu.SetActive(false);
        levels_menu.SetActive(false);
    }
    public void CreateNewLevel()
    {
        MainGame.IndGameLevel = 3;
        startManager.GoToGame();
    }
    public void CreateNewLevel(int number_level)
    {
        MainGame.IndGameLevel=number_level;
        MainGame.GameLevelStart = null;
        MainGame.GameLevelFinish = null;
        startManager.GoToGame();
    }
    public void CreateNewLevel(string startWord, string finishWord)
    {
        MainGame.GameLevelStart = startWord;
        MainGame.GameLevelFinish = finishWord;
        MainGame.IndGameLevel = null;
        startManager.GoToGame();
    }
}
