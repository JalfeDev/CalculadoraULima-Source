using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class ExamsScheduleChanger : MonoBehaviour
{
    [Header("Para el cambio de botones")]
    Text text;  //Texto de "Ciclo 1"
    public int index;
    public int maxIndex;
    public Button leftButton;
    public Button rightButton;
    
    [Header("Para las imagenes")]
    // public ConvertImagesRedirected convertor;
    // public string type;
    public float contentWidth;
    public GetLocalImages localImages;
    public RawImage content;

    [Header("Schedule")]
    public bool isSchedule;
    public Scrollbar scrollbar;

    void Start()
    {
        if (isSchedule) UpdateSchedule();

        text = GetComponent<Text>();
        maxIndex = PlayerPrefs.GetInt("NumCyclesImages");
        index = maxIndex - 1;
        UpdateButtons();
    }

    public void ChangeExam(int add)
    {
        index += add;
        if (localImages.type == GetLocalImages.ImageType.Examenes) localImages.DownloadOneExam(index);
        PlayerPrefs.SetInt("IndexSemester", index + 1);
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        content.texture = localImages.tex[index];
        text.text = $"Ciclo {index+1}";
        ResizeRawImage();

        if (index == 0)
        {
            leftButton.interactable = false;
            rightButton.interactable = true;
        }
        else if (index == maxIndex-1)//(index == localImages.tex.Length-1)
        {
            rightButton.interactable = false;
            leftButton.interactable = true;
        }
        else
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
    }

    public void ResizeRawImage()
    {
        //Width = 312
        if (content.texture != null)
        {
            content.SetNativeSize();
            float mult = contentWidth/content.rectTransform.sizeDelta.x;
            content.rectTransform.sizeDelta *= mult;
            
            // Debug.Log($"rectTransform.sizeDelta.x : {content.rectTransform.sizeDelta.x}");
            // Debug.Log($"rectTransform.sizeDelta.y : {content.rectTransform.sizeDelta.y}");
        }
    }

    public void UpdateSchedule() //Que codigo menos optimo
    {
        int dayOfWeek = Math.Clamp( (int)DateTime.Now.DayOfWeek, 1, 5); //0 es domingo, 6 es sabado
        scrollbar.value = Mathf.Lerp(0f, 1f, (dayOfWeek-1)/4f );
    }
}