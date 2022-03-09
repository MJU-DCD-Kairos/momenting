using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text;



public class ChatManager : MonoBehaviour
{
    
    public GameObject MyArea, ElseArea, DateArea, GetDown, AIArea;
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
    

    public void ReceiveMessage(string text)
    {
        if (MineToggle.isOn) Chat(true, text, "나", null);
        else Chat(false, text, "타인", null);
    }


    public void LayoutVisible(bool b)
    {
        AndroidJavaClass kotlin = new AndroidJavaClass("com.unity3d.player.SubActivity");
        kotlin.CallStatic("LayoutVisible", b);
    }

 
    void Start()
    {
        scrollBar.value = 0.00001f;
        currentTime = startingTime;
    }
    void Update()
    {

        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");

        //10분 5분 3분 타이머 AI
        
        if (currentTime <= 600 && 599 <= currentTime && !ai)
        {
            Transform CurAIArea = Instantiate(AIArea).transform;
            CurAIArea.SetParent(ContentRect.transform, false);
            CurAIArea.GetComponent<AreaScript>().DateText.text = ("단체 채팅 종료까지 \n" + Mathf.Ceil(currentTime / 60) + "분 남았습니다.");
            ai = true;
                }
        if (currentTime <= 300 && 299 <= currentTime && ai)
        {
            Transform CurAIArea = Instantiate(AIArea).transform;
            CurAIArea.SetParent(ContentRect.transform, false);
            CurAIArea.GetComponent<AreaScript>().DateText.text = ("단체 채팅 종료까지 \n" + Mathf.Ceil(currentTime / 60) + "분 남았습니다.");
            ai = false;
        }
        if (currentTime <= 180 && 179 <= currentTime && !ai)
        {
            Transform CurAIArea = Instantiate(AIArea).transform;
            CurAIArea.SetParent(ContentRect.transform, false);
            CurAIArea.GetComponent<AreaScript>().DateText.text = ("단체 채팅 종료까지 \n" + Mathf.Ceil(currentTime / 60) + "분 남았습니다.");
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

        countdownText.text = string.Format("{0:00}{1:00}", mins, "분");

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





    public void Chat(bool isSend, string text, string user, Texture2D picture) 
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


        // 현재 것에 분까지 나오는 날짜와 유저이름 대입
        DateTime t = DateTime.Now;
        Area.Time = t.ToString("yyyy-MM-dd-HH-mm");
        Area.User = user;


        // 현재 것은 항상 새로운 시간 대입
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        Area.TimeText.text = (t.Hour > 12 ? "오후 " : "오전 ") + hour + ":" + t.Minute.ToString("D2");


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
            if(picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
        }


        // 이전 것과 날짜가 다르면 날짜영역 보이기
        if (LastArea != null && LastArea.Time.Substring(0, 10) != Area.Time.Substring(0, 10))
        {
            Transform CurDateArea = Instantiate(DateArea).transform;
            CurDateArea.SetParent(ContentRect.transform, false);
            CurDateArea.SetSiblingIndex(CurDateArea.GetSiblingIndex() - 1);

            string week = "";
            switch (t.DayOfWeek)
            {
                case DayOfWeek.Sunday: week = "일"; break;
                case DayOfWeek.Monday: week = "월"; break;
                case DayOfWeek.Tuesday: week = "화"; break;
                case DayOfWeek.Wednesday: week = "수"; break;
                case DayOfWeek.Thursday: week = "목"; break;
                case DayOfWeek.Friday: week = "금"; break;
                case DayOfWeek.Saturday: week = "토"; break;
            }
            CurDateArea.GetComponent<AreaScript>().DateText.text = t.Year + "년 " + t.Month + "월 " + t.Day + "일 " + week + "요일";
        }

       


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


    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);


    void ScrollDelay() => scrollBar.value = 0;
}
