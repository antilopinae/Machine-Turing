using System.Collections;
using UnityEngine;
using TMPro;


public class CursorToNextInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField next_input;
    private Transform stationContent;
    private TMP_InputField prev_input;
    private TMP_InputValidator inputValidator;
    private InputFieldSystem inputSystem;
    private static bool seeHELP = true;
    private void Awake()
    {
        prev_input = GetComponent<TMP_InputField>();
        inputValidator = prev_input.inputValidator;
        inputSystem = inputValidator as InputFieldSystem;
    }
    private void Start()
    {
        //getSeeHelp
        SeeHelp(prev_input, false);
    }
    public void EndText()
    {
        inputSystem.NextCell -= GoNext;
        SeeHelp(prev_input, false);
    }
    public void StartText()
    {
        inputSystem.NextCell += GoNext;
        SeeHelp(prev_input, true);
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
                        catch { next_input = stationContent.GetChild(i + 2).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                        }
                    }
                }
            }
        }
        if (next_input != null)
        {
            SeeHelp(prev_input, false);
            inputSystem.NextCell -= GoNext;
            Debug.Log("1000");
            next_input.ActivateInputField();
            SeeHelp(next_input, true);
            //StartCoroutine(deselector());
        }
    }
    /*IEnumerator deselector()
    {
        yield return new WaitForEndOfFrame();
        next_input.MoveTextEnd(false);
    }*/
    private void SeeHelp(TMP_InputField text, bool flag)
    {
        if(seeHELP)
        {
            try
            {
                text.gameObject.transform.GetChild(1).gameObject.SetActive(flag);
            }
            catch { Debug.Log("Exeption Not Help"); }
        }
    }

    private void OnDisable()
    {
        inputSystem.NextCell -= GoNext;
    }
}
