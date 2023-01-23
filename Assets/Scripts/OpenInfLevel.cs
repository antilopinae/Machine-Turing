using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class OpenInfLevel : MonoBehaviour
{
    [SerializeField] GameObject button_information;
    [SerializeField] Sprite[] images;
    [SerializeField] GameObject TableLevel;
    private int level;
    void Start()
    {
        TableLevel.SetActive(false);
        if (MainGame.IndGameLevel != null) {
            level = (int)MainGame.IndGameLevel;
            TableLevel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = images[level - 1];
            button_information.GetComponent<Button>().onClick.AddListener(() => {TableLevel.SetActive(true); });
        }
        else button_information.SetActive(false);
    }
    public void getInformation()
    {
        TableLevel.SetActive(true);
    }
}
