using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GDManager : MonoBehaviour
{
    public Scrollbar scrollBar;
    public GameObject getDownbtn;
    public void GetDown()
    {
        Invoke("ScrollDelay", 0.03f);
        getDownbtn.gameObject.SetActive(false);
    }

    void ScrollDelay() => scrollBar.value = 0;
}
