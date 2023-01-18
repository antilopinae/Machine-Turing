using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{
    [SerializeField][Range(1, 3)] private float speed;
    private float progress = 0;
    private bool moving = false;
    private float x_position;
    private float bound;
    private float step=0f;

    public static Action <GameObject>onTouched;
    private void OnTriggerEnter(Collider collider)
    {
        if (!MainGame.SetLevel)
        onTouched?.Invoke(collider.gameObject);
    }
    private void Start()
    {
        this.bound = ControllerManager.bound;
        this.step=ControllerManager.step;
    }
    void FixedUpdate()
    {
        if (moving)
        {
            ControllerManager.Cell.cells_parent.transform.position = Vector3.Lerp(ControllerManager.Cell.cells_parent.transform.position, new Vector3(-x_position, 0, 0), progress);
            progress += Time.deltaTime*speed;
            if(progress >= 1) moving= false;
        }
    }
    private void SlipCell(float x_position)
    {
        progress = 0f;
        this.x_position = x_position;
        moving = true;
    }
    private void SlipCell(int vector)
    {
        x_position = (bound+step) * vector + this.x_position;
        SlipCell(x_position);
    }
    private void OnEnable()
    {
        ControllerManager.CellAct += SlipCell;
        MainGame.MovingCells += SlipCell;
    }
    private void OnDisable()
    {
        ControllerManager.CellAct -= SlipCell;
        MainGame.MovingCells -= SlipCell;
    }

}
