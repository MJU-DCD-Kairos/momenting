using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotMatched : MonoBehaviour
{
    public GameObject[] charPrefabs;
    public GameObject user;
    void Start()
    {
        user = Instantiate(charPrefabs[(int) ChoMgr.instance.currentChoice]);
        
    }

   
}
