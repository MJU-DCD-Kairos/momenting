using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KakaoSystem : MonoBehaviour
{
    private AndroidJavaObject ajo;

    // Start is called before the first frame update
    private void Start()
    {
        ajo = new AndroidJavaObject("com.DefaultCompany.kakao.UKakao");
    }

    // Update is called once per frame
    public void login()
    {
        ajo.Call("KakaoLogin");
    }
}
