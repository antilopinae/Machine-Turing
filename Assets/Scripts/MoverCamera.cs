using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverCamera : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private float speed;
    [SerializeField] private float shift;
    private Vector3 pos_camera;
    private GameObject camera;
    private float progress;
    private bool move;
    private int direction;
    private bool flag_left;
    private bool flag_right;
    void Start()
    {
        camera=gameObject;
        pos_camera=camera.transform.position;
        move = false;
        flag_left = true;
        flag_right = true;
    }
    private void MoveRight()
    {
        direction = 1;
        progress = 0f;
        move = true;
    }
    private void MoveLeft()
    {
        direction = -1;
        progress = 0f;
        move = true;
    }
    private void Update()
    {
        if (flag_left && !panel.activeSelf)
        {
            MoveLeft();
            flag_right=true;
            flag_left=false;
        }
        else if(flag_right && panel.activeSelf)
        {
            MoveRight();
            flag_right = false;
            flag_left = true;
        }
        if (move)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(pos_camera.x + shift * direction, pos_camera.y, pos_camera.z), progress);
            progress += Time.deltaTime * speed;
            if (progress >= 1) move = false;
        }
    }
}
