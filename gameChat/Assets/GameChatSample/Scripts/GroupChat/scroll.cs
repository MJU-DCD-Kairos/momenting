using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text;

public class scroll : MonoBehaviour
{
    
    public GameObject getDownbtn;
    public Scrollbar scrollBar;

    // Start is called before the first frame update
    public void Update()
    {
        bool isBottom = scrollBar.value <= 0.00001f;
        if (isBottom)
        {
            getDownbtn.gameObject.SetActive(false);

        }
    }

}
