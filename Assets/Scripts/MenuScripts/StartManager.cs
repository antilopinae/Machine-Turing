using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public void GoToGame()
    {
        Time.timeScale = 1.0f;
        //MainGame.SetLevel = true;
        //MainGame.IsPlaying= false;
        SceneTransition.SwitchToScene("Main Scene");
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
