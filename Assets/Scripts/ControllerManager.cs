using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class ControllerManager : MonoBehaviour
{
    public class PullObject
    {
        private List<GameObject> objects = new List<GameObject>();
        private GameObject prefab;
        private Transform parent;
        private int LastVoiceCount = -1;
        private int stock;
        public PullObject(int countObjects, GameObject prefab, int stock)
        {
            this.prefab = prefab;
            this.stock=stock;
            GameObject parent;
            parent = new GameObject($"PullParent{prefab.name}");
            parent.transform.position = new Vector3(0, -100, 0);
            this.parent = parent.transform;
            parent.gameObject.SetActive(false);
            InstantiateObjects(countObjects);
        }
        private void InstantiateObjects(int countObjects)
        {
            for (int i = 0; i < countObjects; i++)
            {
                GameObject instance = Instantiate(prefab) as GameObject;
                instance.transform.SetParent(parent, false);
                objects.Add(instance);
            }
        }
        public GameObject GetAnotherObject()
        {
            if (LastVoiceCount+stock > objects.Count)
            {
                InstantiateObjects(stock);
            }
            LastVoiceCount++;
            return objects[LastVoiceCount];
        }
        public GameObject GetAnotherObject(Transform parent)
        {
            GameObject obj = GetAnotherObject();
            obj.transform.SetParent(parent,false);
            return obj;
        }
        public void ResetVoiceCount()
        {
            for(int i=0; i< LastVoiceCount+1; i++)
            {
                objects[i].transform.SetParent(parent, false);
            }
            LastVoiceCount = -1;
        }
    }
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
            //this.cell_obj_parent.transform.parent = cells_parent.transform;
            this.cell_obj_parent.transform.localPosition = new Vector3(cell_position, 0 ,0);
            cell_object.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>().text = this.cell_name;
            if(!withoutTime) GoCellAnimation(States.cell_instant);
        }
        public Cell(GameObject cell_object, float cell_position, bool withoutTime)
        {
            this.cell_obj_parent = cell_object;
            this.cell_position = cell_position;
            //this.cell_obj_parent.transform.parent = cells_parent.transform;
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
    [SerializeField] private GameObject cell_prefab;
    [SerializeField] private float _step;
    public static float step=0f;
    [SerializeField] private float _bound;
    public static float bound;
    private const int sizeMap=40;
    private const int fieldOfVision = 2;
    private float x_position=0f;
    public static List<Cell> StageCells= new List<Cell>();
    private bool isGenerated = true;
    private int tet_tet; //local counter
    public static List<Item> Items;
    private char[] StartWord;
    private char[] FinishWord;

    private bool withoutTime=false;
    [SerializeField] Transform loading;
    ControllerManager.PullObject pull_cells;

    private void Awake()
    {
        bound = _bound;
        step = _step;
    }
    private void Start()
    {
        loading.gameObject.SetActive(false);
        pull_cells = new ControllerManager.PullObject(120, cell_prefab , 20);
    }
    private void Restart()
    {
        ClearTape();
        SetLevel();
    }
    private void ClearTape()
    {
        StopAllCoroutines();
        StageCells.Clear();
        pull_cells.ResetVoiceCount();
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
        loading.gameObject.SetActive(true);
        isGenerated = true;
        OnAnimationOver.AnimationOver += AnimationOver;
        StartCoroutine(GeneratedLine());
    }

    private void EmptyCellToEnd(char? name,bool toEnd=true, bool animation = false)
    {
        GameObject cell = pull_cells.GetAnotherObject(Cell.cells_parent.transform);
        cell.transform.position=new Vector3(vector(), 0, 0);
        if (!toEnd)
        {
            StageCells.Insert(0, new Cell(cell, cell.transform.position.x, true));
        }
        else if (!animation)
        {
            if (name == null) StageCells.Add(new Cell(cell, cell.transform.position.x, true));
            else
            {
                isGenerated = false;
                StageCells.Add(new Cell(name.ToString(), cell, cell.transform.position.x, false));
                //StartCoroutine(IsThisCoordinateY(cell.transform.GetChild(0)));
                CellAct?.Invoke(StageCells[StageCells.Count - 1].GetPosition());
            }
        }
        else
        {
            isGenerated = false;
            StageCells.Add(new Cell(cell, cell.transform.position.x, false));
            //StartCoroutine(IsThisCoordinateY(cell.transform.GetChild(0)));

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
                GameObject cell = pull_cells.GetAnotherObject();
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
                isGenerated = false;
                EmptyCellToEnd(null, true, true);
                tet_tet++;
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
                    isGenerated = false;
                    EmptyCellToEnd(null, true, true);
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
        loading.gameObject.SetActive(false);
        OnAnimationOver.AnimationOver -= AnimationOver;
    }

    public bool CheckCorrectWord()
    {
        int isOneWord = 0;
        int isTwoWord = 0;
        string finishWord = "";

        for (int i = 1; i < Cell.cells_parent.transform.childCount; i++)
        {
            Transform cell = Cell.cells_parent.transform.GetChild(i);
            Transform cell_before = Cell.cells_parent.transform.GetChild(i-1);
            if (cell != null)
            {
                string letter= cell.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text;
                if ((letter != "" && letter!= null )&&
    (cell_before.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text=="" || cell_before.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text == null))
                { isOneWord++; }
                finishWord += letter;
            }
        }
        Debug.Log(finishWord+"FINISH WORD");
        string _finishWord="";
        if (FinishWord[0] != '_') _finishWord += FinishWord[0];
        for (int i = 1; i < FinishWord.Length; i++)
        {
            if (FinishWord[i]=='_' && FinishWord[i- 1] != '_')
            {
                isTwoWord += 1;
            }
            else if (FinishWord[i] != '_') _finishWord += FinishWord[i];
        }
        if(FinishWord.Length > 0 && FinishWord[FinishWord.Length-1] !='_') { isTwoWord += 1; }
        
        if (isOneWord == isTwoWord && (finishWord == _finishWord))
        {
            return true;
        }
        return false;
    }
    //Controller HeadMachine
    //
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
    //
    //private Cell cell;
    public static Action<GameObject> GetCell;
    private void CellHit(GameObject collider)
    {
        //this.cell = InitializeCell((collider.transform.GetChild(0).GetComponent<TextMeshPro>().text == ""), collider);
        GetCell?.Invoke(collider);
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
    private void AnimationOver(States state)
    {
        if (state == States.cell_instant)
        {
            isGenerated = true;
        }
    }
    private void OnEnable()
    {
        CellController.onTouched += CellHit; //sobutie
        MainGame.MainGameMode += EventTracking;
        CellController.AddCellRight += AddCell;
        OnAnimationOver.AnimationOver += AnimationOver;
    }
    private void OnDisable()
    {
        CellController.onTouched -= CellHit;
        MainGame.MainGameMode -= EventTracking;
        CellController.AddCellRight -= AddCell;
        OnAnimationOver.AnimationOver -= AnimationOver;
    }
    public static Action<float> CellAct;
}

public enum States
{
    cell_instant,
    cell_rename,
    cell_state
}