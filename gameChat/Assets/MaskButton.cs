using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class MaskButton : MonoBehaviour
{
    public Image mask;
    public Image image;
    public void EnterSize()
    {
        RectTransform rect = (RectTransform)image.transform;
        rect.sizeDelta = new Vector2(592, 592);
        RectTransform rect2 = (RectTransform)mask.transform;
        rect2.sizeDelta = new Vector2(592, 592);
    }
    public void ExitSize()
    {
        RectTransform rect = (RectTransform)image.transform;
        rect.sizeDelta = new Vector2(512, 512);
        RectTransform rect2 = (RectTransform)mask.transform;
        rect2.sizeDelta = new Vector2(512, 512);
    }
}
