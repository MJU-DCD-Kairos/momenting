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

    public string id = "";

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //don't destroy로 살려서 넘어온 게임씬매니저의 스크립트를 변수에 담음
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

        //Invoke("makeGCList", 0.03f);
        //버튼에 gSM의 로드씬 함수 리스너를 추가함
        backToHome.onClick.AddListener(gSM.LoadScene_Home);
        
        makeGCList();
        
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

        Debug.Log("### room_name ### : " + GameChatSample.NewChatManager.CurChatInfo[0]);
        ui.transform.GetChild(2).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[0]; //채팅방 이름
        ui.transform.GetChild(4).transform.GetChild(1).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[2]; //남은시간
        ui.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[4];//배지 수
        /*if (GameChatSample.NewChatManager.CurChatInfo[4] == "0") 
        {
            ui.transform.GetChild(1).transform.gameObject.SetActive(false);
        }
        else
        {
            ui.transform.GetChild(1).transform.gameObject.SetActive(true);
        }*/
        ui.transform.GetChild(3).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[1]; //라스트 채팅내용

        //CurChatInfo[0]이름 [1]내용 [2]생성시간 [3]생성날짜 [4]새로운메시지수
    }

    public void GetCurrentChatCount(string id)
    {
        Debug.Log("GetCurrentChatCount시작 id는? : " + id);
        //GameChatSample.NewChatManager.getChannelID();//firebase에서 id를 가져와야함. 변수에 id를 string으로 담은 다음
        RoomNewMSGCount = GameChatSample.NewChatManager.setNewMSGCount(id);
        
    }
    /*
    public void getLostTime()
    {
        //string cTime = GameChatSample.NewChatManager.CList[0].created_at.Substring(11, 8);//생성시간
        string cTime= GameChatSample.NewChatManager.CurChatInfo[2];
        //string cDay = GameChatSample.NewChatManager.CList[0].created_at.Substring(0, 10);
        string cDay = GameChatSample.NewChatManager.CurChatInfo[3];
        string nTime = DateTime.Now.ToString("u").Substring(11, 8);//현재시간
        string nDay = DateTime.Now.ToString("u").Substring(0, 10);
        Debug.Log(cTime + "/" + cDay + "/" + nTime + "/" + nDay);
        TimeSpan goTime = Convert.ToDateTime(nTime) - Convert.ToDateTime(cTime);

        if (goTime.Days <= 0) 
        {
            if (goTime.Hours == 0)
            {
                if (goTime.Minutes > 20)
                {
                    RoomTimerText = "종료";
                }
                else
                {
                    RoomTimerText = goTime.Minutes.ToString() + "분";
                }
            }
            RoomTimerText = "종료";


        }

    }*/

    public void makeGCList()
    {
        id = GameChatSample.NewChatManager.CList[0].id.ToString().Replace(" ","");
        RoomContentsText = GameChatSample.NewChatManager.getCurMSG(id);
        RoomTitleText = GameChatSample.NewChatManager.getCurRoomName(id);
        GetCurrentChatCount(id);//배지 수 구하기
        

        Invoke("makeRList", 1f);

     
    }

}
