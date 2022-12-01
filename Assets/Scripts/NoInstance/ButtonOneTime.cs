using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOneTime : MonoBehaviour
{
    // Start is called before the first frame update
    public static Action <bool> action;
    private bool on_click;
    public void ClickMe()
    {
        on_click = true;
        action?.Invoke(on_click);
        on_click = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
