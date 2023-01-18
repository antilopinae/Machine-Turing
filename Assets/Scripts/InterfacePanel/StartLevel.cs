using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;
    void Start()
    {
        for(int i=0; i<gameObjects.Length;i++)
        {
            gameObjects[i].SetActive(i==0);
        }
    }
    public void HideTable()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(i != 0);
        }
    }
    public void SeeTable()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(i == 0);
        }
    }
}
