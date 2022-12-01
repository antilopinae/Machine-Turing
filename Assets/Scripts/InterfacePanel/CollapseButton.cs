using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollapseButton : MonoBehaviour
{
    [SerializeField] private Sprite collapsed;
    [SerializeField] private Sprite nocollapse;
    
    public void Collapsed(bool bl)
    {
        if (bl) this.gameObject.GetComponent<Image>().sprite = nocollapse;
        else this.gameObject.GetComponent<Image>().sprite = collapsed;
    }
}
