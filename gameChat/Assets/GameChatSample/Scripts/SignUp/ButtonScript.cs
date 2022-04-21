using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

// ButtonManger에 적용할 스크립트
public class ButtonScript : MonoBehaviour
{
    // 버튼 누를 시
    public void OnButtonPress() {
        Debug.Log("버튼 누름");

        // CircleAvartar에 적용된 스크립트의 updateProfileImage 함수를 호출함
        GameObject.Find("CircleAvartar").GetComponent<CircleAvartar>().updateProfileImage();
    }
}