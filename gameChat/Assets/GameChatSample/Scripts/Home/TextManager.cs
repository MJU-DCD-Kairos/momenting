using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.UI;

public class TextManager : MonoBehaviour

   
{
    
    public int stringLength;
    public Text inputText;

    public void StringTransfer(String inputString, int stringLength)
    {
        // inputString : �Է� ���ڿ�(�̺κ��� ����ȣ��� ��ü ����)
        // stringLength : �ڸ� ����
        string msg = inputString;
        int textlength = inputString.Length;
        if (textlength > stringLength)
        {
            /* while (textlength > stringLength)
             {
                 for (int i = 0; i < msg.Length; i++)
                 {
                     msg = msg + msg.Substring(i, 1);
                     by = Encoding.GetEncoding(949).GetBytes(msg).Length;
                     if (by > 20) break;
                 }
             }
             msg = msg.Trim() + "..";*/

            //���ϴ� ���� ������ �ؽ�Ʈ�� ����

            string newString = inputText.text.Remove(stringLength, inputText.text.Length - stringLength);
            inputText.text = newString + "...";

        }
    }

    public void Start()
    {
        StringTransfer(inputText.text, stringLength);
        
    }
    

}
