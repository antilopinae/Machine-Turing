using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    private int level_id;
    private List<string[][]> symbols;
    private List<string> states;
       
    private void Awake()
    {
        if (MainGame.IndGameLevel != null)
        {
            level_id = (int)MainGame.IndGameLevel;
            DataTable table = MyDataBase.GetTable($"SELECT * FROM USER_SOLUTIONS WHERE LEVEL_ID = {level_id};");
            if (table.Rows.Count!= 0)
            {
                states = JsonConvert.DeserializeObject<List<string>>(table.Rows[0][1].ToString());
                symbols = JsonConvert.DeserializeObject<List<string[][]>>(table.Rows[0][2].ToString());
            }
        }

    }
    public List<string> GetStates()
    {
        if (MainGame.IndGameLevel != null)
        {
            level_id = (int)MainGame.IndGameLevel;
            DataTable table = MyDataBase.GetTable($"SELECT * FROM USER_SOLUTIONS WHERE LEVEL_ID = {level_id};");
            if (table.Rows.Count != 0)
            {
                return states = JsonConvert.DeserializeObject<List<string>>(table.Rows[0][1].ToString());
            }
        }
        return null;
    }
    public List<string[][]> GetSymbols()
    {
        if (MainGame.IndGameLevel != null)
        {
            level_id = (int)MainGame.IndGameLevel;
            DataTable table = MyDataBase.GetTable($"SELECT * FROM USER_SOLUTIONS WHERE LEVEL_ID = {level_id};");
            if (table.Rows.Count != 0)
            {
                return symbols = JsonConvert.DeserializeObject<List<string[][]>>(table.Rows[0][2].ToString());
            }
        }
        return null;
    }
    public void InputStatesAndSymbols(List<string> states, List<string[][]> symbols)
    {
        string str_states = JsonConvert.SerializeObject(states);
        string str_symbols = JsonConvert.SerializeObject(symbols);
        Debug.Log(str_states);
        Debug.Log(str_symbols);
        Debug.Log("Bug");
        Debug.Log(states.ToArray());
        Debug.Log(symbols.ToArray());
        if (MyDataBase.GetTable($"SELECT * FROM USER_SOLUTIONS WHERE LEVEL_ID = {level_id};").Rows.Count == 0)
        {
            MyDataBase.ExecuteQueryWithoutAnswer($"INSERT INTO USER_SOLUTIONS(LEVEL_ID, USER_STATES, USER_SYMBOLS) VALUES('{level_id}', '{str_states}', '{str_symbols}')");
            Debug.Log("INSERT SAVE");
        }
        else
        {
            MyDataBase.ExecuteQueryWithoutAnswer($"UPDATE USER_SOLUTIONS SET USER_STATES = ‘{str_states}’, USER_SYMBOLS = ‘{str_symbols}’ WHERE LEVEL_ID = {level_id}");
            Debug.Log("UPDATE SAVE");
        }
    }

}
