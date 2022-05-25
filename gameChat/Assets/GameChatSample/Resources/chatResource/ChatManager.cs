using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text;
using AS;
using groupchatManager;


namespace CM{

    public class ChatManager : MonoBehaviour

    {
    public GameObject MyArea, ElseArea, DateArea, GetDown, AIArea, RQArea, FQ;
        [Header("GetDown")]
        public Text gd;
        [Header("Prefab")]
        public RectTransform ContentRect;
        public Scrollbar scrollBar;
        public Toggle MineToggle;
        public GameObject getDownbtn;

        [Header("ellipsis")]
        public int stringLength;
        public Text inputText;
        [Header("Timer")]
        public Text TimerText;
        public float currentTime = 0f;
        public float startingTime = 1200f;
        [SerializeField]
        public Text countdownText;
        public bool ai = false;

        AreaScript LastArea;

        [Header("RandomQuestion")]
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


        //본격적으로 CSV파일을 파싱해서 배열정보로 생성하는 함수 작성
        public void ReadCSV()
        {
            //참조한 CSV파일을 ,와 엔터단위로 파싱
            string[] CSVdata = csvfile.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

            ChatQuestionList.CQL = new ChatQuestion[tableSize];

            for (int i = 0; i < tableSize - 1; i++)
            {
                ChatQuestionList.CQL[i] = new ChatQuestion();
                ChatQuestionList.CQL[i].Num = i + 1;
                ChatQuestionList.CQL[i].chatQuestion = (CSVdata[4 * (i + 1) + 1]);
                ChatQuestionList.CQL[i].answerA = (CSVdata[4 * (i + 1) + 2]);
                ChatQuestionList.CQL[i].answerB = (CSVdata[4 * (i + 1) + 3]);
            }


        }

        public void RandomQuestion()
        {

            int cqlnum = UnityEngine.Random.Range(1, 107);

            Debug.Log(cqlnum);
            //Debug.Log(ChatQuestionList.CQL[cqlnum].chatQuestion);
            Debug.Log(ChatQuestionList.CQL[cqlnum].answerA);
            Debug.Log(ChatQuestionList.CQL[cqlnum].answerB);

            question.text = "<color=#F55637>" + "Q.  " + "</Color>" + ChatQuestionList.CQL[cqlnum].chatQuestion;
            answerA.text = ChatQuestionList.CQL[cqlnum].answerA;
            answerB.text = ChatQuestionList.CQL[cqlnum].answerB;
        }
        public void ReceiveMessage(string text)
        {
            if (MineToggle.isOn) Chat(true, text, "나", "", null);
            else Chat(false, text, "타인", "", null);
        }


        public void LayoutVisible(bool b)
        {
            AndroidJavaClass kotlin = new AndroidJavaClass("com.unity3d.player.SubActivity");
            kotlin.CallStatic("LayoutVisible", b);
        }
        public void RQuestion()
        {
            Transform RArea = Instantiate(RQArea).transform;
            RArea.SetParent(ContentRect.transform, false);
            RArea.GetComponent<AreaScript>().TimeText.text = question.text;
            RArea.GetComponent<AreaScript>().UserText.text = answerA.text;
            RArea.GetComponent<AreaScript>().DateText.text = answerB.text;
            Invoke("ScrollDelay", 0.03f);
            //Invoke("resetQ",0.03f);
        }

        public void SA()
        {
            Transform RArea = Instantiate(FQ).transform;
            RArea.SetParent(ContentRect.transform, false);
        }
        void resetQ()
        {
            question.text = "";
            answerA.text = "";
            answerB.text = "";
        }

        void Start()
        {

            ReadCSV();
            scrollBar.value = 0.00001f;
            //currentTime = groupchatSceneManager.sec;
            RandomQuestion();
            RQuestion();
            SA();
            //Invoke("SA", 0.3f); 
        }
        void Update()
        {

            currentTime -= 1 * Time.deltaTime;
            if (null != GameObject.Find("Text_Timer_GroupChat"))
            {
                countdownText.text = currentTime.ToString("0");
            }


            //10분 5분 3분 타이머 AI

            if (currentTime <= 600 && 599 <= currentTime && !ai && null != GameObject.Find("Text_Timer_GroupChat"))//텍스트 오브젝트를 찾을 수 있는 경우만)
            {
                Transform CurAIArea = Instantiate(AIArea).transform;
                CurAIArea.SetParent(ContentRect.transform, false);
                CurAIArea.GetComponent<AreaScript>().DateText.text = ("단체 채팅 종료까지 \n" + "<b>" + Mathf.Ceil(currentTime / 60) + "</b>" + "<b>분</b> 남았습니다.");
                ai = true;
            }
            if (currentTime <= 300 && 299 <= currentTime && ai)
            {
                Transform CurAIArea = Instantiate(AIArea).transform;
                CurAIArea.SetParent(ContentRect.transform, false);
                CurAIArea.GetComponent<AreaScript>().DateText.text = ("단체 채팅 종료까지 \n" + "<b>" + Mathf.Ceil(currentTime / 60) + "</b>" + "<b>분</b> 남았습니다.");
                ai = false;
            }
            if (currentTime <= 180 && 179 <= currentTime && !ai)
            {
                Transform CurAIArea = Instantiate(AIArea).transform;
                CurAIArea.SetParent(ContentRect.transform, false);
                CurAIArea.GetComponent<AreaScript>().DateText.text = ("단체 채팅 종료까지 \n" + "<b>" + Mathf.Ceil(currentTime / 60) + "</b>" + "<b>분</b> 남았습니다.");
                ai = true;
            }



            if (currentTime <= 0)
            {
                currentTime = 0;
            }

            DisplayTime(currentTime);
        }
        void DisplayTime(float TimeToDisplay)
        {
            if (TimeToDisplay < 0)
            {
                TimeToDisplay = 0;
            }
            float mins = Mathf.FloorToInt(TimeToDisplay / 60);
            float secs = Mathf.FloorToInt(TimeToDisplay % 60);

            if (null != GameObject.Find("Text_Timer_GroupChat"))//해당이름의 게임오브젝트가 씬에 있으면으로 그룹챗 씬인지 판별
            {
                countdownText.text = string.Format("{0:00}{1:00}", mins, "분");//그룹챗에서만 작동하도록 예외처리
            }


        }

        //말줄임코드
        public void StringTransfer(String inputString, int stringLength)
        {
            string msg = inputString;
            int textlength = inputString.Length;
            if (textlength > stringLength)
            {
                string newString = inputText.text.Remove(stringLength, inputText.text.Length - stringLength);
                inputText.text = newString + "...";

            }
        }





        public void Chat(bool isSend, string text, string user,string time, Texture2D picture)
        {
            if (text.Trim() == "") return;

            bool isBottom = scrollBar.value <= 0.00001f;


            //보내는 사람은 노랑, 받는 사람은 흰색영역을 크게 만들고 텍스트 대입

            AreaScript Area = Instantiate(isSend ? MyArea : ElseArea).GetComponent<AreaScript>();
            AreaScript Area2 = Instantiate(isSend ? MyArea : GetDown).GetComponent<AreaScript>();


            Area.transform.SetParent(ContentRect.transform, false);
            Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);

            Area.TextRect.GetComponent<Text>().text = text;

            //최근 메시지 보기 버튼에 텍스트 불러오기
            Area2.TextRect.GetComponent<Text>().text = text;
            gd.GetComponent<Text>().text = text;

            Fit(Area.BoxRect);
            //최근 메시지 말줄임
            StringTransfer(inputText.text, stringLength);

            // 두 줄 이상이면 크기를 줄여가면서, 한 줄이 아래로 내려가면 바로 전 크기를 대입 
            float X = Area.TextRect.sizeDelta.x + 150;
            float Y = Area.TextRect.sizeDelta.y;
            if (Y > 49)
            {
                for (int i = 0; i < 200; i++)
                {
                    Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                    Fit(Area.BoxRect);

                    if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                }
            }
            else Area.BoxRect.sizeDelta = new Vector2(X, Y);

            Area.User = user;

            DateTime t = DateTime.Now;
            // 현재 것에 분까지 나오는 날짜와 유저이름 대입
            if (time == "")
            {                
                Area.Time = t.ToString("yyyy-MM-dd-HH-mm");            

                // 현재 것은 항상 새로운 시간 대입
                int hour = t.Hour;
                if (t.Hour == 0) hour = 12;
                else if (t.Hour > 12) hour -= 12;
                Area.TimeText.text = (t.Hour > 12 ? "오후 " : "오전 ") + hour + ":" + t.Minute.ToString("D2");
            }
            else
            {
                Area.TimeText.text = time;
            }

            // 이전 것과 같으면 이전 시간, 꼬리 없애기
            bool isSame = LastArea != null && LastArea.Time == Area.Time && LastArea.User == Area.User;
            if (isSame) LastArea.TimeText.text = "";
            //Area.Tail.SetActive(!isSame);


            // 타인이 이전 것과 같으면 이미지, 이름 없애기
            if (!isSend)
            {
                Area.UserImage.gameObject.SetActive(!isSame);
                Area.UserText.gameObject.SetActive(!isSame);
                Area.UserText.text = Area.User;
                if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
            }


            // 이전 것과 날짜가 다르면 날짜영역 보이기
            //if (time == "")
            //{

            //    if (LastArea != null && LastArea.Time.Substring(0, 10) != Area.Time.Substring(0, 10))
            //    {
            //        Transform CurDateArea = Instantiate(DateArea).transform;
            //        CurDateArea.SetParent(ContentRect.transform, false);
            //        CurDateArea.SetSiblingIndex(CurDateArea.GetSiblingIndex() - 1);

            //        string week = "";
            //        switch (t.DayOfWeek)
            //        {
            //            case DayOfWeek.Sunday: week = "일"; break;
            //            case DayOfWeek.Monday: week = "월"; break;
            //            case DayOfWeek.Tuesday: week = "화"; break;
            //            case DayOfWeek.Wednesday: week = "수"; break;
            //            case DayOfWeek.Thursday: week = "목"; break;
            //            case DayOfWeek.Friday: week = "금"; break;
            //            case DayOfWeek.Saturday: week = "토"; break;
            //        }
            //        CurDateArea.GetComponent<AreaScript>().DateText.text = t.Year + "년 " + t.Month + "월 " + t.Day + "일 " + week + "요일";
            //    }
            //}



            Fit(Area.BoxRect);
            Fit(Area.AreaRect);
            Fit(ContentRect);
            LastArea = Area;


            // 스크롤바가 위로 올라간 상태에서 메시지를 받으면 맨 아래로 내리지 않음
            if (!isSend && !isBottom)
            {
                if (scrollBar.size < 1)
                {
                    getDownbtn.gameObject.SetActive(true);
                }
                return;
            }
            Invoke("ScrollDelay", 0.03f);
        }


        public void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);


        void ScrollDelay() => scrollBar.value = 0;
    }
}
    
