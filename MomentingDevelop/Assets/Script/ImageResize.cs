using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageResize : MonoBehaviour
{
    //컴포넌트에 따라 인스펙터상에서 사이즈 조절 가능하도록 선언
    public float size = 384;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }


    // Update is called once per frame
    void Update()
    {
        
        //소스이미지 원래 사이즈로 세팅
        GetComponent<Image>().SetNativeSize();

        var RectTransform = transform as RectTransform;

        //넓이가 더 짧으면 넓이를 인스펙터상에서 입력한 size로 바꿈
        if (RectTransform.sizeDelta.x < RectTransform.sizeDelta.y)
        {
            RectTransform.sizeDelta = new Vector2(size, RectTransform.sizeDelta.y);
        }

        //높이가 더 짧으면 높이를 인스펙터상에서 입력한 size로 바꿈
        else
        {
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, size);
        }

    }

    
}
