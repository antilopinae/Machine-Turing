using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExepshinState : MonoBehaviour
{
    [SerializeField] private Sprite exepshin;

    public void Exepshin()
    {
        this.gameObject.GetComponent<Image>().sprite = exepshin;
    }
}
