using System.Data;
using System;
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
        MainGame.IndGameLevel = GetDataBase();
        startManager.GoToGame();
    }
    private void UpdateDataBase(int id)
    {
        DataTable table = MyDataBase.GetTable($"SELECT * FROM LAST_LEVEL WHERE FIELD = 0;");
        if (table.Rows.Count == 0)
        {
            MyDataBase.ExecuteQueryWithoutAnswer($"INSERT INTO LAST_LEVEL(FIELD, LAST_LEVEL_ID) VALUES('0', '{id}')");
        }
        else
        {
            MyDataBase.ExecuteQueryWithoutAnswer($"UPDATE LAST_LEVEL SET LAST_LEVEL_ID = '{id}' WHERE FIELD = 0");
        }
    }
    private int GetDataBase()
    {
        DataTable table = MyDataBase.GetTable("SELECT * FROM LAST_LEVEL WHERE FIELD = 0;");
        if (table.Rows.Count != 0)
        {
            string str = table.Rows[0][1].ToString();
            Debug.Log(str);
            return Convert.ToInt32(str);
        }
        else
        {
            return 1;
        }
    }
    public void CreateNewLevel(int number_level)
    {
        MainGame.IndGameLevel=number_level;
        MainGame.GameLevelStart = null;
        MainGame.GameLevelFinish = null;
        UpdateDataBase(number_level);
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
