using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class ControllerManager : MonoBehaviour
{
    public class Cell
    {
        public static GameObject cells_parent;
        private GameObject cell_obj_orig;
        private GameObject cell_obj_parent;
        private string cell_name;
        private float cell_position;
        public Cell(string cell_name, GameObject cell_object,float cell_position)
        {
            this.cell_obj_orig = cell_object.transform.GetChild(0).gameObject;
            this.cell_obj_parent = cell_object;
            this.cell_position = cell_position;
            this.cell_name = cell_name;
            this.cell_obj_parent.transform.parent = cells_parent.transform;
            this.cell_obj_parent.transform.localPosition = new Vector3(cell_position, 0 ,0);
            this.cell_obj_orig.transform.GetChild(0).GetComponent<TextMeshPro>().text = this.cell_name;
        }
    }
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private Button button;
    [SerializeField] float step;
    private float bound;
    private float x_position=0f;
    public static List<Cell> StageCells= new List<Cell>();
    private bool isGenerated = true;
    private bool tet_tet = false;
    private bool setlevel = false;
    private int i=0;
    private char namecell;
    public static List<Item> Items;
    private char[] StartWord;
    private char[] FinishWord;
    private int level_id;

    private void Start()
    {
        bound = gameObjects[0].GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.x;
        button.onClick.AddListener(() => { AddCell('Y'); });
        SetLevel(1);
    }
    private void Update()
    {
        if (isGenerated)
        {
            if (setlevel)
            {
                if (i < StartWord.Length)
                {
                    AddCell(StartWord[i]);
                    i++;
                }
                else { setlevel = false; MainGame.SetLevel = false; }
                }
            else{
                if (tet_tet)
                {
                    AddCell(namecell);
                    tet_tet = false;
                }
            }

            void AddCell(char name)
            {
                GameObject cell = generateCells(gameObjects[1]);
                StartCoroutine(IsThisCoordinateY(cell.transform.GetChild(0)));
                StageCells.Add(new Cell(name.ToString(), cell, x_position));
                CellAct?.Invoke(x_position);
                x_position += (bound + step);
            }
        }
        else tet_tet = false;
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
        setlevel = true;
    }

    private GameObject generateCells(GameObject prefab)
    {
        return Instantiate(prefab, new Vector3(0, 1, 0), Quaternion.identity);
    }
    
    private void AddCell(char name)
    {
        namecell = name;
        tet_tet = true;
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
    private void CellHit(GameObject collider)
    {
        //Debug.Log($"{value}Кубик ударился о землю!");
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


    private States StateCell
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }
}

public enum States
{
    instant,
    change,
}