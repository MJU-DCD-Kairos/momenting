using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public void ServiceURL()
    {
        Application.OpenURL("https://www.notion.so/a34044f43cfb49b38c648b700e723495");
    }

    public void privacypolicyURL()
    {
        Application.OpenURL("https://www.notion.so/cf8bec1cd081488a9fb35dae231ec61d");
    }

    public void QuestionsURL()
    {
        Application.OpenURL("https://forms.gle/MK17MPGAquW6VD986");
    }
}