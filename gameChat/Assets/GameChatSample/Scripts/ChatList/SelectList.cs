using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectList : MonoBehaviour
{
    public Toggle check;
    public string selected;

    public void OnMouseUpAsButton()
    {
        if (check.isOn == true) //토글의 isOn 값이 true일 때(선택했을 때)
        {
            //OnSelect();
            GameObject.FindGameObjectWithTag("ListManager").GetComponent<PersonalListEdit>().CheckedToggles.Add(check); //체크된 토글 리스트에 추가
        }
        else //토글의 isOn 값이 false일 때(선택해제됐을 때)
        {
            //OffSelect();
            GameObject.FindGameObjectWithTag("ListManager").GetComponent<PersonalListEdit>().CheckedToggles.Remove(check); //체크된 토글 리스트에서 제거
        }

        GameObject.FindGameObjectWithTag("ListManager").GetComponent<PersonalListEdit>().OnEnableBtn();
        GameObject.FindGameObjectWithTag("ListManager").GetComponent<PersonalListEdit>().changeLabel(); //선택 리스트 개수 레이블 
    }

    

    public void OnSelect()
    {
        selected = "selected";

    }

    public void OffSelect()
    {
        selected = "";
    }

}
