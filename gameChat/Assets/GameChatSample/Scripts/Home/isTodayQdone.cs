using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isTodayQdone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (1 == PlayerPrefs.GetInt("todayQdone"))
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
