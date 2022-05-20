using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InputFieldChecker : MonoBehaviour
{
    public InputField Nickname;
    public GameObject DisTitle;
    public GameObject EnTitle;


    // Update is called once per frame
    public void Update()
    {
        if (Nickname.text == "")
        {
            DisTitle.SetActive(true);
            EnTitle.SetActive(false);
        }
    }
}
