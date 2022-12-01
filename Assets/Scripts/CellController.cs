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
    void Start()
    {

    }

    private bool start = true;
    void Update()
    {
        if (MainGame.SetLevel == false && this.start)
        {
            ControllerManager.Cell.cells_parent.transform.position = new Vector3(0, 0, 0);
            start = false;
        }
    }
    private List<ControllerManager.Cell> StageCells;
    private void SlipCell(float x_position)
    {
        ControllerManager.Cell.cells_parent.transform.position = new Vector3(-x_position, 0, 0);
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
