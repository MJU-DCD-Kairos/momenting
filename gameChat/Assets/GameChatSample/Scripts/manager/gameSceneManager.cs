using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using GameChatUnity;
using GameChatUnity.SimpleJSON;
using GameChatUnity.SocketIO;

public class gameSceneManager : MonoBehaviour
{
    //스플래시 타이머 코드를 위한 변수 선언
    private float time_current = 3f;
    private float time_Max = 3f;
    private bool isEnded;
    public GameObject title;
    public GameObject splash;

    public string creatChatCode;


    //채팅리스트에서 채널 메시지 로드로 넘어갈 때 사용하는 채팅방 이름 및 채널 ID
    public static string chatRname;
    public static string chatRID;



    //GameObject TodayQ;



    //채팅방 이름 생성함수를 위한 선언
    //csv파일을 외부에서 인스펙터에서 직접 참조할 수 있도록 생성_채팅방 이름 생성
    public TextAsset csvfile;

    //오늘의 질문을 넣어줄 UI텍스트 오브젝트를 인스펙터로 참조받기위한 선언_채팅방 이름 텍스트
    public Text ChatRoomNameText;

    ////CSV파일의 행 개수를 인스펙터상에서 입력하기 위한 퍼블릭 변수 선언_채팅방 이름을 위한 작업
    //public int tableSize;
    //string randomCRN;

    ////각 값을 보유할 클래스 생성
    //[System.Serializable]
    //public class chatRoomName
    //{
    //    public string Adjective;
    //    public string Noun;
    //}

    ////리스트를 보유할 클래스 생성
    //[System.Serializable]
    //public class CRNList
    //{
    //    public chatRoomName[] CRN;
    //}

    ////각 클래스를 기반으로 배열 변수 생성
    //public CRNList ChatRoomNameList = new CRNList();





    // Start is called before the first frame update
    void Start()
    {
        //씬매니저 파괴 방지를 위한 코드
        DontDestroyOnLoad(this.gameObject);
        //TodayQ = GameObject.Find("TodayQ").GetComponent<>();
        //GameChat.initialize("e3558324-2d64-47d0-bd7a-6fa362824bd7");
        //PlayerPrefs.SetString("LastMSGID", "62860f665d257d31a779d5cf");
        


    }

    // Update is called once per frame
    void Update()
    {
        //스플래시 타이머 작동 코드
        if (null != GameObject.Find("isTitle"))
        {
            Check_Timer();
        }
        

    }

    //씬 네임으로 원하는 씬을 로드
    static void LoadScene(string name)
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
        //PlayerPrefs.SetString("GCName", "hj");
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

    public static void LoadScene_GroupChat()
    {
        LoadScene("SampleScene_Main");
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
            //Debug.Log(time_current);
        }
        else if (!isEnded)
        {
            End_Timer();
        }


    }

    private void End_Timer()
    {
        Debug.Log("Splah Timer End");
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



    //채팅방 이름 랜덤생성을 위한 csv파싱코드
    //public void ReadCSV()
    //{
    //    //참조한 CSV파일을 ,와 엔터단위로 파싱
    //    string[] CSVdata = csvfile.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);

    //    ChatRoomNameList.CRN = new chatRoomName[tableSize];

    //    for (int i = 0; i < tableSize - 1; i++)
    //    {
    //        ChatRoomNameList.CRN[i] = new chatRoomName();
    //        ChatRoomNameList.CRN[i].Adjective = (CSVdata[2 * (i + 1)]);
    //        ChatRoomNameList.CRN[i].Noun = (CSVdata[2 * (i + 1) + 1]);
    //    }

    //}
   
    //public string makeChatRoomName()
    //{
    //    while (true)
    //    {
    //        int AdjNum = UnityEngine.Random.Range(1, tableSize);
    //        int NounNum = UnityEngine.Random.Range(1, tableSize);

    //        string adj = ChatRoomNameList.CRN[AdjNum].Adjective;
    //        string noun = ChatRoomNameList.CRN[NounNum].Noun;

    //        int adjNum = adj.Length;
    //        int nounNum = noun.Length;
    //        Debug.Log("위에디버그" + (adjNum + nounNum + 1));
    //        //Debug.Log(adjNum);
    //        //Debug.Log(nounNum);

    //        if (adjNum + nounNum + 1 < 8)
    //        {
    //            Debug.Log(adj + " " + noun);
    //            return (adj + " " + noun);
    //        }
    //    }

    //}



    //public string CreatChatCode()
    //{
    //    GameChat.getChannels(0, 1, (List<Channel> Channels, GameChatException exception) =>
    //    {
    //        if (exception != null)
    //        {
    //            Debug.Log("getChannels Exception Log => " + exception.ToJson());
    //            return;
    //        }
    //        foreach (Channel elem in Channels)
    //        {
    //            if (null != elem)
    //            {
    //                List<string> chatList = new List<string>();
    //                chatList.Add(elem.ToString());
    //                creatChatCode = chatList[0];
    //                Debug.Log("creatChatCode" + creatChatCode);

    //            }
    //            else
    //            {
    //                Debug.Log("elem없음");
    //            }
                
    //        }   
    //    });
    //    return (creatChatCode);
    //}

}
