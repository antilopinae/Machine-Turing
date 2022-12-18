using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMenuPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject panelGoTo;
    void Start()
    {
        panelGoTo.SetActive(false);
    }
    public void OpenPanelGoTo()
    {
        panelGoTo.SetActive(!panelGoTo.activeSelf);
    }
}
