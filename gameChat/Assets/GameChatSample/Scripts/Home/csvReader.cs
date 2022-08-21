using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FireStoreScript;

public class csvReader : MonoBehaviour
{
    //오늘의 질문 답변 안했을 때 활성화할 게임오브젝트를 인스펙터에서 직접 참조
    public GameObject TQpopUp;


    //csv파일을 외부에서 인스펙터에서 직접 참조할 수 있도록 생성
    public TextAsset csvfile;

    //오늘의 질문을 넣어줄 UI텍스트 오브젝트를 인스펙터로 참조받기위한 선언
    public Text Question;
    public Text answerA;
    public Text answerB;

    //오늘의 질문 답변 1,2 버튼 오브젝트를 받아옴
    public Button answerBtnA;
    public Button answerBtnB;


    //CSV파일의 행 개수를 인스펙터상에서 입력하기 위한 퍼블릭 변수 선언
    public int tableSize;
    
    //각 값을 보유할 클래스 생성
    [System.Serializable]
    public class TodayQuestion
    {
        public int Num;
        public string QuestionT;
        public string answerA;
        public string answerB;
    }

    //리스트를 보유할 클래스 생성
    [System.Serializable]
    public class TQList
    {
        public TodayQuestion[] TQL; 
    }

    //각 클래스를 기반으로 배열 변수 생성
    public TQList todayQuestionList = new TQList();

    
    // Start is called before the first frame update
    void Start()
    {
        ReadCSV();
        PrintQuestionT();
        if (FirebaseManager.myTqAnswer == 0)
        {
            TQpopUp.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //본격적으로 CSV파일을 파싱해서 배열정보로 생성하는 함수 작성
    void ReadCSV(){
        //참조한 CSV파일을 ,와 엔터단위로 파싱
        string[] CSVdata = csvfile.text.Split(new string[] {",", "\n"}, StringSplitOptions.None);

        todayQuestionList.TQL = new TodayQuestion[tableSize];

        for(int i = 0; i<tableSize-1; i++)
        {
            todayQuestionList.TQL[i] = new TodayQuestion();
            todayQuestionList.TQL[i].Num = i+1;
            todayQuestionList.TQL[i].QuestionT = (CSVdata[4*(i+1)+1]);
            todayQuestionList.TQL[i].answerA = (CSVdata[4*(i+1)+2]);
            todayQuestionList.TQL[i].answerB = (CSVdata[4*(i+1)+3]);
        }

        
    }

    void PrintQuestionT()
    {
        //오늘의 질문을 랜덤으로 뽑는 함수(현진 삭제- DB의 인덱스를 가져오는 것으로 변경함)
        //int tqlNum = UnityEngine.Random.Range(1,109);
        int tqlNum = FirebaseManager.todayQIndex;

        Debug.Log(tqlNum);
        Debug.Log(todayQuestionList.TQL[tqlNum].QuestionT);
        Debug.Log(todayQuestionList.TQL[tqlNum].answerA);
        Debug.Log(todayQuestionList.TQL[tqlNum].answerB);

        Question.text = todayQuestionList.TQL[tqlNum].QuestionT;
        answerA.text = todayQuestionList.TQL[tqlNum].answerA;
        answerB.text = todayQuestionList.TQL[tqlNum].answerB;
    }

    //오늘의 질문 답변 1을 눌렀을 때 FBM정보 업데이트 함수 호출
    public void pressBtn1()
    {
        FirebaseManager.setTqAnswer1();
    }

    //오늘의 질문 답변 2를 눌렀을 때 FBM정보 업데이트 함수 호출
    public void pressBtn2()
    {
        FirebaseManager.setTqAnswer2();
    }

    //텍스트 버튼으로 오늘의 질문 확인하기를 눌렀을 때, 사용자의 답변 상태 분기에 따라 UI를 처리하는 함수
    public void loadTqResult()
    {
        //답변을 하지 않았다면 두개의 버튼을 모두 활성화 함
        if (FirebaseManager.myTqAnswer == 0)
        {
            answerBtnA.interactable = true;
            answerBtnB.interactable = true;
        }
        else if(FirebaseManager.myTqAnswer == 1)
        {
            answerBtnA.interactable = false;
            answerBtnB.interactable = false;
            //버튼1의 스프라이트를 변경해주기(or 컬러 변경해주기)

        }
        else if (FirebaseManager.myTqAnswer == 2)
        {
            answerBtnA.interactable = false;
            answerBtnB.interactable = false;
            //버튼 2의 스프라이트를 변경해주기(or 컬러 변경해주기)
        }
    }
}
