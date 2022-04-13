using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizerFollow : MonoBehaviour
{
    public float size;
    public Image flow;


    public void Follow()
    {
        var RectTransform = transform as RectTransform;
        flow.GetComponent<RectTransform>().sizeDelta = new Vector2(592 , 592);
        Debug.Log("ss");
    }
        
}
