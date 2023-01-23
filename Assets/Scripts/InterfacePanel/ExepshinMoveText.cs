using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExepshinMoveText : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Color exepshinColor;
    [SerializeField] Color normalColor;
    [SerializeField] Color exepshinColor2;
    private string[] str = { "R", "L", "N", "" };
    private bool isSymbolExeption=false;
    private void Start()
    {
        MoveText();
    }
    public void MoveText()
    {
        if (isSymbolExeption) { inputField.image.color = exepshinColor2; }
        else
        {
            if (inputField.gameObject.activeSelf && inputField != null && TouchScreenKeyboard.visible == false && !str.Contains(inputField.text))
            {
                inputField.image.color = exepshinColor;
            }
            else
            {
                inputField.image.color = normalColor;
            }
        }
    }
    public void SetExeptionColor(bool fl)
    {
        isSymbolExeption = fl;
    }
}
