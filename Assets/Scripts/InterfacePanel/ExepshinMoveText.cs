using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExepshinMoveText : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Color exepshinColor;
    private string[] str = { "R", "L", "N" };
    void Update()
    {
        if (inputField.gameObject.activeSelf && inputField!=null && TouchScreenKeyboard.visible==false && inputField.text!= "" && inputField.text != str[0]&& inputField.text != str[1] && inputField.text != str[2])
        {
            inputField.image.color = exepshinColor;
        }
    }
}
