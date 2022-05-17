using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GKSManager : MonoBehaviour
{
    public GameObject GKS;
    Animation anim;
    public void GKEnable()
    {
        GKS.GetComponent<Animator>().Play("MultiChatBardis");



    
    }
    public void GKDisable()
    {
        GKS.GetComponent<Animator>().Play("MultiChatBar");


    }
}
