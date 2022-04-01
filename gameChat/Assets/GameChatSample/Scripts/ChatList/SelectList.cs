using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectList : MonoBehaviour
{
    public Toggle check;
    public string selected;

    public void OnMouseUpAsButton()
    {
        if (check.isOn == true)
        {
            OnSelect();
            CheckSelected();
            //GameObject.FindGameObjectWithTag("ListManager").GetComponent<PersonalListEdit>().OnChecked();

        }
        else
        {
            OffSelect();
        }
    }

    public void CheckSelectedOn()
    {
        GameObject.Find("Btn_Edit_Enabled").SetActive(true);
        //DisabledBtn.SetActive(false);

        //PersonalList.Remove(PersonalList.FindAll(x => x.toggle.isOn));

        
    }

    public void CheckSelectedOff()
    {

    }

    public void OnSelect()
    {
        selected = "selected";

    }

    public void OffSelect()
    {
        selected = "";
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public GameObject obj;

    /*
    public static void OnChecked()
    {
        GameObject obj = GameObject.Find("ListManager").GetComponent<GameObject>();
        obj.OnChecked();
        //int index = PersonalList.FindIndex(x => x.toggle.isOn = true);
        //string idx = index.ToString();
        //currentSelected.Add(idx);
    }*/
    

}
