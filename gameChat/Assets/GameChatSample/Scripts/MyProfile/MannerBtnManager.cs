using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MannerBtnManager : MonoBehaviour
{
    public Toggle Level1;
    public Toggle Level2;
    public Toggle Level3;
    public Toggle Level4;
    public Toggle Level5;
    public Toggle Level6;

    //public GameObject Level1_text;
    //public GameObject Level2_text;
    //public GameObject Level3_text;
    //public GameObject Level4_text;
    //public GameObject Level5_text;
    //public GameObject Level6_text;

    public void MannerButtons(bool select)
    {
        GameObject Level1_text = GameObject.Find("Background_Image").transform.Find("Level1").gameObject;
        GameObject Level2_text = GameObject.Find("Background_Image").transform.Find("Level2").gameObject;
        GameObject Level3_text = GameObject.Find("Background_Image").transform.Find("Level3").gameObject;
        GameObject Level4_text = GameObject.Find("Background_Image").transform.Find("Level4").gameObject;
        GameObject Level5_text = GameObject.Find("Background_Image").transform.Find("Level5").gameObject;
        GameObject Level6_text = GameObject.Find("Background_Image").transform.Find("Level6").gameObject;
        Level1_text.SetActive(false);
        Level2_text.SetActive(false);
        Level3_text.SetActive(false);
        Level4_text.SetActive(false);
        Level5_text.SetActive(false);
        Level6_text.SetActive(false);

        if (Level1.isOn)
        {
            Level1_text.SetActive(true);
        }
        else if (Level2.isOn)
        {
            Level2_text.SetActive(true);
        }
        else if (Level3.isOn)
        {
            Level3_text.SetActive(true);
        }
        else if (Level4.isOn)
        {
            Level4_text.SetActive(true);
        }
        else if (Level5.isOn)
        {
            Level5_text.SetActive(true);
        }
        else
        {
            Level6_text.SetActive(true);
        }
        
    }

    private void Awake()
    {
        Level1.onValueChanged.AddListener(MannerButtons);
        Level2.onValueChanged.AddListener(MannerButtons);
        Level3.onValueChanged.AddListener(MannerButtons);
        Level4.onValueChanged.AddListener(MannerButtons);
        Level5.onValueChanged.AddListener(MannerButtons);
        Level6.onValueChanged.AddListener(MannerButtons);
    }

    public void OpenPopup()
    {
        Level1.isOn = true;
    }
}
