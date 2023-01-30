using System;
using System.Collections;
using UnityEngine;

public class CellController : MonoBehaviour
{
    [SerializeField][Range(0, 1)] private float speed;
    private float x_position;
    private float bound;
    private float step=0f;

    public static Action <GameObject>onTouched;
    public static Action<bool> AddCellRight;
    private void OnTriggerEnter(Collider collider)
    {
        //if (!MainGame.SetLevel )
        onTouched?.Invoke(collider.gameObject);
    }
    private void Start()
    {
        this.bound = ControllerManager.bound;
        this.step=ControllerManager.step;
    }
    IEnumerator IMoving()
    {
        float progress = 0f;
        while (progress < 1)
        {
            if (ControllerManager.Cell.cells_parent != null)
            ControllerManager.Cell.cells_parent.transform.position = Vector3.Lerp(ControllerManager.Cell.cells_parent.transform.position, new Vector3(-x_position, 0, 0), progress);
            progress += speed;
            yield return null;
        }
    }
    private void SlipCell(float x_position)
    {
        this.x_position = x_position;
        StartCoroutine(IMoving());
    }
    private void SlipCell(int vector)
    {
        x_position = (bound+step) * vector + this.x_position;
        SlipCell(x_position);

        if(x_position>70*(bound+step))
        {
            AddCellRight?.Invoke(true);
        }
        else if(x_position < -70 * (bound + step))
        {
            AddCellRight?.Invoke(false);
        }
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
