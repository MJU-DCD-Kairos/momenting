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
        Name.text = UserName + "님의 매너등급은";

        if (MannerLevel == "1")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level1") as Sprite;
            Level.text = "모래알입니다.";
        }
        else if(MannerLevel == "2")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level2") as Sprite;
            Level.text = "모래뭉치입니다.";
        }
        else if (MannerLevel == "3")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level3") as Sprite;
            Level.text = "두꺼비집입니다.";
        }
        else if (MannerLevel == "4")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level4") as Sprite;
            Level.text = "모래집입니다.";
        }
        else if (MannerLevel == "5")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level5") as Sprite;
            Level.text = "모래성입니다.";
        }
        else if (MannerLevel == "6")
        {
            Indicator.sprite = Resources.Load<Sprite>("Art/UI/Indicator/C_Indicator_ProgressBar_Level6") as Sprite;
            Level.text = "모래궁전입니다.";
        }
    }

}
