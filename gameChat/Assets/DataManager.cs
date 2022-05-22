using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
public class DataManager : MonoBehaviour
{
    public static List<string> TenList = new List<string>();

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
