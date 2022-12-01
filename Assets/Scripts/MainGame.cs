using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    //обработка игровых движений из скриптов Station Content и Controller Manager
    [SerializeField] StationContent stationcontent;
    [SerializeField] ControllerManager controllerManager;
    [SerializeField] Button buttonPause;
    [SerializeField] Button buttonContinue;
    [SerializeField] Button buttonOneStep;
    public static bool IsPlaying = false;
    public static bool SetLevel = true;
    private void Start()
    {
        buttonPause.onClick.AddListener(()=> { ; });
        buttonOneStep.onClick.AddListener(() => {; });
        buttonContinue.onClick.AddListener(()=> { ; });
    }

    private void OnApplicationPause(bool pause)
    {
        Pause();
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }
    private void OneStep(string test)
    {

    }
    private void Pause()
    {

    }
    private void Continue()
    {

    }
    private void PlayGame()
    {

    }
    
}
