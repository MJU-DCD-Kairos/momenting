using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameChatSample;

public class chatlistSceneManager : MonoBehaviour
{
    //스크립트 받아오기위한 타입 변수 선언
    gameSceneManager gSM;
    public Button backToHome;

    //그룹채팅 카드에 들어갈 게임오브젝트 참조
    public GameObject CListUI;

    //그룹채팅에 들어갈 정보 받아올 변수 선언
    public string GetID = "";//받아온channelID
    public string RoomTitleText = "샘플타이틀";
    public string RoomTimerText = "00:00";
    public string RoomContentsText = "콘텐츠내용을 입력";
    public int RoomNewMSGCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        //don't destroy로 살려서 넘어온 게임씬매니저의 스크립트를 변수에 담음
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

        makeRList();
        //버튼에 gSM의 로드씬 함수 리스너를 추가함
        backToHome.onClick.AddListener(gSM.LoadScene_Home);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)  // 플렛폼 정보 .
        {
            if (Input.GetKey(KeyCode.Escape)) // 키 눌린 코드 신호를 받아오는것.
            {
                SceneManager.LoadScene("Home"); // 씬으로 이동 .
                //Application.Quit(); // 씬 종료 .(나가기)            위씬으로 이동이나 종료기능 둘중하나 원하시는것을 사용하시면 됩니다.
            }
        }
    }
    public void makeRList()
    {
        GameObject ui = Instantiate(CListUI, GameObject.Find("GCViewport").GetComponent<RectTransform>());
        ui.transform.SetParent(GameObject.Find("GCViewport").transform);

        ui.transform.GetChild(2).GetComponent<Text>().text = RoomTitleText; //채팅방 이름
        ui.transform.GetChild(4).transform.GetChild(1).GetComponent<Text>().text = RoomTimerText; //남은시간
        ui.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = RoomNewMSGCount.ToString(); //배지 수
        ui.transform.GetChild(3).GetComponent<Text>().text = RoomContentsText; //라스트 채팅내용
    }

    public void GetCurrentChatCount(string id)
    {
        GameChatSample.NewChatManager.getChannelID();//firebase에서 id를 가져와야함. 변수에 id를 string으로 담은 다음

        RoomNewMSGCount = GameChatSample.NewChatManager.getCurrentMSG(id);
        getLostTime();

        
    }

    public string getLostTime()
    {
        string cTime = GameChatSample.NewChatManager.CList[0].created_at.Substring(14, 5);//생성시간
        string nTime = DateTime.Now.ToString("u").Substring(12, 5);//현재시간
        TimeSpan goTime = DateTime.Parse(cTime) - DateTime.Parse(nTime);
        TimeSpan lostTime = DateTime.Parse("20:00") - DateTime.Parse(goTime.ToString());
        return lostTime.ToString();
    }

}
