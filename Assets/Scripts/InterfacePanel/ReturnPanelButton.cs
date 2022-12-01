using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPanelButton : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject partition;
    [SerializeField] GameObject button;
    private void Start()
    {
        button.SetActive(false);
    }
    public void OnClick()
    {
        panel.transform.GetChild(0).GetChild(0).GetComponent<StationContent>().OnReceivedStations();
        panel.SetActive(false);
        partition.SetActive(false);
        button.SetActive(true);
        gameObject.SetActive(false);
    }
}
