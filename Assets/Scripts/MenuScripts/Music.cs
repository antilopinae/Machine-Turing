using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    [SerializeField] GameObject music;
    [SerializeField] Sprite musicPlaying;
    [SerializeField] Sprite musicPaused;
    private Image musicImage;
    void Start()
    {
        music.SetActive(true);
        musicImage= music.GetComponent<Image>();
        music.GetComponent<Button>().onClick.AddListener(() => { ChangeSprite(musicImage.sprite!= musicPlaying); }); //sql
    }
    private void ChangeSprite(bool flag)
    {
        if (musicImage != null)
        {
            if (flag)
            {
                musicImage.sprite = musicPlaying;
            }
            else musicImage.sprite = musicPaused;
        }
    }
}
