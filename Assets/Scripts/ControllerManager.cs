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
            cell_obj_parent.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>().text = NewCellName;
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
    [SerializeField] private float _bound;
    public static float bound;
    private const int sizeMap=100;
    private const int fieldOfVision = 10;
    private float x_position=0f;
    public static List<Cell> StageCells= new List<Cell>();
    private bool isGenerated = true;
    private int tet_tet; //local counter
    public static List<Item> Items;
    private char[] StartWord;
    private char[] FinishWord;
    private int level_id;
    private bool withoutTime=false;
    private void Awake()
    {
        //bound = 2*gameObjects[0].GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.x;
        bound = _bound;
        step = _step;
    }
    private void Restart()
    {
        ClearTape();
        SetLevel();
    }
    private void ClearTape()
    {
        StopAllCoroutines();
        while (StageCells.Count != 0)
        {
            Destroy(StageCells[0].GetObject());
            StageCells.RemoveAt(0);
        }
        Destroy(Cell.cells_parent);
    }
    
    public void SetLevel(string startWord, string finishWord)
    {
        this.StartWord = startWord.ToCharArray();
        this.FinishWord = finishWord.ToCharArray();
        InitializeLevel();
    }
    private void SetLevel()
    {
        InitializeLevel();
    }
    private void InitializeLevel()
    {
        Cell.cells_parent = new GameObject("CellsParent");
        Cell.cells_parent.transform.position = new Vector3(0, 0, 0);
        tet_tet = 0;
        StartCoroutine(GeneratedLine());
    }

    private void EmptyCellToEnd(char? name,bool toEnd=true, bool animation = false)
    {
        GameObject cell = Instantiate(gameObjects[0], new Vector3(vector(), 0, 0), Quaternion.identity);
        if(!toEnd)
        {
            Cell obj = new Cell(cell, cell.transform.position.x, true);
            List<Cell> objects = new List<Cell>();
            objects.Add(obj);
            //StageCells = objects.Concat(StageCells);
            //StageCells.AddRange(obj+StageCells);
        }
        else if (!animation)
        {
            if (name == null) StageCells.Add(new Cell(cell, cell.transform.position.x, true));
            else
            {
                StageCells.Add(new Cell(name.ToString(), cell, cell.transform.position.x, false));
                StartCoroutine(IsThisCoordinateY(cell.transform.GetChild(0)));
                CellAct?.Invoke(StageCells[StageCells.Count - 1].GetPosition());
            }
        }
        else
        {
            StageCells.Add(new Cell(cell, cell.transform.position.x, false));
            StartCoroutine(IsThisCoordinateY(cell.transform.GetChild(0)));
        }
        float vector()
        {
            if(toEnd) { return StageCells[StageCells.Count - 1].GetPosition() + (bound + step); }
            else { return StageCells[0].GetPosition() - (bound + step); }
        }
    }
    IEnumerator GeneratedLine()
    {
        while (tet_tet < 2*sizeMap + StartWord.Length)
        {
            if (tet_tet == 0)
            {
                x_position = -sizeMap * (bound + step);
                GameObject cell = Instantiate(gameObjects[0], new Vector3(0, 0, 0), Quaternion.identity);
                StageCells.Add(new Cell(cell, x_position, withoutTime));
                tet_tet++;
                isGenerated = true;
            }
            else if (tet_tet < sizeMap - fieldOfVision)
            {
                EmptyCellToEnd(null);
                tet_tet++;
            }
            else if (tet_tet < sizeMap)
            {
                if (isGenerated)
                {
                    isGenerated = false;
                    EmptyCellToEnd(null,true, true);
                    tet_tet++;
                }
            }
            else if (tet_tet < sizeMap + StartWord.Length)
            {
                if (isGenerated)
                {
                    isGenerated = false;
                    EmptyCellToEnd(StartWord[tet_tet - sizeMap]);
                    tet_tet++;
                }
            }
            else if (tet_tet < sizeMap + StartWord.Length + fieldOfVision)
            {
                if (isGenerated)
                {
                    EmptyCellToEnd(null,true, true);
                    isGenerated = false;
                    tet_tet++;
                }
            }
            else
            {
                EmptyCellToEnd(null);
                tet_tet++;
            }
            yield return null;
        }
        MainGame.SetLevel = false;
        CellAct?.Invoke(0f);
    }
    IEnumerator IsThisCoordinateY(Transform coordY)
    {
        this.isGenerated = false;
        while (coordY.position.y > 0.00001f)
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
                if ((letter != "" && letter!= null )&& (StageCells[i-1].GetObject().transform.GetChild(0).GetComponent<TextMeshPro>().text=="" | StageCells[i - 1].GetObject().transform.GetChild(0).GetComponent<TextMeshPro>().text == null)) 
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
                //withoutTime = false;
                Restart();
                break;
            case GameMode.RestartEcxept:
                //withoutTime = true;
                Restart();
                break;
            case GameMode.ClearAndExit: ClearTape(); break;
        }
    }
    private void AddCell(bool right)
    {
        //for (int i=0; i<10; i++)
        EmptyCellToEnd(null, right);
    }
    private void OnEnable()
    {
        CellController.onTouched += CellHit; //sobutie
        MainGame.MainGameMode += EventTracking;
        CellController.AddCellRight += AddCell;
    }
    private void OnDisable()
    {
        CellController.onTouched -= CellHit;
        MainGame.MainGameMode -= EventTracking;
        CellController.AddCellRight -= AddCell;
    }
    public static Action<float> CellAct;
}

public enum States
{
    cell_instant,
    cell_rename,
    cell_state
}