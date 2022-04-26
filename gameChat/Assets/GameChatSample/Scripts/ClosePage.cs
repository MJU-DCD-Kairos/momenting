using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClosePage : MonoBehaviour
{
    public GameObject Inactive;
    public void Close()
    {
        Inactive.SetActive(false);
    }
 
}
