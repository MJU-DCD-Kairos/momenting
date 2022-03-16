using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class TypeDescription : MonoBehaviour
{
    /*

    [System.Serializable]

    public class TypeList
    {

        public TypeList(string _Mbti, string _TypeName, string _Description1, string _Description2, string _Chemi1, string _Chemi2, string _Chemi3)
        { Mbti = _Mbti; TypeName = _TypeName; Description1 = _Description1; Description2 = _Description2; Chemi1 = _Chemi1; Chemi2 = _Chemi2; Chemi3 = _Chemi3; }
       

        public string Mbti;
        public string TypeName;
        public string Description1;
        public string Description2;
        public string Chemi1;
        public string Chemi2;
        public string Chemi3;
    }

    //txt파일을 인스펙터에서 참조하도록 생성
    public TextAsset txt;

    //텍스트를 넣어줄 UI텍스트 오브젝트를 인스펙터로 참조받기위한 선언
    //public Text TypeName;
    //public Text Description1;
    //public Text Description2;
    public Text Chemi1;
    public Text Chemi2;
    public Text Chemi3;

    public int order;


    //리스트 배열 생성
    public List<TypeList> MbtiList;

    void Start()
    {
        ReadTXT();
        PrintDescription();

    }

    void ReadTXT() //참조한 txt파일 파싱하여 리스트에 넣기
    {
        string[] line = txt.text.Substring(0, txt.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            //MbtiList.Add(new TypeList(row[0], row[1], row[2], row[3], row[4], row[5], row[6]));
            MbtiList.Add(new TypeList(row[0], row[1], row[2].Replace("\\n", "\n"), row[3].Replace("\\n", "\n"), row[4], row[5], row[6]));


        }

    }

    public void PrintDescription()
    {
        //TypeName.text = "<color=#F55637>" + "\" " + "</color>" + MbtiList[order].TypeName + "<color=#F55637>" + " \"" + "</color>";
        //TypeName.text = MbtiList[order].TypeName;
        //Description1.text = MbtiList[order].Description1;
        //Description2.text = MbtiList[order].Description2;

        Chemi1.text = MbtiList[order].Chemi1;
        Chemi2.text = MbtiList[order].Chemi2;
        Chemi3.text = MbtiList[order].Chemi3;
    }
    */
    
    public string MbtiType; //일단은 인스펙터에서 MBTI 입력하여 띄우는 방식으로 함. 추후 DB 들어가면 수정 필요.
    public GameObject Content; //프리팹 인스턴스화 해줄 위치 참조

    void Start()
    {
        PrintType();
    }

    public void PrintType()
    {
        //인스펙터에 입력한 엠비티아이와 일치하는 프리팹 로드

        if (MbtiType == "INTP")
        {
            GameObject MbtiResult = Instantiate(Resources.Load("Prefabs/Result_INTP")) as GameObject;
            MbtiResult.transform.SetParent(Content.transform, false);
        }
        else if(MbtiType == "INTJ")
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
    
}
