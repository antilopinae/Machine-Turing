using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPanelButton : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject partition;

    private void Start()
    {
        gameObject.SetActive(true);
        panel.SetActive(false);
        partition.SetActive(false);
    }
    public void OnClick()
    {
        panel.transform.GetChild(0).GetChild(0).GetComponent<StationContent>().OnReceivedStations();
        panel.SetActive(true);
        partition.SetActive(MainGame.IsPlaying);
        gameObject.SetActive(false);
    }
}
