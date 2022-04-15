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

    public Text CurrentIndex;

    //public int SavedAnswer;

    //txt파일의 행 개수를 인스펙터상에서 입력하기 위한 퍼블릭 변수 선언
    //public int tableSize;

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
        PrintType();


    }

    //본격적으로 txt파일을 파싱해서 배열정보로 생성하는 함수 작성
    void ReadTXT()
    {
        //참조한 txt파일을 탭과 엔터단위로 파싱
        string[] TXTdata = txt.text.Split(new string[] { "\n", "\t" }, StringSplitOptions.None);

        int tableSize = 12;
        typeQuestionList.Mbti = new TypeTest[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            typeQuestionList.Mbti[i] = new TypeTest();
            typeQuestionList.Mbti[i].Num = (i + 1);
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
        toggle1.isOn = false;
        toggle2.isOn = false;

        if (order == 1)
        {
            GameObject.Find("Btn_Previous").gameObject.SetActive(false);
            if(E_I == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 0;
            E_I = 0;
        }
        else if (order == 2) 
        {
            if (E_I == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 1; E_I = E_I - A2; }
        else if (order == 3) 
        {
            if (E_I == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 2; E_I = E_I - A3; }
        else if (order == 4) 
        {
            if (N_S == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 3; N_S = N_S - A4; }
        else if (order == 5) 
        {
            if (N_S == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 4; N_S = N_S - A5; }
        else if (order == 6) 
        {
            if (N_S == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 5; N_S = N_S - A6; }
        else if (order == 7) 
        {
            if (T_F == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 6; T_F = T_F - A7; }
        else if (order == 8)
        {
            if (T_F == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 7; T_F = T_F - A8; }
        else if (order == 9)
        {
            if (T_F == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 8; T_F = T_F - A9; }
        else if (order == 10) 
        {
            if (P_J == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 9; P_J = P_J - A10; }
        else if (order == 11)
        {
            if (P_J == 0) { toggle1.isOn = true; }
            else { toggle2.isOn = true; }
            order = 10;
            P_J = P_J - A11;
            Text result = GameObject.Find("Btn_Next").transform.GetChild(0).gameObject.GetComponent<Text>();
            result.text = "다음";
        }

        PrintQuestion();
        changeIndex();


        //toggle1.isOn = false;
        //toggle2.isOn = false;
    }



    public void OnClick_Next()
    {
        if (toggle1.isOn || toggle2.isOn)
        {
            if (order == 0)
            {
                E_I = E_I + A1;
                order = 1;
                GameObject.Find("Test").transform.Find("Btn_Previous").gameObject.SetActive(true);
            }
            else if (order == 1) { order = 2; E_I = E_I + A2; }
            else if (order == 2) { order = 3; E_I = E_I + A3; }
            else if (order == 3) { order = 4; N_S = N_S + A4; }
            else if (order == 4) { order = 5; N_S = N_S + A5; }
            else if (order == 5) { order = 6; N_S = N_S + A6; }
            else if (order == 6) { order = 7; T_F = T_F + A7; }
            else if (order == 7) { order = 8; T_F = T_F + A8; }
            else if (order == 8) { order = 9; T_F = T_F + A9; }
            else if (order == 9) { order = 10; P_J = P_J + A10; }
            else if (order == 10)
            {
                order = 11;
                P_J = P_J + A11;
                Text result = GameObject.Find("Btn_Next").transform.GetChild(0).gameObject.GetComponent<Text>();
                result.text = "결과 보기";
            }
            else if (order == 11)
            {
                order = 0;
                P_J = P_J + A12;
                GameObject.Find("Test").SetActive(false);
                GameObject.Find("TypeTest").transform.Find("MyType").gameObject.SetActive(true);
                TypeResult();
                PrintType();
                //Debug.Log(MbtiType);
            }

            PrintQuestion();
            changeIndex();

            //버튼 선택되지 않은 상태로 만들기
            toggle1.isOn = false;
            toggle2.isOn = false;
        }
        
    }


    public void changeIndex()
    {
        int idx = 1;
        idx += order;
        CurrentIndex.text = idx.ToString();
    }

    public Toggle toggle1;
    public Toggle toggle2;
    private void Awake()
    {
        toggle1.onValueChanged.AddListener(Answer1Btn_TypeTest);
        toggle2.onValueChanged.AddListener(Answer2Btn_TypeTest);

    }

    private Color color;
    //public int Answer;
    //각 질문들에 대한 답의 값을 저장
    public int A1;
    public int A2;
    public int A3;
    public int A4;
    public int A5;
    public int A6;
    public int A7;
    public int A8;
    public int A9;
    public int A10;
    public int A11;
    public int A12;
    //활성화된 버튼의 텍스트 색상 화이트로 바꾸기
    public void Answer1Btn_TypeTest(bool select)
    {

        if (toggle1.isOn)
        {
            if (order == 0) { A1 = 0; }
            else if (order == 1) { A2 = 0; }
            else if (order == 2) { A3 = 0; }
            else if (order == 3) { A4 = 0; }
            else if (order == 4) { A5 = 0; }
            else if (order == 5) { A6 = 0; }
            else if (order == 6) { A7 = 0; }
            else if (order == 7) { A8 = 0; }
            else if (order == 8) { A9 = 0; }
            else if (order == 9) { A10 = 0; }
            else if (order == 10) { A11 = 0; }
            else if (order == 11) { A12 = 0; }
            ColorUtility.TryParseHtmlString("#ffffff", out color);
            answerA.color = color;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#1f1f1f", out color);
            answerA.color = color;
        }
        
    }
    public void Answer2Btn_TypeTest(bool select)
    {
        if (toggle2.isOn)
        {
            if (order == 0) { A1 = 1; }
            else if (order == 1) { A2 = 1; }
            else if (order == 2) { A3 = 1; }
            else if (order == 3) { A4 = 1; }
            else if (order == 4) { A5 = 1; }
            else if (order == 5) { A6 = 1; }
            else if (order == 6) { A7 = 1; }
            else if (order == 7) { A8 = 1; }
            else if (order == 8) { A9 = 1; }
            else if (order == 9) { A10 = 1; }
            else if (order == 10) { A11 = 1; }
            else if (order == 11) { A12 = 1; }
            ColorUtility.TryParseHtmlString("#ffffff", out color);
            answerB.color = color;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#1f1f1f", out color);
            answerB.color = color;
        }


    }
    public int E_I;
    public int N_S;
    public int T_F;
    public int P_J;

    public string MbtiType; //도출한 유형을 인스펙터에서 볼 수 있도록 선언
    public void TypeResult()
    {
        if ((E_I <= 1) && (N_S <=1) && (T_F <= 1) && (P_J <= 1)) { MbtiType = "ENTP"; }
        else if ((E_I <= 1) && (N_S <= 1) && (T_F > 1) && (P_J <= 1)) { MbtiType = "ENFP"; }
        else if ((E_I <= 1) && (N_S <= 1) && (T_F > 1) && (P_J > 1)) { MbtiType = "ENFJ"; }
        else if ((E_I <= 1) && (N_S <= 1) && (T_F <= 1) && (P_J > 1)) { MbtiType = "ENTJ"; }
        else if ((E_I <= 1) && (N_S > 1) && (T_F <= 1) && (P_J <= 1)) { MbtiType = "ESTP"; }
        else if ((E_I <= 1) && (N_S > 1) && (T_F > 1) && (P_J <= 1)) { MbtiType = "ESFP"; }
        else if ((E_I <= 1) && (N_S > 1) && (T_F > 1) && (P_J > 1)) { MbtiType = "ESFJ"; }
        else if ((E_I <= 1) && (N_S > 1) && (T_F <= 1) && (P_J > 1)) { MbtiType = "ESTJ"; }
        else if ((E_I > 1) && (N_S <= 1) && (T_F <= 1) && (P_J <= 1)) { MbtiType = "INTP"; }
        else if ((E_I > 1) && (N_S <= 1) && (T_F > 1) && (P_J <= 1)) { MbtiType = "INFP"; }
        else if ((E_I > 1) && (N_S <= 1) && (T_F > 1) && (P_J > 1)) { MbtiType = "INFJ"; }
        else if ((E_I > 1) && (N_S <= 1) && (T_F <= 1) && (P_J > 1)) { MbtiType = "INTJ"; }
        else if ((E_I > 1) && (N_S > 1) && (T_F <= 1) && (P_J <= 1)) { MbtiType = "ISTP"; }
        else if ((E_I > 1) && (N_S > 1) && (T_F > 1) && (P_J <= 1)) { MbtiType = "ISFP"; }
        else if ((E_I > 1) && (N_S > 1) && (T_F > 1) && (P_J > 1)) { MbtiType = "ISFJ"; }
        else if ((E_I > 1) && (N_S > 1) && (T_F <= 1) && (P_J > 1)) { MbtiType = "ISTJ"; }
    }

    public GameObject Content; //프리팹 인스턴스화 해줄 위치 참조
    public void PrintType()
    {
        //인스펙터에 입력한 엠비티아이와 일치하는 프리팹 로드

        if (MbtiType == "")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_None")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "INTP")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_INTP")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "INTJ")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_INTJ")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "INFP")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_INFP")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "INFJ")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_INFJ")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ISTP")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ISTP")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ISTJ")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ISTJ")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ISFP")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ISFP")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ISFJ")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ISFJ")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ENTP")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ENTP")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ENTJ")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ENTJ")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ENFP")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ENFP")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ENFJ")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ENFJ")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ESTP")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ESTP")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ESTJ")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ESTJ")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ESFP")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ESFP")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if (MbtiType == "ESFJ")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_ESFJ")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }

    }

    public void Redo()
    {
        E_I = 0; N_S = 0; T_F = 0; P_J = 0;
        order = 0;
        Text result = GameObject.Find("Btn_Next").transform.GetChild(0).gameObject.GetComponent<Text>();
        result.text = "다음";
        GameObject.Find("Test").transform.Find("Btn_Previous").gameObject.SetActive(false);
    }
        


}
