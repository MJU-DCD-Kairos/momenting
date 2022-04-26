using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class homeSceneManager : MonoBehaviour
{
    //스크립트 받아오기위한 타입 변수 선언
    gameSceneManager gSM;
    
    //매칭 화면에서 채팅으로 넘어가는 플로우 테스트용 타이머, 불변수 선언
    bool isMatching = true;
    // 타이머 코드를 위한 변수 선언
    private float time_current = 5f;
    private float time_Max = 5f;
    private bool isEnded;
    //매칭화면 활성화 여부 체크를 위한 오브젝트 참조
    public GameObject MatchingPage;


    //onclick 이용할 버튼 참조
    public Button ProfileBtn;
    public Button MailBox;
    public Button ChatList;

    // Start is called before the first frame update
    void Start()
    {
        //don't destroy로 살려서 넘어온 게임씬매니저의 스크립트를 변수에 담음
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();
        
        //버튼에 gSM의 로드씬 함수 리스너를 추가함
        ProfileBtn.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
        MailBox.onClick.AddListener(gSM.LoadScene_MailBox);
        ChatList.onClick.AddListener(gSM.LoadScene_ChatList);
    }

    // Update is called once per frame
    void Update()
    {
        if (MatchingPage.active)
        {
            Check_Timer();
        }
    }

    //매칭 완료 시 채팅 플로우를 시작하는 씬을 로드하는 함수
    void matchingScene()
    {
        if(isMatching == true)
        {
            gSM.LoadScene_SampleScene_Login();
        }
        else
        {
            Reset_Timer();
            Check_Timer();
        }
        
    }

    // 타이머 코드
    private void Check_Timer()
    {

        if (0 < time_current)
        {
            time_current -= Time.deltaTime;
            Debug.Log(time_current);
        }
        else if (!isEnded)
        {
            End_Timer();
        }


    }

    private void End_Timer()
    {
        Debug.Log("End");
        time_current = 0;
        matchingScene();
    }


    private void Reset_Timer()
    {
        time_current = time_Max;
        isEnded = false;
        Debug.Log("Start");
    }

}
