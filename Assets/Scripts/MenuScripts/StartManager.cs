using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public void GoToGame()
    {
        SceneTransition.SwitchToScene("Main Scene");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& SceneManager.GetActiveScene().name == "Main Scene")
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
