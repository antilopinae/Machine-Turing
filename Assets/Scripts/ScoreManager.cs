using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
                return states = ConvertState(table.Rows[0][1].ToString());
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
                return symbols = ConvertSymbol(table.Rows[0][2].ToString());
            }
        }
        return null;
    }
    public void InputStatesAndSymbols(List<string> states, List<string[][]> symbols)
    {
        //string str_states = JsonConvert.SerializeObject(states).ToString();
        //string str_symbols = JsonConvert.SerializeObject(symbols).ToString();
        string str_states = ConvertList(states);
        string str_symbols= ConvertList(symbols);
        Debug.Log(str_states);
        Debug.Log(str_symbols);
        Debug.Log("Bug");
        Debug.Log(states.ToArray());
        Debug.Log(symbols.ToArray());
        if (MyDataBase.GetTable($"SELECT * FROM USER_SOLUTIONS WHERE LEVEL_ID = {level_id};").Rows.Count == 0)
        {
            MyDataBase.ExecuteQueryWithoutAnswer($"INSERT INTO USER_SOLUTIONS(LEVEL_ID, USER_STATES, USER_SYMBOLS) VALUES('{level_id}', '{str_states}', '{str_symbols}');");
            Debug.Log("INSERT SAVE");
        }
        else
        {
            Debug.Log($"UPDATE USER_SOLUTIONS SET USER_STATES = '{str_states}', USER_SYMBOLS = '{str_symbols}' WHERE LEVEL_ID = '{level_id}'");
            MyDataBase.ExecuteQueryWithoutAnswer($"UPDATE USER_SOLUTIONS SET USER_STATES = '{ str_states}', USER_SYMBOLS ='{str_symbols}' WHERE LEVEL_ID = '{level_id}'");
            Debug.Log("UPDATE SAVE");
        }
    }
    private string ConvertList(List<string[][]> symbols)
    {
        string str = "";
        for(int i=0; i<symbols.Count;i++)
        {
            for (int j=0; j < symbols[i].Length; j++)
            {
                for (int k=0; k < symbols[i][j].Length; k++)
                {
                    str += symbols[i][j][k]+"*";
                }
                str += "#";
            }
            str += "@";
        }
        return str;
    }
    private string ConvertList(List<string> states)
    {
        string str = "";
        for (int i = 0; i < states.Count; i++)
        {
            str += states[i].ToString()+"*";
        }
        return str;
    }
    private List<string> ConvertState(string str)
    {
        List<string> states = new List<string>();
        string t = "";
        for(int i=0; i < str.Length;i++)
        {
            if (str[i] != '*') { t += str[i]; }
            else { states.Add(t); t = ""; }
        }
        Debug.Log(states);
        return states;
    }
    private List<string[][]> ConvertSymbol(string str)
    {
        
        List<string[][]> symbol = new List<string[][]>();
        string t = "";
        string[] arr2 = new string[4];
        //string[][] arr1 = new string[3][];
        List<string[]> arr1= new List<string[]>();
        int count_arr2 = 0;
        int count_arr1 = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] =='*')
            {
                arr2[count_arr2] = t;
                count_arr2++;
                t = "";
            }
            else if (str[i] == '#')
            {
                arr1.Add(arr2);
                count_arr1++;
                count_arr2 = 0;
                arr2 = new string[4];
                t = "";
            }
            else if (str[i] == '@')
            {
                symbol.Add(arr1.ToArray <string[]>());
                arr1.Clear();
                count_arr1= 0;
                t = "";
            }
            else if (str[i]!='@'&& str[i] != '#' && str[i]!='*')
            {
                t += str[i];
            }
        }
        Debug.Log(symbol.ToString());
        return symbol;
    }
}
