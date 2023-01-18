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
    private string[] str = { "R", "L", "N" };
    void Update()
    {
        if (inputField.gameObject.activeSelf && inputField!=null && TouchScreenKeyboard.visible==false && !str.Contains(inputField.text))
        {
            inputField.image.color = exepshinColor;
        }
        else
        {
            inputField.image.color = normalColor;
        }
    }
}
