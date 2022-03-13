using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Newtonsoft.Json;
//using System.IO;
using System;

public class TypeTestManager : MonoBehaviour
{
    //txt파일을 외부에서 인스펙터에서 직접 참조할 수 있도록 생성
    public TextAsset txt;

    //텍스트를 넣어줄 UI텍스트 오브젝트를 인스펙터로 참조받기위한 선언
    public int order;
    public Text QNumber;
    public Text QContents;
    public Text answerA;
    public Text answerB;

    //public int order;

    //txt파일의 행 개수를 인스펙터상에서 입력하기 위한 퍼블릭 변수 선언
    public int tableSize;

    //각 값을 보유할 클래스 생성
    [System.Serializable]
    public class TypeTest
    {
        public int Num;
        public string QNumber;
        public string QContents;
        public string answerA;
        public string answerB;
    }

    //리스트를 보유할 클래스 생성
    [System.Serializable]
    public class MbtiList
    {
        public TypeTest[] Mbti;
    }

    //각 클래스를 기반으로 배열 변수 생성
    public MbtiList typeQuestionList = new MbtiList();


    // Start is called before the first frame update
    void Start()
    {
        ReadTXT();
        PrintQuestion();
    }

    //본격적으로 txt파일을 파싱해서 배열정보로 생성하는 함수 작성
    void ReadTXT()
    {
        //참조한 txt파일을 탭과 엔터단위로 파싱
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
            result.text = "다음";
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
            result.text = "결과 보기";
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
