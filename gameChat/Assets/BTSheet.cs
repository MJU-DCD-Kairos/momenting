using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSheet : MonoBehaviour
{
    public GameObject BTS;
    

    public void ShowHideMenu()
    {
        if (BTS != null)
        {
            Animator animator = BTS.GetComponent<Animator>();
            if (animator != null)
            {

                bool isOpen = animator.GetBool("show");
                animator.SetBool("show", !isOpen); 
                
            }

        }
       
    }
}

