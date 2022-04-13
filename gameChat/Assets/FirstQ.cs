using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class FirstQ : MonoBehaviour
{
    public GameObject FQ;
    // Start is called before the first frame update
    public void Start()
    {
        Invoke("SA", 0.3f);
    }

    // Update is called once per frame
    public void SA()
    {
        FQ.gameObject.SetActive(true);
        Debug.Log("ss");
    }
}
