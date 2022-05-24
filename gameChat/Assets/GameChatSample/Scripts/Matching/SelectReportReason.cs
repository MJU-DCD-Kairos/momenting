using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectReportReason : MonoBehaviour
{
    public Reasons ReportReason;
    public Toggle reasonTgl;

    public void OnMouseUpAsButton()
    {
        if (reasonTgl.isOn)
        {
            Report.instance.selectReason = ReportReason;
            checkReason();
        }
    }

    public void checkReason()
    {
        Debug.Log(ReportReason);
    }
}
