using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputNewLevelSystem : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldStartWord;
    [SerializeField] private TMP_InputField inputFieldFinishWord;
    [SerializeField] private GameObject PanelInput;
    [SerializeField] private Button butOpenPanel;
    [SerializeField] private Button butClosePanel;
    [SerializeField] private Button butStartGame;
    [SerializeField] private MainGame mainGame;
    private const string Correctsymbols = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
    private const string CorrectsymbolsEmpty = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890_";
    private Color color;

    private void Start()
    {
        PanelInput.SetActive(false);
        color = inputFieldFinishWord.image.color;
        butOpenPanel.onClick.AddListener(() => { PanelInput.SetActive(true); });
        butClosePanel.onClick.AddListener(() => { PanelInput.SetActive(false); });
        butStartGame.onClick.AddListener(() => {ButtonStartgame() ; });
        InitializedInputField(inputFieldFinishWord);
        InitializedInputField(inputFieldStartWord);
    }
    private void InitializedInputField(TMP_InputField inputField)
    {
        inputField.onValueChanged.AddListener((string str) => { inputField.image.color = color; });
    }
    private void ButtonStartgame()
    {
            if (inputFieldStartWord.text == "" || !CheckCorrectedSymbols(inputFieldStartWord.text, false))
            {
                inputFieldStartWord.image.color = Color.red;
                return;
            }
            if (inputFieldFinishWord.text == "" || !CheckCorrectedSymbols(inputFieldFinishWord.text, true))
            {
                inputFieldFinishWord.image.color = Color.red;
                return;
            }
            StartGame();
    }
    private bool CheckCorrectedSymbols(string text, bool isEmpty)
    {
        foreach (char c in text.ToCharArray())
        {
            if(isEmpty)
            {
                if (!CorrectsymbolsEmpty.Contains(c)) return false;
            }
            else
            {
                if (!Correctsymbols.Contains(c)) return false;
            }
        }
        return true;
    }
    private void StartGame()
    {
        PanelInput.SetActive(false);
        mainGame.StartGame(inputFieldStartWord.text, inputFieldFinishWord.text);
    }
}
