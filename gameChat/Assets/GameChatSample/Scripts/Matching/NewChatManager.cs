using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameChatUnity;
using GameChatUnity.SimpleJSON;
using GameChatUnity.SocketIO;
using System.Threading;
using System.Threading.Tasks;

using CM;
using AS;
using FireStoreScript;
using Firebase.Firestore;
using Firebase.Extensions;

using UnityEngine.Networking;

namespace GameChatSample
{
    public class NewChatManager : MonoBehaviour
    {

        //필요 변수 설정
        [Header("매칭")]
        //매칭 쿨타임
        public bool canMatching = true;




        [Header("채팅방 이름 랜덤 생성")]
        public TextAsset CRnameCSVfile;
        public static string newCRname = "";
        public static int tableSize = 104;
        string randomCRN;
        //각 클래스를 기반으로 배열 변수 생성
        public static CRNList ChatRoomNameList = new CRNList();

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
        //테스트용 버튼
        public Button CreatChatBtn;

        //채팅리스트 받아와서 id를 가져오기 위한 리스트, 이름 초기화
        public static string currentCRname = "";
        public static List<Channel> CList = new List<Channel>();


        //채팅방 이름이 보여질 UI 텍스트 참조
        public Text ThisCRoomNameTitle;

        //chatmanager를 참조받아오기 위한 선언
        public ChatManager chatManager;


        //내 말풍선 상대 말풍선 가리기 참조 위한 선언
        AreaScript LastArea;
        public GameObject MyArea, ElseArea;
        public RectTransform ContentRect;
        //불러오려는 메시지가 마지막 메시지랑 같은지 판별하기 위한 전역변수 선언
        public Message LastMSG;
        //이전메시지의 시간을 받아오기 위한 변수 선언
        public Message xMSG;

        public static string CCName;
        public static string[] CurChatInfo = new string[5];



        //매칭을 위한 선언부
        public string username;
        public int usersex;
        public string docID; //도큐먼트 고유ID 참조하기 위해 필요
        public int countMembers;
        public static bool isMatchComplete = false;
        public ListenerRegistration listener;
        public ListenerRegistration listener2;

        public static Dictionary<string, object> newChatRoom = new Dictionary<string, object>();

        //매칭을 위한 상수 선언부
        public static string GAMECHAT_ROOM = "gameChatRoom";
        public static string ISOPEN = "isOpen";
        public static string ISACTIVE = "isActive";
        public static string MEMBER = "member";
        public static string NICKNAME = "nickName";
        public static string SEX = "sex";
        public static string CHANNELID = "channelID";
        public static string CREATETIME = "createTime";
        public static string OPENTIME = "openTime";






        // Start is called before the first frame update
        void Start()
        {
            //don't destroy 처리
            DontDestroyOnLoad(this.gameObject);

            //채팅방 이름 랜덤 생성을 위한 텍스트 파싱 함수 호출
            ReadCSV();

            //도큐먼트 id초기화를 위한 과정// 필요함
            docID = "";
            
            //매칭 과정을 위한 테스트설정
            username = "솔비";//유저 이름 임의설정
            usersex = 2;//유저 성별 임의 설정
            Debug.Log("현재 로그인 유저 닉네임 : " + username);
            Debug.Log("현재 로그인 유저 성별 : " + usersex);
        }

        // Update is called once per frame
        void Update()
        {

        }


        //채팅방 이름 랜덤생성 및 채팅방 생성 함수
        //채팅방 이름 중복체크 기능 추가 필요

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

        #region Matching
        public async void matchingOn()
        {
            CollectionReference roomRef = FirebaseManager.db.Collection(GAMECHAT_ROOM); //채팅룸 컬렉션 참조
            Query allroomRef = roomRef;
            await allroomRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot allroomSnapshot = task.Result;
                foreach (DocumentSnapshot doc in allroomSnapshot.Documents)
                {
                    docID = doc.Id;
                    Debug.Log(docID);

                    Dictionary<string, object> docDictionary = doc.ToDictionary();
                    //Debug.Log(docDictionary[MEMBER].GetType());

                    List<object> memberList = (List<object>)docDictionary[MEMBER];
                    string open = docDictionary[ISOPEN].ToString();
                    Debug.Log(open);

                    if (open == "False")
                    {
                        int fcount = 0;
                        int mcount = 0;
                        foreach (Dictionary<string, object> m in memberList)
                        {
                            //Debug.Log(m[SEX]);

                            if (m[SEX].ToString() == "1")
                            {
                                mcount++;
                            }
                            else
                            {
                                fcount++;
                            }

                        }
                        Debug.Log("남성수: " + mcount + " 여성수: " + fcount);

                        countMembers = fcount + mcount;

                        Dictionary<string, object> addUser = new Dictionary<string, object>
                {
                    { NICKNAME , username },
                    { SEX , usersex }
                };
                        if (usersex == 2)
                        {
                            if (fcount <= 2)
                            {
                                roomRef.Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
                                fcount++;
                            }

                        }
                        else if (usersex == 1)
                        {
                            if (mcount <= 2)
                            {
                                roomRef.Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
                                mcount++;
                            }
                        }

                        if (fcount + mcount == 6)
                        {
                            //전체유저수가 6명이면 채팅방의 오픈 여부를 true로 바꿈
                            roomRef.Document(docID).UpdateAsync(ISOPEN, true); //채팅방 열림
                            roomRef.Document(docID).UpdateAsync(ISACTIVE, true); //채팅방 활성화
                            roomRef.Document(docID).UpdateAsync(OPENTIME, System.DateTime.Now.ToString()); //채팅방 열린 시간 기록
                        }
                        DocumentReference docRef = roomRef.Document(docID);
                        listener = docRef.Listen(snapshot =>
                        {
                            if (snapshot.Exists)
                            {
                                Debug.Log("콜백");
                                if (fcount + mcount == 6)
                                {
                                    Debug.Log("6명 채워짐");
                                    //전체유저수가 6명이면 채팅방의 오픈 여부를 true로 바꿈
                                    roomRef.Document(docID).UpdateAsync(ISOPEN, true); //채팅방 열림
                                    roomRef.Document(docID).UpdateAsync(ISACTIVE, true); //채팅방 활성화
                                    roomRef.Document(docID).UpdateAsync(OPENTIME, System.DateTime.Now.ToString()); //채팅방 열린 시간 기록
                                    CallCreatCR(); //게임챗 채팅방 생성
                                    listener.Stop();
                                    isMatchComplete = true; //매칭여부 true로 바꿈
                                }

                            }

                            else
                            {
                                Debug.Log(string.Format("문서가 존재하지 않습니다!", snapshot.Id)); //재매칭 시도해야됨
                                listener.Stop();
                                //matchingOn(); //재매칭 시도 UI 띄워주기
                                isMatchComplete = false; //매칭여부 false로 바꿈
                            }

                        });

                        return;
                    }

                }

                makeNewRoom(); // 방을 새로 생성
                return;
            });

        }


        async void makeNewRoom() //채팅방 생성
        {
            CallCreatCR();
            string channelID = newChatRoom["ChannelID"].ToString(); //채팅방ID 저장 //현진처리
            docID = newChatRoom["ChannelName"].ToString(); //채팅방이름 저장
            Dictionary<string, object> addUser = new Dictionary<string, object> //member에 추가할 유저정보
                {
                    { NICKNAME , username },
                    { SEX , usersex }
                };

            Dictionary<string, object> room = new Dictionary<string, object>
        {
            { CHANNELID , channelID }, //채팅방ID 받아와서 넣기
            { CREATETIME, System.DateTime.Now.ToString()}, //타임스탬프 (현재시간)
            { ISACTIVE, false },
            { ISOPEN , false },
            { MEMBER , "" },
            { OPENTIME , null } //타임스탬프 (6명된 시간)
        };
            //문서 새로 생성
            DocumentReference addmrRef = FirebaseManager.db.Collection(GAMECHAT_ROOM).Document(docID);
            await addmrRef.SetAsync(room).ContinueWithOnMainThread(task =>
            {
                Debug.Log(addmrRef.Id);
            });
            await FirebaseManager.db.Collection(GAMECHAT_ROOM).Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
            Debug.Log("채팅방 문서 생성됨");

            listener2 = addmrRef.Listen(snapshot =>
            {
                if (snapshot.Exists)
                {
                    Debug.Log("새로운 문서 업데이트");
                }
                else
                {
                    Debug.Log(string.Format("새로 생성한 문서가 존재하지 않습니다!", snapshot.Id)); //재매칭 시도해야됨
                    listener2.Stop();
                    matchingOn();
                }

            });
        }




        #endregion


        //임의의 형용사 + 명사 7자 이하의 채팅방 이름 생성 함수
        //스트링 타입의 (adj + " " + noun)를 반환 "형용사 한칸띄고 명사"
        public static string makeChatRoomName()
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
        public void CallCreatCR()
        {
            StartCoroutine("CreateChatR");
        }

        public static IEnumerator CreateChatR()
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

            //Debug.Log("현재 생성된 채팅방 아이디는 : " + www.result);
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
                string result = www.downloadHandler.text;
                Debug.Log(result);
                result = result.Substring(22, 36);
                GameChat.subscribe(result);
                GameChat.getChannel(result, null, (Channel channel, GameChatException Exception) =>
                {
                    if (Exception != null)
                    {
                        Debug.Log(Exception.message);
                        Debug.Log(Exception.code);
                        //에러 핸들링
                        Debug.Log("get channel 에러");
                        return;
                    }
                    CCName = channel.name;
                });

                newChatRoom.Add("ChannelID", result);//채널 id
                newChatRoom.Add("ChannelName", CCName);//채팅방이름


            }
        }


        //채팅방 정보 가져오는 함수


        public static void getChannelID()
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
                    for (int i = 0; i < CList.Count; i++)
                    {

                        Debug.Log(i);
                        Debug.Log("겟채널 가져온 리스트의 id : " + CList[i].id);
                        currentCRname = CList[i].id.ToString();
                        Debug.Log("겟채널 가져온 리스트의 rawJson : " + CList[i].rawJson);

                        /*
                        if(i == CList.Count)
                        {
                            return (CList[0].id);
                        }*/

                    }
                }
            });

        }

        public void sendMSG()
        {
            GameChat.sendMessage(CList[0].id, "메시지 전송");
        }

        public void GetMSG()
        {
            StartCoroutine("TestMSG"); //gettedCid);
            
            if (null != GameObject.Find("ThisRoomName"))
            {
                ThisCRoomNameTitle = GameObject.Find("ThisRoomName").GetComponent<Text>();
                ThisCRoomNameTitle.text = "dd";
            }
            else
            {
                Debug.Log("룸 네임 찾지 못함");
            }

            if (null != GameObject.Find("GContent"))
            {
                ContentRect = GameObject.Find("GContent").GetComponent<RectTransform>();
            }
            else
            {
                Debug.Log("GContent찾지 못함");
            }

        }

        public IEnumerator TestMSG(string id)
        {
            //마지막 채팅을 받아옴
            GameChat.getMessages(id, 0, 1, "", "", "", (List<Message> Messages, GameChatException Exception) =>
            {

                if (Exception != null)
                {
                    // Error 핸들링
                    return;
                }


                foreach (Message elem in Messages)
                {
                    LastMSG = elem;
                    //Debug.Log(LastMSG);
                }
            });


            GameChat.getMessages(id, 0, 200, "", "", "asc", (List<Message> Messages, GameChatException Exception) =>
            {

                if (Exception != null)
                {
                    // Error 핸들링
                    return;
                }

                int count = 0;
                foreach (Message elem in Messages)
                {
                    if (LastMSG.message_id != elem.message_id)
                    {
                        if (count == 0)
                        {
                            xMSG = elem;
                            Debug.LogError("이전: " + xMSG.content);
                            if (SampleGlobalData.G_User.id == elem.sender.id)//나
                            {
                                Debug.Log("나_시간있음_최초");
                                //메시지랑 시간만 표기
                                AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                                Area.transform.SetParent(ContentRect.transform, false);
                                Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                                Area.TextRect.GetComponent<Text>().text = elem.content;
                                Debug.Log(elem.content);
                                Area.TimeText.text = elem.created_at;

                                //텍스트가 두줄 이상인 경우 처리
                                float X = Area.TextRect.sizeDelta.x + 400;
                                float Y = Area.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area.BoxRect);

                                        if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area.BoxRect.sizeDelta = new Vector2(X, Y);

                            }
                            else
                            {
                                Debug.Log("타인_시간있음_최초");
                                //메시지, 이름, 시간표기
                                AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                                Area2.transform.SetParent(ContentRect.transform, false);
                                Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                                Area2.TextRect.GetComponent<Text>().text = elem.content;
                                Area2.UserText.text = elem.sender.name;
                                Area2.TimeText.text = elem.created_at;

                                //텍스트가 두줄 이상인 경우 처리
                                float X = Area2.TextRect.sizeDelta.x + 400;
                                float Y = Area2.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area2.BoxRect.sizeDelta = new Vector2(X - i * 2, Area2.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area2.BoxRect);

                                        if (Y != Area2.TextRect.sizeDelta.y) { Area2.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area2.BoxRect.sizeDelta = new Vector2(X, Y);

                            }
                            count = 1;
                        }
                        else
                        {
                            if (xMSG.created_at == elem.created_at)
                            {

                                //이전 메시지랑 시간이 같으면
                                if (SampleGlobalData.G_User.id == elem.sender.id)//나
                                {
                                    Debug.Log("나_시간없음");
                                    //메시지만 표기
                                    AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                                    Area.transform.SetParent(ContentRect.transform, false);
                                    Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                                    Area.TextRect.GetComponent<Text>().text = elem.content;
                                    Area.TimeText.text = "";
                                    Debug.Log("타인 시간없음 :  " + (xMSG.created_at.ToString() == elem.created_at.ToString()));

                                    //텍스트가 두줄 이상인 경우 처리
                                    float X = Area.TextRect.sizeDelta.x + 400;
                                    float Y = Area.TextRect.sizeDelta.y;
                                    if (Y > 700)
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                                            chatManager.Fit(Area.BoxRect);

                                            if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                        }
                                    }
                                    else Area.BoxRect.sizeDelta = new Vector2(X, Y);

                                }
                                else
                                {
                                    Debug.Log("타인_시간없음");
                                    //메시지, 이름 표기
                                    AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                                    Area2.transform.SetParent(ContentRect.transform, false);
                                    Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                                    Area2.TextRect.GetComponent<Text>().text = elem.content;
                                    Area2.UserText.text = elem.sender.name;
                                    Area2.TimeText.text = "";
                                    Debug.Log("타인 시간없음 :  " + (xMSG.created_at.ToString() == elem.created_at.ToString()));


                                    //텍스트가 두줄 이상인 경우 처리
                                    float X = Area2.TextRect.sizeDelta.x + 400;
                                    float Y = Area2.TextRect.sizeDelta.y;
                                    if (Y > 700)
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            Area2.BoxRect.sizeDelta = new Vector2(X - i * 2, Area2.BoxRect.sizeDelta.y);
                                            chatManager.Fit(Area2.BoxRect);

                                            if (Y != Area2.TextRect.sizeDelta.y) { Area2.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                        }
                                    }
                                    else Area2.BoxRect.sizeDelta = new Vector2(X, Y);
                                }
                            }
                            else
                            {

                                if (SampleGlobalData.G_User.id == elem.sender.id)//나
                                {
                                    Debug.Log("나_시간있음");
                                    //나: 메시지랑 시간만 표기
                                    AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                                    Area.transform.SetParent(ContentRect.transform, false);
                                    Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                                    Area.TextRect.GetComponent<Text>().text = elem.content;
                                    Area.TimeText.text = elem.created_at;

                                    //텍스트가 두줄 이상인 경우 처리
                                    float X = Area.TextRect.sizeDelta.x + 400;
                                    float Y = Area.TextRect.sizeDelta.y;
                                    if (Y > 700)
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                                            chatManager.Fit(Area.BoxRect);

                                            if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                        }
                                    }
                                    else Area.BoxRect.sizeDelta = new Vector2(X, Y);
                                }
                                else
                                {
                                    Debug.Log("타인_시간있음");
                                    //메시지, 이름, 시간표기
                                    AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                                    Area2.transform.SetParent(ContentRect.transform, false);
                                    Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                                    Area2.TextRect.GetComponent<Text>().text = elem.content;
                                    Area2.UserText.text = elem.sender.name;
                                    Area2.TimeText.text = elem.created_at;

                                    //텍스트가 두줄 이상인 경우 처리
                                    float X = Area2.TextRect.sizeDelta.x + 400;
                                    float Y = Area2.TextRect.sizeDelta.y;
                                    if (Y > 700)
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            Area2.BoxRect.sizeDelta = new Vector2(X - i * 2, Area2.BoxRect.sizeDelta.y);
                                            chatManager.Fit(Area2.BoxRect);

                                            if (Y != Area2.TextRect.sizeDelta.y) { Area2.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                        }
                                    }
                                    else Area2.BoxRect.sizeDelta = new Vector2(X, Y);
                                }
                            }
                            //이전 메시지 초기화
                            xMSG = elem;
                            Debug.LogError("이후: " + xMSG.content);
                        }
                    }
                    else
                    {
                        if (SampleGlobalData.G_User.id == elem.sender.id)
                        {
                            if (xMSG.created_at == elem.created_at)
                            {
                                //나_시간없음
                                AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                                Area.transform.SetParent(ContentRect.transform, false);
                                Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                                Area.TextRect.GetComponent<Text>().text = elem.content;


                                //텍스트가 두줄 이상인 경우 처리
                                float X = Area.TextRect.sizeDelta.x + 400;
                                float Y = Area.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area.BoxRect);

                                        if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area.BoxRect.sizeDelta = new Vector2(X, Y);
                            }
                            else
                            {
                                //나_시간있음
                                AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                                Area.transform.SetParent(ContentRect.transform, false);
                                Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                                Area.TextRect.GetComponent<Text>().text = elem.content;
                                Area.TimeText.text = elem.created_at;

                                //텍스트가 두줄 이상인 경우 처리
                                float X = Area.TextRect.sizeDelta.x + 400;
                                float Y = Area.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area.BoxRect);

                                        if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area.BoxRect.sizeDelta = new Vector2(X, Y);
                            }
                        }
                        else
                        {
                            if (xMSG.created_at == elem.created_at)
                            {
                                //타인_시간없음
                                AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                                Area2.transform.SetParent(ContentRect.transform, false);
                                Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                                Area2.TextRect.GetComponent<Text>().text = elem.content;
                                Area2.UserText.text = elem.sender.name;


                                //텍스트가 두줄 이상인 경우 처리
                                float X = Area2.TextRect.sizeDelta.x + 400;
                                float Y = Area2.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area2.BoxRect.sizeDelta = new Vector2(X - i * 2, Area2.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area2.BoxRect);

                                        if (Y != Area2.TextRect.sizeDelta.y) { Area2.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area2.BoxRect.sizeDelta = new Vector2(X, Y);
                            }
                            else
                            {
                                //타인_시간있음
                                AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                                Area2.transform.SetParent(ContentRect.transform, false);
                                Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                                Area2.TextRect.GetComponent<Text>().text = elem.content;
                                Area2.UserText.text = elem.sender.name;
                                Area2.TimeText.text = elem.created_at;

                                //텍스트가 두줄 이상인 경우 처리
                                float X = Area2.TextRect.sizeDelta.x + 400;
                                float Y = Area2.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area2.BoxRect.sizeDelta = new Vector2(X - i * 2, Area2.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area2.BoxRect);

                                        if (Y != Area2.TextRect.sizeDelta.y) { Area2.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area2.BoxRect.sizeDelta = new Vector2(X, Y);
                            }
                        }
                    }
                }
            });
            yield return null;

            yield return null;

        }

        public static int CountingNewMSG(string Channel_ID)
        {
            GameChat.getMessages(Channel_ID, 0, 1, "", "", "");
            //채널id를 받아와서 최근 메시지 부터 비교, 개수를 카운팅
            int newMSGint = 0;
            return newMSGint;

        }

        public static int count;
        public static int setNewMSGCount(string id)
        {
            GameChat.getMessages(id, 0, 100, "", "", "", (List<Message> Messages, GameChatException Exception) =>
            {

                if (Exception != null)
                {
                    // Error 핸들링
                    return;
                }

                int count = 0;
                foreach (Message elem in Messages)
                {
                    string curMsgID = elem.message_id;
                    if (curMsgID != PlayerPrefs.GetString("LastMSGID"))
                    {
                        count += 1;
                        Debug.Log("안읽은 메시지: " + count);
                    }
                }

                CurChatInfo[4] = count.ToString();
            });
            return count;
        }


        public static string curMsg = "";
        public static void getCurMSG(string id, System.Action<string, string> callBack)
        {
            GameChat.getMessages(id, 0, 1, "", "", "", (List<Message> Messages, GameChatException Exception) =>
            {

                if (Exception != null)
                {
                    // Error 핸들링
                    
                }

                foreach (Message elem in Messages)
                {
                    curMsg = elem.content;
                    Debug.Log(elem.content);

                }

                callBack(id, curMsg);
                Debug.Log("getCurMSG-Msg: " + curMsg);
            });    
            
        }

        public static string curRoomName = "";
        public static string getCurRoomName(string id)
        {
            Debug.Log("getCurRoomName-id : " + id);
            GameChat.getChannel(id, null, (Channel channel, GameChatException Exception) =>
            {
                Debug.LogError("#### id ###: " + id);
                Debug.LogError("#### channel ###: " + channel);
                if (Exception != null)
                {
                    Debug.Log(Exception.message);
                    Debug.Log(Exception.code);
                    //에러 핸들링
                    Debug.Log("get channel 에러");
                    return;
                }
                CurChatInfo[0] = channel.name;
                Debug.Log(channel.created_at);


                CurChatInfo[2] = channel.created_at.Substring(11, 8).ToString();
                CurChatInfo[3] = channel.created_at.Substring(0, 10).ToString();

                //getLostTime();


            });

            return curRoomName;
        }





        public static string getLostTime(string openTime)
        {
            //string cTime = GameChatSample.NewChatManager.CurChatInfo[2];//생성시간
            //string cDay = GameChatSample.NewChatManager.CurChatInfo[3];//생성날짜
            //string c = cDay + " " + cTime;
            //string nTime = DateTime.Now.ToString("u").Substring(11, 8);//현재시간
            //string nDay = DateTime.Now.ToString("u").Substring(0, 10);//현재날짜
            //string n = nDay + " " + nTime;
            //Debug.Log(cTime + "/" + cDay + "/" + nTime + "/" + nDay);
            //Debug.Log(c+ "/" +n );
            //Debug.Log(Convert.ToDateTime(n)+"/"+ Convert.ToDateTime(c));
            //TimeSpan goTime = Convert.ToDateTime(n) - Convert.ToDateTime(c);
            TimeSpan goTime = DateTime.Now-Convert.ToDateTime(openTime);
            // Debug.Log(Convert.ToDateTime(openTime) + "////" + DateTime.Now);
            //Debug.Log("고타임   "+goTime.ToString());
            //Debug.Log("고타임데이  " + goTime.Days+"  고타임아워  " +goTime.Hours + "  고타임미닛  " + goTime.Minutes);

            if (goTime.Days <= 0)
            {
                if (goTime.Hours == 0)
                {
                    if (goTime.Minutes > 20)
                    {
                        //CurChatInfo[5] = "종료";
                        return "종료";
                    }
                    else
                    {
                        return (20 - goTime.Minutes).ToString() + "분";
                        //CurChatInfo[5] = (20 - goTime.Minutes).ToString() + "분";
                        //20분에서 경과 시간 빼주기
                    }
                }
                else
                {
                    //CurChatInfo[5] = "종료";
                    return "종료";
                }

            }
            else
            {
                return "종료";
                //CurChatInfo[5] = "종료";
            }

        }

    }
}