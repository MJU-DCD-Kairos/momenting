using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject Canvas_back;
    public GameObject Canvas_next;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Back()
    {
        Canvas_back.SetActive(true);
        Canvas_next.SetActive(false);

    }

    public void Next()
    {
        Canvas_back.SetActive(false);
        Canvas_next.SetActive(true);

    }
}
