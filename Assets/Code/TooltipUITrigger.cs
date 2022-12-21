using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TooltipUITrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Tooltip tooltip;
    public string title;
    public string saveKey;
    public bool comingSoon;


    public void OnPointerEnter(PointerEventData eventData)
    {

        tooltip.SetHeader(title);

        if (comingSoon)
        {
            tooltip.SetContent("Coming Soon!");
            tooltip.contentField.color = Color.red;
        }
        else
        {
            tooltip.contentField.color = Color.white;
            float saveSeconds = PlayerPrefs.GetFloat(saveKey, -1f);

            if (saveSeconds <= 0)
            {
                tooltip.SetContent("Play this level next to set a high record!");
            }
            else
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, (int)saveSeconds);
                Debug.Log(timeSpan.TotalMinutes);
                tooltip.SetContent("Best time: " + Math.Floor(timeSpan.TotalMinutes).ToString("00") + ":" + timeSpan.Seconds.ToString("00"));
            }
        }

        tooltip.Show();
        tooltip.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }

    private void OnDisable()
    {
        tooltip.Hide();
    }

}
