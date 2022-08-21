using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameChatSample;
using FireStoreScript;

public class homeSceneManager : MonoBehaviour
{
    //스크립트 받아오기위한 타입 변수 선언
    //NewChatManager NCM;
    public TextAsset csvFile;

    //매칭화면 활성화 여부 체크를 위한 오브젝트 참조
    public GameObject MatchingPage;

    //가입승인 여부
    public GameObject ispasscan;
    public GameObject HomeCan;

    gameSceneManager gSM;
    FirebaseManager FbM;
    
    //오늘의 질문 답변 여부 체크
    

    //onclick 이용할 버튼 참조
    public Button ProfileBtn;
    public Button MailBox;
    public Button ChatList;


    // Start is called before the first frame update
    void Start()
    {
        //don't destroy로 살려서 넘어온 게임씬매니저의 스크립트를 변수에 담음
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();
        FbM = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();
        //NCM = GameObject.Find("GameSceneManager").GetComponent<NewChatManager>();
        //NewChatManager.getChannelID();

        //버튼에 gSM의 로드씬 함수 리스너를 추가함
        ProfileBtn.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
        MailBox.onClick.AddListener(gSM.LoadScene_MailBox);
        ChatList.onClick.AddListener(gSM.LoadScene_ChatList);
        //FirebaseManager.LoadData();
        if(FirebaseManager.ispass == "false")
        {
            ispasscan.SetActive(true);
            HomeCan.SetActive(false);
        }
        else
        {
            ispasscan.SetActive(false);
            HomeCan.SetActive(true);
        }
      

    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)  // 플렛폼 정보 .
        {
            if (Input.GetKey(KeyCode.Escape)) // 키 눌린 코드 신호를 받아오는것.
            {
                //SceneManager.LoadScene("뒤로갈 씬 이름 "); // 씬으로 이동 .
                Application.Quit(); // 씬 종료 .(나가기)            위씬으로 이동이나 종료기능 둘중하나 원하시는것을 사용하시면 됩니다.
            }
        }

    }

    
    //오늘의 질문 답변 시 다이얼로그 안뜸상태
    //public void isTodayQdoneTrue()
    //{
    //    PlayerPrefs.SetInt("todayQdone",1);
    //    PlayerPrefs.SetInt("todayQanswer", 1);
    //}

    ////오늘의 질문 닫은 경우 인지
    //public void isTodayQdoneFalse()
    //{
    //    if(1!= PlayerPrefs.GetInt("todayQanswer"))
    //    {
    //        PlayerPrefs.SetInt("todayQdone", 0);
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt("todayQdone", 1);
    //    }
        
    //}

}
