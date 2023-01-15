using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class ControllerManager : MonoBehaviour
{
    public class Cell
    {
        public static GameObject cells_parent;
        private GameObject cell_obj_parent;
        private Animator anim;
        private string cell_name = "";
        private float cell_position;
        private bool cell_visible = false;
        public Cell(string cell_name, GameObject cell_object,float cell_position, bool withoutTime)
        {
            cell_visible=true;
            //this.cell_obj_orig = cell_object.transform.GetChild(0).gameObject;
            this.cell_obj_parent = cell_object;
            this.cell_position = cell_position;
            this.cell_name = cell_name;
            this.cell_obj_parent.transform.parent = cells_parent.transform;
            this.cell_obj_parent.transform.localPosition = new Vector3(cell_position, 0 ,0);
            cell_object.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>().text = this.cell_name;
            if(!withoutTime) GoCellAnimation(States.cell_instant);
        }
        public Cell(GameObject cell_object, float cell_position, bool withoutTime)
        {
            this.cell_obj_parent = cell_object;
            this.cell_position = cell_position;
            this.cell_obj_parent.transform.parent = cells_parent.transform;
            this.cell_obj_parent.transform.localPosition = new Vector3(cell_position, 0, 0);
            cell_object.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>().text = this.cell_name;
            if(!withoutTime) GoCellAnimation(States.cell_instant);
        }
        public void GoCellAnimation(States states)
        {
            if (anim==null) anim = cell_obj_parent.transform.GetChild(0).GetComponent<Animator>();
            anim.SetTrigger(name: states.ToString());
        }
        public void CellRename(string NewCellName)
        {
            cell_obj_parent.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>().text=NewCellName;
            cell_name = NewCellName;
        }
        public GameObject GetObject()
        {
            if (cell_obj_parent == null || cell_obj_parent.transform.GetChild(0)==null) return null;
            return cell_obj_parent.transform.GetChild(0).gameObject;
        }
        public bool Visible()
        {
            return cell_visible;
        }
        public float GetPosition()
        {
            return cell_position;
        }
    }
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private float _step;
    public static float step=0f;
    public static float bound;
    private const int sizeMap2=200; //%2
    private float x_position=0f;
    public static List<Cell> StageCells= new List<Cell>();
    private bool isGenerated = true;
    private bool setlevel = false;
    private int tet_tet; //local counter
    public static List<Item> Items;
    private char[] StartWord;
    private char[] FinishWord;
    private int level_id;
    private bool withoutTime=false;
    private void Awake()
    {
        bound = 2*gameObjects[0].GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.x;
        step = _step;
    }
    private void Restart()
    {
        StopAllCoroutines();
        while (StageCells.Count != 0)
        {
            Destroy(StageCells[0].GetObject());
            StageCells.RemoveAt(0);
        }
        SetLevel(StartWord.ToString(), FinishWord.ToString());
    }
    
    public void SetLevel(string startWord, string finishWord)
    {
        this.StartWord = startWord.ToCharArray();
        this.FinishWord = finishWord.ToCharArray();
        InitializeLevel();
    }
    private void InitializeLevel()
    {
        Cell.cells_parent = new GameObject("CellsParent");
        Cell.cells_parent.transform.position = new Vector3(0, 0, 0);
        x_position = -100 * (bound + step);
        tet_tet = 0;
        setlevel = true;
        isGenerated = true;
    }
    private void Update()
    {
        if (isGenerated)
        {
            if (setlevel)
            {
                if (tet_tet <= sizeMap2 + StartWord.Length && (tet_tet <= sizeMap2 / 2 - 1 || tet_tet >= sizeMap2 / 2 + StartWord.Length))
                {
                    AddEmptyCell();
                    Time.timeScale = 4 + Math.Abs(tet_tet - sizeMap2 / 2) / (sizeMap2 / 2);
                }
                else if (tet_tet <= sizeMap2 / 2 + StartWord.Length)
                {
                    AddCell(StartWord[tet_tet - sizeMap2 / 2]);
                    Time.timeScale = 1f;
                }
                else
                {
                    setlevel = false;
                    MainGame.SetLevel = false;
                    Time.timeScale = 1f;
                    CellAct?.Invoke(0f);
                    return;
                }
                tet_tet++;
                x_position += (bound + step);
            }
            void AddEmptyCell()
            {
                GameObject cell = generateCells(gameObjects[0]);
                StartCoroutine(IsThisCoordinateY(cell.transform));
                StageCells.Add(new Cell(cell, x_position,withoutTime));
            }
            void AddCell(char name)
            {
                GameObject cell = generateCells(gameObjects[0]);
                StartCoroutine(IsThisCoordinateY(cell.transform.GetChild(0)));
                StageCells.Add(new Cell(name.ToString(), cell, x_position, withoutTime));
                CellAct?.Invoke(x_position);
            }
        }
    }

    private GameObject generateCells(GameObject prefab)
    {
        if (withoutTime) return Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        return Instantiate(prefab, new Vector3(0, 1, 0), Quaternion.identity);
    }
    

    IEnumerator IsThisCoordinateY(Transform coordY)
    {
        this.isGenerated = false;
        while (coordY.position.y > 0f)
        {
            yield return null;
        }
        this.isGenerated = true;

    }
    public bool CheckCorrectWord()
    {
        int isOneWord = 0;
        string finishWord = "";

        for (int i = 1; i < StageCells.Count; i++)
        {
            Cell cell = StageCells[i];
            if (cell.GetObject() != null)
            {
                string letter= cell.GetObject().transform.GetChild(0).GetComponent<TextMeshPro>().text;
                if (letter != ""&& StageCells[i-1].GetObject().transform.GetChild(0).GetComponent<TextMeshPro>().text=="") 
                { isOneWord++; }
                finishWord += letter;
            }
        }
        string _finishWord="";
        foreach (char s in FinishWord)
        {
            _finishWord+=s;
        }
        if (isOneWord == 1&& finishWord==_finishWord)
        {
            return true;
        }
        Debug.Log(finishWord);
        Debug.Log(_finishWord);
        return false;
    }
    //Controller HeadMachine
    private Cell InitializeCell(bool isTextEmpty, GameObject gameObject)
    {
        for (int i=0; i < StageCells.Count; i++)
        {
            Cell cell = StageCells[i];
            if (isTextEmpty || cell.Visible())
            {
                if (cell.GetObject()!=null &&gameObject == cell.GetObject())
                {
                    return cell;
                }
            }
        }
        return null;
    }
    private Cell cell;
    public static Action<Cell> GetCell;
    private void CellHit(GameObject collider)
    {
        this.cell = InitializeCell((collider.transform.GetChild(0).GetComponent<TextMeshPro>().text == ""), collider);
        GetCell?.Invoke(cell);
    }
    private void EventTracking(GameMode _event)
    {
        switch(_event)
        {
            case GameMode.Restart:
                withoutTime = false;
                Restart();
                break;
            case GameMode.RestartEcxept:
                withoutTime = true;
                Restart();
                break;
        }
    }
    private void OnEnable()
    {
        CellController.onTouched += CellHit; //sobutie
        MainGame.MainGameMode += EventTracking;
    }
    private void OnDisable()
    {
        CellController.onTouched -= CellHit;
        MainGame.MainGameMode -= EventTracking;
    }
    public static Action<float> CellAct;
}

public enum States
{
    cell_instant,
    cell_rename,
    cell_state
}