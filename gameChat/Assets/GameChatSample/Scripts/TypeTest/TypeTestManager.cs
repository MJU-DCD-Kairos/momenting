using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Newtonsoft.Json;
//using System.IO;
using System;

public class TypeTestManager : MonoBehaviour
{
    //txt������ �ܺο��� �ν����Ϳ��� ���� ������ �� �ֵ��� ����
    public TextAsset txt;

    //�ؽ�Ʈ�� �־��� UI�ؽ�Ʈ ������Ʈ�� �ν����ͷ� �����ޱ����� ����
    public int order;
    public Text QNumber;
    public Text QContents;
    public Text answerA;
    public Text answerB;

    //public int order;

    //txt������ �� ������ �ν����ͻ󿡼� �Է��ϱ� ���� �ۺ� ���� ����
    public int tableSize;

    //�� ���� ������ Ŭ���� ����
    [System.Serializable]
    public class TypeTest
    {
        public int Num;
        public string QNumber;
        public string QContents;
        public string answerA;
        public string answerB;
    }

    //����Ʈ�� ������ Ŭ���� ����
    [System.Serializable]
    public class MbtiList
    {
        public TypeTest[] Mbti;
    }

    //�� Ŭ������ ������� �迭 ���� ����
    public MbtiList typeQuestionList = new MbtiList();


    // Start is called before the first frame update
    void Start()
    {
        ReadTXT();
        PrintQuestion();
    }

    //���������� txt������ �Ľ��ؼ� �迭������ �����ϴ� �Լ� �ۼ�
    void ReadTXT()
    {
        //������ txt������ �ǰ� ���ʹ����� �Ľ�
        string[] TXTdata = txt.text.Split(new string[] { "\n", "\t" }, StringSplitOptions.None);

        typeQuestionList.Mbti = new TypeTest[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            typeQuestionList.Mbti[i] = new TypeTest();
            typeQuestionList.Mbti[i].Num = (i+1);
            typeQuestionList.Mbti[i].QNumber = (TXTdata[4 * i]);
            typeQuestionList.Mbti[i].QContents = (TXTdata[4 * i + 1]);
            typeQuestionList.Mbti[i].answerA = (TXTdata[4 * i + 2]);
            typeQuestionList.Mbti[i].answerB = (TXTdata[4 * i + 3]);
        }


    }
 

    public void PrintQuestion()
    {
        //Debug.Log(order);
        //Debug.Log(typeQuestionList.Mbti[order].QNumber);
        //Debug.Log(typeQuestionList.Mbti[order].QContents);
        //Debug.Log(typeQuestionList.Mbti[order].answerA);
        //Debug.Log(typeQuestionList.Mbti[order].answerB);

        QNumber.text = typeQuestionList.Mbti[order].QNumber;
        QContents.text = typeQuestionList.Mbti[order].QContents;
        answerA.text = typeQuestionList.Mbti[order].answerA;
        answerB.text = typeQuestionList.Mbti[order].answerB;

    }


    public void OnClick_Previous()
    {
        if (order == 1)
        {
            order = 0;
            GameObject.Find("Btn_Previous").gameObject.SetActive(false);
        }
        else if (order == 2)
        {
            order = 1;
        }
        else if (order == 3)
        {
            order = 2;
        }
        else if (order == 4)
        {
            order = 3;
        }
        else if (order == 5)
        {
            order = 4;
        }
        else if (order == 6)
        {
            order = 5;
        }
        else if (order == 7)
        {
            order = 6;
        }
        else if (order == 8)
        {
            order = 7;
        }
        else if (order == 9)
        {
            order = 8;
        }
        else if (order == 10)
        {
            order = 9;
        }
        else if (order == 11)
        {
            order = 10;
            Text result = GameObject.Find("Btn_Next").transform.GetChild(0).gameObject.GetComponent<Text>();
            result.text = "����";
        }

        PrintQuestion();
    }
    


    public void OnClick_Next()
    {
        if (order == 0)
        {
            order = 1;
            GameObject.Find("Test").transform.Find("Btn_Previous").gameObject.SetActive(true);
        }
        else if (order == 1)
        {
            order = 2;
        }
        else if (order == 2)
        {
            order = 3;
        }
        else if (order == 3)
        {
            order = 4;
        }
        else if (order == 4)
        {
            order = 5;
        }
        else if (order == 5)
        {
            order = 6;
        }
        else if (order == 5)
        {
            order = 6;
        }
        else if (order == 6)
        {
            order = 7;
        }
        else if (order == 7)
        {
            order = 8;
        }
        else if (order == 8)
        {
            order = 9;
        }
        else if (order == 9)
        {
            order = 10;
        }
        else if (order == 10)
        {
            order = 11;
            Text result = GameObject.Find("Btn_Next").transform.GetChild(0).gameObject.GetComponent<Text>();
            result.text = "��� ����";
        }
        else if (order == 11)
        {
            order = 0;
            GameObject.Find("Test").SetActive(false);
            GameObject.Find("TypeTest").transform.Find("Result").gameObject.SetActive(true);
        }

        PrintQuestion();

    }
}
