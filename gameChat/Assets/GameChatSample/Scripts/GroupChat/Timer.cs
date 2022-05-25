using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using groupchatManager;
public class Timer : MonoBehaviour
{
    Image timerBar;
    public float maxTime = 1200f;
    float timeLeft;
    public GameObject eend;
    public GameObject deActive;

    void Start()
    {

        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;

        }
        else
        {
            Time.timeScale = 0;

            //eend.gameObject.SetActive(true);
            deActive.gameObject.SetActive(true);

        }

    }
    
}
