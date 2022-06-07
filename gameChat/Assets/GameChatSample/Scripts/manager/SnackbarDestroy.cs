using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackbarDestroy : MonoBehaviour
{
    public GameObject Snackbar;
    // Start is called before the first frame update
    public float DestroyTime = 4.3f;
    void OnEnable()
    {
        Destroy(Snackbar, DestroyTime);
        Debug.LogError("ªÁ∂Û¡Æ!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
