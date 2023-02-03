using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Input Field Validator", menuName = "Input Field Validator")]

public class InputFieldSystem : TMPro.TMP_InputValidator
{
    [SerializeField] int count_symbols=1;
    /// <summary>
    /// Create your validator class and inherit TMPro.TMP_InputValidator 
    /// Note that this is a ScriptableObject, so you'll have to create an instance of it via the Assets -> Create -> Input Field Validator 
    /// </summary>
    /// <summary>
    /// Override Validate method to implement your own validation
    /// </summary>
    /// <param name="text">This is a reference pointer to the actual text in the input field; changes made to this text argument will also result in changes made to text shown in the input field</param>
    /// <param name="pos">This is a reference pointer to the input field's text insertion index position (your blinking caret cursor); changing this value will also change the index of the input field's insertion position</param>
    /// <param name="ch">This is the character being typed into the input field</param>
    /// <returns>Return the character you'd allow into </returns>
    private const string _currentSymbols = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm,1234567890!_$%^&()-+=.";
    public Action NextCell;

    public override char Validate(ref string text, ref int pos, char ch)
    {
        Debug.Log($"Text = {text}; pos = {pos}; chr = {ch}");
        // If the typed character is a number, insert it into the text argument at the text insertion position (pos argument)
        if((int)ch== 9 || (int)ch == 13)
        {
            Debug.Log("NextCell");
            NextCell?.Invoke();
            return '\0';
        }
        if (_currentSymbols.Contains(ch) && text.Length <= count_symbols)
        {
            if (ch.ToString() == ","||ch.ToString()==" ")
            {
                Debug.Log("NextCell");
                NextCell?.Invoke();
                return '\0';
            }
            else if (text.Length == count_symbols && ch.ToString()!=",") { return '\0'; }
            else
            {
                // Insert the character at the given position if we're working in the Unity Editor
#if UNITY_EDITOR
                text = text.Insert(pos, ch.ToString());
#endif
                pos++;
                //text = System.String.Format("{0:(#}", number);

            }
            return ch;
        }
            // If the character is not a number, return null
        else
        {
            return '\0';
        }
    }
}
