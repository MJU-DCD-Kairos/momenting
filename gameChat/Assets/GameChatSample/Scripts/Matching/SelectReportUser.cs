using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectReportUser : MonoBehaviour
{
    public Toggles toggles;
    public Toggle tgl;

    public void OnMouseUpAsButton()
    {
        if (tgl.isOn)
        {
            Report.instance.currentToggle = toggles;
            toggleSelect();
        }
        
    }

    public void toggleSelect()
    {
        Debug.Log(toggles);
    }
}
