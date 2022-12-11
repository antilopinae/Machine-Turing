using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnPanelButton : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject partition;
    [SerializeField] GameObject gameBack;
    private void Start()
    {
        gameBack.SetActive(false);
        gameObject.GetComponent<Button>().onClick.AddListener(() => onClick());
    }
    private void onClick()
    {
        panel.transform.GetChild(1).GetChild(0).GetComponent<StationContent>().OnReceivedStations();
        panel.SetActive(false);
        partition.SetActive(false);
        gameBack.SetActive(true);
    }
}
