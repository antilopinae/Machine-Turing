using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static ControllerManager;
using Unity.VisualScripting;

public class ControllerManager : MonoBehaviour
{
    public class Cell
    {
        public static GameObject cells_parent;
        //private GameObject cell_obj_orig;
        private GameObject cell_obj_parent;
        private Animator anim;
        private string cell_name="";
        private float cell_position;
        private bool cell_visible=false;
        public Cell(string cell_name, GameObject cell_object,float cell_position)
        {
            cell_visible=true;
            //this.cell_obj_orig = cell_object.transform.GetChild(0).gameObject;
            this.cell_obj_parent = cell_object;
            this.cell_position = cell_position;
            this.cell_name = cell_name;
            this.cell_obj_parent.transform.parent = cells_parent.transform;
            this.cell_obj_parent.transform.localPosition = new Vector3(cell_position, 0 ,0);
            cell_object.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>().text = this.cell_name;
            Animator(true);
        }
        public Cell(GameObject cell_object, float cell_position)
        {
            this.cell_obj_parent = cell_object;
            this.cell_position = cell_position;
            this.cell_obj_parent.transform.parent = cells_parent.transform;
            this.cell_obj_parent.transform.localPosition = new Vector3(cell_position, 0, 0);
            cell_object.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>().text = this.cell_name;
            Animator(true);
        }
        public void Animator(bool instant)
        {
            if (anim==null) anim = cell_obj_parent.transform.GetChild(0).GetComponent<Animator>();
            if (instant) StateCell = States.cell_instant;
            else StateCell = States.cell_change;
        }
        public GameObject GetObject()
        {
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
        private States StateCell
        {
            get { return (States)anim.GetInteger("state"); }
            set { anim.SetInteger("state", (int)value); }
        }
    }
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private Button button;
    [SerializeField] float step;
    private float bound;
    private const int sizeMap2=200; //%2
    private float x_position=0f;
    public static List<Cell> StageCells= new List<Cell>();
    private bool isGenerated = true;
    private bool setlevel = false;
    private int tet_tet; //local counter
    private char namecell;
    public static List<Item> Items;
    private char[] StartWord;
    private char[] FinishWord;
    private int level_id;

    private void Start()
    {
        bound = gameObjects[0].GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.x;
        button.onClick.AddListener(() => { ; });
        SetLevel(1);
    }
    private void SetLevel(int id)
    {
        level_id = id;
        foreach (Item item in Items)
        {
            if (item.Id == id)
            {
                this.StartWord = item.StartWord.ToCharArray();
                this.FinishWord = item.FinishWord.ToCharArray();
            }
        }
        Cell.cells_parent = new GameObject("CellsParent");
        Cell.cells_parent.transform.position = new Vector3(0, 0, 0);
        x_position = -100 * (bound + step);
        tet_tet = 0;
        setlevel = true;
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
                }
                else if (tet_tet <= sizeMap2 / 2 + StartWord.Length)
                {
                    AddCell(StartWord[tet_tet - 100]);
                    Time.timeScale = 1 + Math.Abs(tet_tet - sizeMap2 / 2) / (sizeMap2 / 2);
                }
                else
                {
                    setlevel = false; MainGame.SetLevel = false;
                    Time.timeScale = 1f;
                    return;
                }
                tet_tet++;
                x_position += (bound + step);
            }

            
            void AddEmptyCell()
            {
                GameObject cell = generateCells(gameObjects[1]);
                StartCoroutine(IsThisCoordinateY(cell.transform));
                StageCells.Add(new Cell(cell, x_position));
            }
            void AddCell(char name)
            {
                GameObject cell = generateCells(gameObjects[1]);
                StartCoroutine(IsThisCoordinateY(cell.transform.GetChild(0)));
                StageCells.Add(new Cell(name.ToString(), cell, x_position));
                //Debug.Log(cell.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("cell_instant"));
                CellAct?.Invoke(x_position);
            }
        }
        
    }

    private GameObject generateCells(GameObject prefab)
    {
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
    //Controller HeadMachine
    private Cell InitializeCell(bool visible, GameObject gameObject)
    {
        for (int i=0; i < StageCells.Count; i++)
        {
            Cell cell = StageCells[i];
            if (!visible || cell.Visible())
            {
                if (gameObject == cell.GetObject())
                {
                    return cell;
                }
            }
        }
        return null;
    }
    private void CellHit(GameObject collider)
    {
        Cell cell = InitializeCell(!(collider.transform.GetChild(0).GetComponent<TextMeshPro>().text == ""), collider);
        Transform cell_transform = cell.GetObject().transform;
        cell.Animator(false);
        StartCoroutine(IsThisCoordinateY(cell_transform));
        StartCoroutine(IsGenerated());
    }
    IEnumerator IsGenerated()
    {
        while (isGenerated)
        {
            yield return null;
        }
        Debug.Log("Stop");
    }
    private void OnEnable()
    {
        CellController.onTouched += CellHit; //sobutie
    }
    private void OnDisable()
    {
        CellController.onTouched -= CellHit;
    }
    public static Action<float> CellAct;
}

public enum States
{
    cell_instant,
    cell_change,
}