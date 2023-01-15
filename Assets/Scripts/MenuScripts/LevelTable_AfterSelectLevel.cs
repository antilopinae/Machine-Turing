using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelTable_AfterSelectLevel : MonoBehaviour
{
    [SerializeField] Sprite[] images;
    [SerializeField] CreateLevel createlevel;
    [SerializeField] GameObject TableLevel;
    private Image background;
    private int level;
    
    private void Start()
    {
        TableLevel.SetActive(false);
        background = TableLevel.transform.GetChild(1).GetComponent<Image>();
    }
    public void SelectLevel(int level)
    {
        TableLevel.SetActive(true);
        background.sprite = images[level-1];
        this.level = level;
    }
    public void GoToLevel()
    {
        createlevel.CreateNewLevel(level);
    }
    public void Exit()
    {
        TableLevel.SetActive(false);
    }
}
