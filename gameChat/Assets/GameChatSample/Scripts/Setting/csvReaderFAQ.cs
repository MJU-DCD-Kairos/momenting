using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class csvReaderFAQ : MonoBehaviour
{
    //csv파일을 외부에서 인스펙터에서 직접 참조할 수 있도록 생성
    public TextAsset csvfile;

    //FAQ를 넣어줄 UI텍스트 오브젝트를 인스펙터로 참조받기위한 선언
    public Text FAQtitle;
    public Text FAQbody;

    //csv파일의 행 개수를 인스펙터상에서 입력하기 위한 퍼블릭 변수 선언
    public int tableSize;

    //각 값을 보유할 클래스 생성
    [System.Serializable]
    public class FAQ
    {
        public int Num;
        public string FAQtitle;
        public string FAQbody;
    }


    //리스트를 보유할 클래스 생성
    public class FAQList
    {
        public FAQ[] FAQL;
    }

    //각 클래스를 기반으로 배열 변수 생성
    public FAQList faqList = new FAQList();

    // Start is called before the first frame update
    void Start()
    {
        ReadCSV();
        PrintQuestionT();

    }

    // Update is called once per frame
    void Update()
    {

    }

    //본격적으로 CSV파일을 파싱해서 배열정보로 생성하는 함수 작성
    void ReadCSV()
    {
        //참조한 CSV파일을 ,와 엔터단위로 파싱
        string[] CSVdata = csvfile.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        faqList.FAQL = new FAQ[tableSize];

        for (int i = 0; i < tableSize - 1; i++)
        {
            faqList.FAQL[i] = new FAQ();
            faqList.FAQL[i].Num = i + 1;
            faqList.FAQL[i].FAQtitle = (CSVdata[3 * (i + 1) + 1]);
            faqList.FAQL[i].FAQbody = (CSVdata[3 * (i + 1) + 2]);
        }

    }
    void PrintQuestionT()
    {
        //int faqlNum = UnityEngine.Random.Range(1, 20);

        //Debug.Log(faqlNum);
        //Debug.Log(faqList.FAQL[faqlNum].FAQtitle);
        //Debug.Log(faqList.FAQL[faqlNum].FAQbody);

        FAQtitle.text = faqList.FAQL[3].FAQtitle;
        FAQbody.text = faqList.FAQL[4].FAQbody;
    }
}

