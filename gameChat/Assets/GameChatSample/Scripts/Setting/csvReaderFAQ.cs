using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class csvReaderFAQ : MonoBehaviour
{
    //csv������ �ܺο��� �ν����Ϳ��� ���� ������ �� �ֵ��� ����
    public TextAsset csvfile;

    //FAQ�� �־��� UI�ؽ�Ʈ ������Ʈ�� �ν����ͷ� �����ޱ����� ����
    public Text FAQtitle;
    public Text FAQbody;

    //csv������ �� ������ �ν����ͻ󿡼� �Է��ϱ� ���� �ۺ� ���� ����
    public int tableSize;

    //�� ���� ������ Ŭ���� ����
    [System.Serializable]
    public class FAQ
    {
        public int Num;
        public string FAQtitle;
        public string FAQbody;
    }


    //����Ʈ�� ������ Ŭ���� ����
    public class FAQList
    {
        public FAQ[] FAQL;
    }

    //�� Ŭ������ ������� �迭 ���� ����
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

    //���������� CSV������ �Ľ��ؼ� �迭������ �����ϴ� �Լ� �ۼ�
    void ReadCSV()
    {
        //������ CSV������ ,�� ���ʹ����� �Ľ�
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

