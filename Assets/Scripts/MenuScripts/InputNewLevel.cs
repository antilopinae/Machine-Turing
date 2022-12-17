using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputNewLevel : MonoBehaviour
{
    [SerializeField] GameObject PanelInputWord;
    private TMP_InputField inputFieldStartWord;
    private TMP_InputField inputFieldFinishWord;
    [SerializeField] CreateLevel createLevel;
    private string name_level;
    private const string Correctsymbols = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
    private Color Color;
    void Start()
    {
        inputFieldStartWord= PanelInputWord.transform.GetChild(0).GetComponent<TMP_InputField>();
        inputFieldFinishWord= PanelInputWord.transform.GetChild(1).GetComponent<TMP_InputField>();
        PanelInputWord.SetActive(false);
        InitializedInputField(inputFieldFinishWord);
        InitializedInputField(inputFieldStartWord);
        Color = inputFieldFinishWord.image.color;
    }
    private void InitializedInputField(TMP_InputField inputField)
    {
        inputField.onValueChanged.AddListener((string str) => { inputField.image.color = Color; });
    }
    public void SetInput()
    {
        if (PanelInputWord.activeSelf)
        {
            if (inputFieldStartWord.text == "" || !correctSymbol(inputFieldStartWord.text))
            {
                inputFieldStartWord.image.color = Color.red;
                return;
            }
            if (inputFieldStartWord.text == "" || !correctSymbol(inputFieldFinishWord.text))
            {
                inputFieldFinishWord.image.color = Color.red;
                return;
            }
            createLevel.CreateNewLevel(inputFieldStartWord.text, inputFieldFinishWord.text);
        }
        PanelInputWord.SetActive(true);
    }
    private bool correctSymbol(string str) 
    {
        foreach (char c in str.ToCharArray())
        {
            if (!Correctsymbols.Contains(c)) return false;
        }
        return true;
    }
}
