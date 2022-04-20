using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MannerEvaluation : MonoBehaviour
{
    public string MannerLevel;
    public string UserName;
    public Text Name;
    public Text Level;
    public Image Indicator;



    // Start is called before the first frame update
    void Start()
    {
        Name.text = UserName + "���� �ųʵ����";

        if (MannerLevel == "1")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level1") as Sprite;
            Level.text = "�𷡾��Դϴ�.";
        }
        else if(MannerLevel == "2")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level2") as Sprite;
            Level.text = "�𷡹�ġ�Դϴ�.";
        }
        else if (MannerLevel == "3")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level3") as Sprite;
            Level.text = "�β������Դϴ�.";
        }
        else if (MannerLevel == "4")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level4") as Sprite;
            Level.text = "�����Դϴ�.";
        }
        else if (MannerLevel == "5")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level5") as Sprite;
            Level.text = "�𷡼��Դϴ�.";
        }
        else if (MannerLevel == "6")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level6") as Sprite;
            Level.text = "�𷡱����Դϴ�.";
        }
    }

}
