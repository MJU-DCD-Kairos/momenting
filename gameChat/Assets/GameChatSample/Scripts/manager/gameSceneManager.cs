using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameSceneManager : MonoBehaviour
{
    //스플래시 타이머 코드를 위한 변수 선언
    private float time_current = 3f;
    private float time_Max = 3f;
    private bool isEnded;
    public GameObject title;
    public GameObject splash;
    
    //GameObject TodayQ;


    // Start is called before the first frame update
    void Start()
    {
        //씬매니저 파괴 방지를 위한 코드
        DontDestroyOnLoad(this.gameObject);
        //TodayQ = GameObject.Find("TodayQ").GetComponent<>();

    }

    // Update is called once per frame
    void Update()
    {
        //스플래시 타이머 작동 코드
        Check_Timer();
    }

    //씬 네임으로 원하는 씬을 로드
    void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    //타이틀 씬 호출
    public void LoadScene_Title()
    {
        LoadScene("Title");
    }

    //홈 씬 호출
    public void LoadScene_Home()
    {
        LoadScene("Home");
    }

    //챗리스트 씬 호출
    public void LoadScene_ChatList()
    {
        LoadScene("ChatList");
    }

    //메일박스 씬 호출
    public void LoadScene_MailBox()
    {
        LoadScene("MailBox");
    }

    //마이프로필 씬 호출
    public void LoadScene_MyProfile_Sample3()
    {
        LoadScene("MyProfile_Sample3");
        Debug.Log("프로필씬 호출버튼 클릭");
    }
   
    //일대일대화 씬 호출
    public void LoadScene_PersonalChat()
    {
        LoadScene("PersonalChat");
    }

    //모래알테스트 씬 호출
    public void LoadScene_TypeTest()
    {
        LoadScene("TypeTest");
    }

    //단체채팅방 씬 호출
    public void LoadScene_SampleScene_Login()
    {
        LoadScene("SampleScene_Login");
    }
    
    //회원가입 씬 호출
    public void LoadScene_SignUp()
    {
        LoadScene("SignUp");
    }

    //로그인 씬 호출
    public void LoadScene_SignIn()
    {
        LoadScene("SignIn");
    }

    //세팅 씬 호출
    public void LoadScene_Setting()
    {
        LoadScene("Setting");
    }


    //스플래시 화면 동작을 위한 타이머 코드
    
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
        title.SetActive(true);
        splash.SetActive(false);
        isEnded = true;
    }


    private void Reset_Timer()
    {
        time_current = time_Max;
        isEnded = false;
        Debug.Log("Start");
    }
}
