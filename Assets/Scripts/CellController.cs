using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{

    //обработка заднего фона, обработка движения обьектов, обработка движения камеры при открытии панели

    public static Action <GameObject>onTouched;
    private void OnTriggerEnter(Collider collider)
    {
        onTouched?.Invoke(collider.gameObject);
    }
    private float progress = 0;
    private bool moving=false;
    private bool start = true;
    private float x_position;
    private void Start()
    {
        
    }
    void Update()
    {
        if (MainGame.SetLevel == false && this.start)
        {
            ControllerManager.Cell.cells_parent.transform.position = new Vector3(0, 0, 0);
            start = false;
        }
        if (moving)
        {
            ControllerManager.Cell.cells_parent.transform.position = Vector3.Lerp(ControllerManager.Cell.cells_parent.transform.position, new Vector3(-x_position, 0, 0), progress);
            progress += Time.deltaTime;
            if(progress >= 1) moving= false;
        }
        
    }
    private List<ControllerManager.Cell> StageCells;
    private void SlipCell(float x_position)
    {
        progress = 0f;
        this.x_position = x_position;
        moving = true;
    }
    private void OnEnable()
    {
        ControllerManager.CellAct += SlipCell;
    }
    private void OnDisable()
    {
        ControllerManager.CellAct -= SlipCell;
    }
}
