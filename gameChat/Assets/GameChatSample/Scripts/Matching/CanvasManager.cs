using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject Canvas_inactive;
    public GameObject Canvas_active;


    public void Swtich()
    {
        Canvas_inactive.SetActive(false);
        Canvas_active.SetActive(true);

    }

    
}
