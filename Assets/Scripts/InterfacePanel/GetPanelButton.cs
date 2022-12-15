using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPanelButton : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject partition;
    [SerializeField] GameObject gameBack;

    private void Start()
    {
        gameBack.SetActive(true);
        panel.SetActive(false);
        partition.SetActive(false);
        gameObject.GetComponent<Button>().onClick.AddListener(() =>onClick());
    }
    private void onClick()
    {
        panel.SetActive(true);
        panel.transform.GetChild(1).GetChild(0).GetComponent<StationContent>().OnReceivedStations();
        //partition.SetActive(MainGame.IsPlaying);
        gameBack.SetActive(false);
    }
}
