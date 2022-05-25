using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FireStoreScript;
using UnityEngine.UI;
using System;
public class ElseProfileManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Text userName;
    public void ElseProfile()
    {
        GameObject.Find("PrCanvas").transform.Find("GC_Chat_PrCanvas").gameObject.SetActive(true);
        FirebaseManager.ElseData(userName.text);
    }
}
