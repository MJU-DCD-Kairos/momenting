using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
   
    float currentTime = 0f;
    public float startingTime = 1200f;
    [SerializeField] 
    public Text countdownText;
  
    void Start()
    {
        currentTime = startingTime;
    }
    void Update() { 

        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");
       


        if (currentTime <= 0)
        {
            currentTime = 0;
        }
        DisplayTime(currentTime);
    }
    void DisplayTime(float TimeToDisplay)
    {
        if (TimeToDisplay < 0)
        {
            TimeToDisplay = 0;
        }
        float mins = Mathf.FloorToInt(TimeToDisplay / 60);
        float secs = Mathf.FloorToInt(TimeToDisplay % 60);

        countdownText.text = string.Format("{0:00}{1:00}", mins,"Ка");

    }
}
