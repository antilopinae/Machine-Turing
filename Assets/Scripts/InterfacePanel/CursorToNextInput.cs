using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

using Unity.VisualScripting;
using UnityEditor.VersionControl;

public class CursorToNextInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField next_input;
    private Transform stationContent;
    private TMP_InputField prev_input;
    private TMP_InputValidator inputValidator;
    private InputFieldSystem inputSystem;
    private void Awake()
    {
        prev_input = GetComponent<TMP_InputField>();
        inputValidator = prev_input.inputValidator;
        inputSystem = inputValidator as InputFieldSystem;
    }
    public void EndText()
    {
        inputSystem.NextCell -= GoNext;
    }
    public void StartText()
    {
        inputSystem.NextCell += GoNext;
    }
    private void GoNext()
    {
        Debug.Log("Go next");
        if(next_input==null)
        {
            stationContent = gameObject.transform.parent.parent.parent;
            for (int i = 0; i < stationContent.childCount; i++)
            {
                Transform child = stationContent.GetChild(i);
                if (child != null && child==transform.parent.parent) {
                    if (i+3< stationContent.childCount)
                    {
                        try
                        {
                            next_input = stationContent.GetChild(i + 1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                        }
                        catch { next_input = stationContent.GetChild(i + 2).GetChild(0).GetChild(0).GetComponent<TMP_InputField>(); }
                    }
                }
            }
        }
        if (next_input != null)
        {
            next_input.ActivateInputField();
            StartCoroutine(deselector());
        }
    }
    IEnumerator deselector()
    {
        yield return new WaitForEndOfFrame();
        next_input.MoveTextEnd(false);
    }


    private void OnDisable()
    {
        inputSystem.NextCell -= GoNext;
    }
}
