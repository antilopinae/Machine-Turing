using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExepshinSymbol : MonoBehaviour
{
    [SerializeField] private Sprite exepshin;
    [SerializeField] private ExepshinMoveText exepshinmoveText;
    public void Exepshin()
    {
        this.gameObject.GetComponent<Image>().sprite = exepshin;
    }
    public void SetExeptionColor(bool fl)
    {
        exepshinmoveText.SetExeptionColor(fl);
    }
}
