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
        if (check.isOn == true) //����� isOn ���� true�� ��(�������� ��)
        {
            //OnSelect();
            GameObject.FindGameObjectWithTag("ListManager").GetComponent<PersonalListEdit>().CheckedToggles.Add(check); //üũ�� ��� ����Ʈ�� �߰�
        }
        else //����� isOn ���� false�� ��(������������ ��)
        {
            //OffSelect();
            GameObject.FindGameObjectWithTag("ListManager").GetComponent<PersonalListEdit>().CheckedToggles.Remove(check); //üũ�� ��� ����Ʈ���� ����
        }

        GameObject.FindGameObjectWithTag("ListManager").GetComponent<PersonalListEdit>().OnEnableBtn();
        GameObject.FindGameObjectWithTag("ListManager").GetComponent<PersonalListEdit>().changeLabel(); //���� ����Ʈ ���� ���̺� 
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
