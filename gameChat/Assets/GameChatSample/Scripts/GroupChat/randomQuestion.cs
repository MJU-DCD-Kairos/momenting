using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class randomQuestion : MonoBehaviour
{

    //csv파일을 외부에서 인스펙터에서 직접 참조할 수 있도록 생성
    public TextAsset csvfile;

    //오늘의 질문을 넣어줄 UI텍스트 오브젝트를 인스펙터로 참조받기위한 선언
    public Text question;
    public Text answerA;
    public Text answerB;

    //CSV파일의 행 개수를 인스펙터상에서 입력하기 위한 퍼블릭 변수 선언
    public int tableSize;
    
    //각 값을 보유할 클래스 생성
    [System.Serializable]
    public class ChatQuestion
    {
        public int Num;
        public string chatQuestion;
        public string answerA;
        public string answerB;
    }

    //리스트를 보유할 클래스 생성
    [System.Serializable]
    public class CQList
    {
        public ChatQuestion[] CQL; 
    }

    //각 클래스를 기반으로 배열 변수 생성
    public CQList ChatQuestionList = new CQList();

    
    // Start is called before the first frame update
    void Start()
    {
        ReadCSV();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //본격적으로 CSV파일을 파싱해서 배열정보로 생성하는 함수 작성
    public void ReadCSV()
    {
        //참조한 CSV파일을 ,와 엔터단위로 파싱
        string[] CSVdata = csvfile.text.Split(new string[] {",", "\n"}, StringSplitOptions.None);

        ChatQuestionList.CQL = new ChatQuestion[tableSize];

        for(int i = 0; i<tableSize-1; i++)
        {
            ChatQuestionList.CQL[i] = new ChatQuestion();
            ChatQuestionList.CQL[i].Num = i+1;
            ChatQuestionList.CQL[i].chatQuestion = (CSVdata[4*(i+1)+1]);
            ChatQuestionList.CQL[i].answerA = (CSVdata[4*(i+1)+2]);
            ChatQuestionList.CQL[i].answerB = (CSVdata[4*(i+1)+3]);
        }

        
    }

    public void RandomQuestion()
    {
        int cqlnum = UnityEngine.Random.Range(1,107);
        
        Debug.Log(cqlnum);
        Debug.Log(ChatQuestionList.CQL[cqlnum].chatQuestion);
        Debug.Log(ChatQuestionList.CQL[cqlnum].answerA);
        Debug.Log(ChatQuestionList.CQL[cqlnum].answerB);

        question.text = "<color=#F55637>"+ "Q.  " + "</Color>" + ChatQuestionList.CQL[cqlnum].chatQuestion;
        answerA.text = ChatQuestionList.CQL[cqlnum].answerA;
        answerB.text = ChatQuestionList.CQL[cqlnum].answerB;
    }
}
