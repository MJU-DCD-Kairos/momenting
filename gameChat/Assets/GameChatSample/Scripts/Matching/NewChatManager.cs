using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameChatUnity;
using GameChatUnity.SimpleJSON;
using GameChatUnity.SocketIO;
//using GameChatUnity.Extention;

using UnityEngine.Networking;


public class NewChatManager : MonoBehaviour
{

    //필요 변수 설정
    [Header("매칭")]
    //매칭 쿨타임
    public bool canMatching = true; 

    //RTDB에 들어갈 유저 정보를 클래스화
    public class UserInfo
    {
        public string uid = "";
        public string sex = "";
        public bool isMatching = false; //매칭중인지 체크
        public List<string> RuidList = new List<string>();
    }


    [Header("채팅방 이름 랜덤 생성")]
    public TextAsset CRnameCSVfile;
    public string newCRname = "";
    public int tableSize = 104;
    string randomCRN;
    //각 클래스를 기반으로 배열 변수 생성
    public CRNList ChatRoomNameList = new CRNList();

    //각 값을 보유할 클래스 생성
    //[System.Serializable]
    [SerializeField]
    public class chatRoomName
    {
        public string Adjective;
        public string Noun;
    }

    [SerializeField]
    public class CRNList
    {
        public chatRoomName[] CRN;
    }

    public string currentCRname = "" ;
    public Button sendMSGBtn;
    public List<Channel> CList = new List<Channel>();



    // Start is called before the first frame update
    void Start()
    {
        //don't destroy 처리
        DontDestroyOnLoad(this.gameObject);
        

        //채팅방 이름 랜덤 생성을 위한 텍스트 파싱 함수 호출
        ReadCSV();
        getChannelID();
        Debug.Log("최근 채팅방ID : "+ CList[0].id+"/ 이름: "+CList[0].name);
        //GameChat.sendMessage("241ad48e-cb5c-4a55-8756-b595f30324bd", "고정 테스트 메시지 전송");
        //GameChat.sendMessage(currentCRname, "CRN 테스트 메시지 전송");
        sendMSG();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //채팅방 이름 랜덤생성 및 채팅방 생성 함수
    //채팅방 이름 중복체크 기능 추가 필요
    #region
    //랜덤 채팅방 이름 생성을 위한 csv파일 파싱 함수
    void ReadCSV()
    {
        //참조한 CSV파일을 ,와 엔터단위로 파싱
        string[] CSVdata = CRnameCSVfile.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);

        ChatRoomNameList.CRN = new chatRoomName[tableSize];

        for (int i = 0; i < tableSize - 1; i++)
        {
            ChatRoomNameList.CRN[i] = new chatRoomName();
            ChatRoomNameList.CRN[i].Adjective = (CSVdata[2 * (i + 1)]);
            ChatRoomNameList.CRN[i].Noun = (CSVdata[2 * (i + 1) + 1]);
        }
    }

    //임의의 형용사 + 명사 7자 이하의 채팅방 이름 생성 함수
    //스트링 타입의 (adj + " " + noun)를 반환 "형용사 한칸띄고 명사"
    public string makeChatRoomName()
    {
        while (true)
        {
            int AdjNum = UnityEngine.Random.Range(1, tableSize);
            int NounNum = UnityEngine.Random.Range(1, tableSize);

            string adj = ChatRoomNameList.CRN[AdjNum].Adjective;
            string noun = ChatRoomNameList.CRN[NounNum].Noun;

            int adjNum = adj.Length;
            int nounNum = noun.Length;
            Debug.Log("위에디버그" + (adjNum + nounNum + 1));
            //Debug.Log(adjNum);
            //Debug.Log(nounNum);

            if (adjNum + nounNum + 1 < 8)
            {
                Debug.Log(adj + " " + noun);
                return (adj + " " + noun);
            }
        }
    }


    //채팅방 이름을 생성하여 새로운 채널을 생성하는 함수

    public IEnumerator CreateChatRoom()
    {
        string url = "https://dashboard-api.gamechat.naverncp.com/v1/api/project/e3558324-2d64-47d0-bd7a-6fa362824bd7/channel";
        string APIKey = "ec31cc21b559da9eb19eaec2dadcd50ed786857a740a561d";



        WWWForm form = new WWWForm();
        string name = makeChatRoomName();
        //string projectID = "e3558324 - 2d64 - 47d0 - bd7a - 6fa362824bd7";
        //form.AddField("projectId", projectID);
        form.AddField("name", name);


        //웹 url 요청함 POST 요청
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        www.SetRequestHeader("x-api-key", APIKey);
        //www.SetRequestHeader("content-type", "application/json");

        //요청에 대한 응답을 기다림
        yield return www.SendWebRequest();

        Debug.Log("현재 생성된 채팅방 아이디는 : " + www.result);
        //result값이 success라고 뜨는 상황

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("성공");
            
            //DB의 유저 데이터에 해당 채팅방 코드로 접속하는 로직 추가 필요
            Debug.Log(www.downloadHandler.text);

        }
    }

    #endregion

    //채팅방 정보 가져오는 함수
    #region
  
    public void getChannelID()
    {
        
        GameChat.getChannels(0, 1, (List<Channel> Channels, GameChatException Exception) =>
        {
            if (Exception != null)
            {
                Debug.Log(Exception.message);
                Debug.Log(Exception.code);
                //에러 핸들링
                Debug.Log("get channel 에러");
                return;
            }
            
            foreach (Channel elem in Channels)
            {
                Debug.Log("get channel 성공");
                CList.Add(elem);
                for(int i=0; i<CList.Count; i++)
                {
                    Debug.Log(i);
                    Debug.Log("겟채널 가져온 리스트의 id : " + CList[i].id);
                    currentCRname = CList[i].id.ToString();
                    Debug.Log("겟채널 가져온 리스트의 rawJson : " + CList[i].rawJson);

                }  
            }
        });
        //Debug.Log(" CList[0].id: " + CList[0].id);
    }

    public void sendMSG()
    {
        
        GameChat.sendMessage(CList[0].id, "테스트 메시지 전송");
    }
    

    #endregion
}
