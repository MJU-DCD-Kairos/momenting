using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SUCM;

public class Togglebaby : MonoBehaviour
{
    public Text toggletext;
    //public int limit = GameObject.Find("SignUpSceneManager").GetComponent<SignUpSceneManager>().a;
    

    //���ӿ�����Ʈ�� ������ �����ͼ� ī�װ��� �Ǻ�
    //public void getKeywordList()
    //{
    //    Debug.Log("����  " + SignUpSceneManager.sum);
    //    if (SignUpSceneManager.sum < 5)
    //    {
    //        Debug.Log(this.name);
    //        if (this.name == "Btn_Keyword")
    //        {
    //            if (this.GetComponent<Toggle>().isOn == true)
    //            {
    //                categoryAddKeyword("Tendency");
    //            }
    //            else
    //            {
    //                categorySubKeyword("Tendency");

    //            }
    //            Debug.Log(SignUpSceneManager.Tendency[0] + "////" + SignUpSceneManager.Tendency[1] + "////" + SignUpSceneManager.Tendency[2]
    //                + "////" + SignUpSceneManager.Tendency[3] + "////" + SignUpSceneManager.Tendency[4]);

    //            //SignUpSceneManager.Tendency
    //        }
    //        else if (this.name == "InBtn_Keyword")
    //        {

    //            //SignUpSceneManager.Interests
    //        }
    //        else
    //        {

    //            //SignUpSceneManager.Lifestyle
    //        }
    //    }
    //    else
    //    {
    //        if (this.GetComponent<Toggle>().isOn == false) 
    //        { 
    //            for (int i = 0; i < SignUpSceneManager.Tendency.Length; i++)
    //                {
    //                    if (SignUpSceneManager.Tendency[i] == toggletext.text)
    //                    {
    //                        SignUpSceneManager.Tendency[i] = "";
    //                        Debug.Log(SignUpSceneManager.Tendency[i] + i + "������");
    //                        Debug.LogError(SignUpSceneManager.Tendency.Length);
    //                        //Debug.Log(toggletext.text+"������");
    //                        SignUpSceneManager.sum -= 1;
    //                        break;

    //                    }
    //                }
    //        }
    //            Debug.Log("Ű���� ���� 5�̻� ////" + SignUpSceneManager.sum);
    //        this.GetComponent<Toggle>().isOn = false;
    //    }
    //}
    
    //public void categoryAddKeyword(string category)
    //{
    //    for (int a = 0; a < 5; a++)
    //    {
    //        if (SignUpSceneManager(SignUpSceneManager + "." + category + "[" + a + "]") == null || (SignUpSceneManager + "." + category + "[" + a + "]") == "")
    //        {
    //            SignUpSceneManager + "." + category + "[" + i + "]" = toggletext.text;
    //            Debug.Log(toggletext.text + a + "��");
    //            SignUpSceneManager.sum += 1;
    //            Debug.Log("���ϰ� �� ����  " + SignUpSceneManager.sum);
    //            break;
    //        }
    //    }
    //}

    //public void categorySubKeyword(string category)
    //{
    //    for (int i = 0; i < 5; i++)
    //    {
    //        if (SignUpSceneManager + "." + category + "[" + i + "]" == toggletext.text)
    //        {
    //            (SignUpSceneManager + "." + category + "["+i+ "]" )= "";
    //            Debug.Log((SignUpSceneManager + "." + category + "[" + i + "]") + i + "������");
    //            //Debug.Log(toggletext.text+"������");
    //            SignUpSceneManager.sum -= 1;
    //            break;

    //        }
    //    }
    //}
    /*
    public void thishtihi()
    {
        if (limit < 5)
        {
            GameObject.Find("SignUpSceneManager").GetComponent<SignUpSceneManager>().Tendency[limit] = toggletext.text;
            Debug.Log(toggletext.text);

            limit += 1;

        }
        else
        {
            Debug.Log("Ű���� ���� ������ �ʰ��Ͽ���");
        }
    }*/

}
