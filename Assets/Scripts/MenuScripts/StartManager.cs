using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Button butInf;
    public void GoToGame()
    {
        Time.timeScale = 1.0f;
        //MainGame.SetLevel = true;
        //MainGame.IsPlaying= false;
        SceneTransition.SwitchToScene("Main Scene");
    }
    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }
    private void Start()
    {
        butInf.onClick.AddListener(() => {Application.OpenURL("https://youtube.com/playlist?list=PL23FbGs8UKxQFqFNWeU6v6F_Ma-h8gsE6"); });
    }

    /*public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& SceneManager.GetActiveScene().name == "Main Scene")
        {
            SceneManager.LoadScene("Menu");
        }
    }*/
    public void Logout()
    {
        Application.Quit();
    }
    public void GoToMenu()
    {
        Time.timeScale= 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
