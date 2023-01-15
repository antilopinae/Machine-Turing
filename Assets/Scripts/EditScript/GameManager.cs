using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Item> Items;
    private void Awake()
    {
        ControllerManager.Items=Items;
    }
}
[System.Serializable]
public class Item
{
    public int Id;
    public string Test1_StartWord;
    public string Test1_FinishWord;
    public string Test2_StartWord;
    public string Test2_FinishWord;
    public string Test3_StartWord;
    public string Test3_FinishWord;
    public string ABC;
}
