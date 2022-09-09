using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

public class countManager : MonoBehaviour
{
    public InputField stt;
    public Text counting;
    
    private void Update()
    {
        CountT();
    }
    public void CountT()
    {
        int length = stt.text.Length;
        counting.text = length.ToString();
    }
}
