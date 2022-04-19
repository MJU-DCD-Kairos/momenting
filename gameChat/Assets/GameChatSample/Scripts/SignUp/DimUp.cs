using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DimUp : MonoBehaviour
{
    public Scrollbar scrollBar;
    public GameObject DimUpImg;
    public Image image;

    private void Start()
    {
        scrollBar.value = 0.00001f;
    }
    public void Update()
    {
        if (scrollBar.value < 0.99999f)
        {

            DimUpImg.gameObject.SetActive(true);
            
        }
        if (scrollBar.value > 0.99999f)
        {
            DimUpImg.gameObject.SetActive(false);
            //StartCoroutine(FadeCo());
            
        }
    }
    IEnumerator FadeCo()
    {
        float fadecount = 0;
        while (fadecount > 1.0f )
        {
            fadecount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(255, 255,255, fadecount);
        }
        
    }
}
