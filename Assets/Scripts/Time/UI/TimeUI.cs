using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    public RectTransform dayNightImage;
    public RectTransform clockParent;
    public Image seasonImage;
    public TextMeshProUGUI dataText;
    public TextMeshProUGUI timeText;


    public Sprite[] seasonSprite;

    private List<GameObject> clockBlocks = new List<GameObject>();

    public void Awake()
    {
        for(int i = 0; i < clockParent.childCount; i++)
        {
            clockBlocks.Add(clockParent.GetChild(i).gameObject);
            clockParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.GameDateEvent += OnGameDateEvent;
    }
private void OnDisable()
    {
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.GameDateEvent -= OnGameDateEvent;
    }

    private void OnGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        dataText.text = string.Format("{0}/{1}/{2}", year, month,day );
        seasonImage.sprite = seasonSprite[(int)season];
        SwitchHourImage(hour);
        DayNightImageRotate(hour);
        SwitchSeaonSprite(season);
    }

    private void OnGameMinuteEvent(int arg1, int arg2)
    {
        timeText.text = string.Format("{0:D2}:{1:D2}", arg1, arg2);
    }

    private void SwitchHourImage(int hour)
    {
        int index = hour / 4;
        for (int i = 0; i < clockBlocks.Count; i++)
        {
            if (i <= index)
            {
                clockBlocks[i].SetActive(true);
            }
            else
            {
                clockBlocks[i].SetActive(false);
            }
        }
    }
    private void DayNightImageRotate(int hour)
    {
        var target = new Vector3(0, 0, hour * 15);
        dayNightImage.DORotate(target, 1.5f, RotateMode.Fast);
    }

    private void SwitchSeaonSprite(Season Season)
    {
        seasonImage.sprite = seasonSprite[(int)Season];
    }
}
