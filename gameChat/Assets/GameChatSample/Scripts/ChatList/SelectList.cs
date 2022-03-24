using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectList : MonoBehaviour
{
    public Toggle checkbox;
    public string selected;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUpAsButton()
    {
        if (checkbox.isOn == true)
        {
            OnSelect();
        }
        else
        {
            OffSelect();
        }
    }
    public void OnSelect()
    {
        selected = "selected";

    }

    public void OffSelect()
    {
        selected = "";
    }

    
}
