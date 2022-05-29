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
using Prefebs;
using CLCM;

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
      

        //채팅리스트 받아와서 id를 가져오기 위한 리스트, 이름 초기화
        public static string currentCRname = "";
        public static List<Channel> CList = new List<Channel>();


        //chatmanager를 참조받아오기 위한 선언
        public ChatManager chatManager;

        public static string CCName;
        public static string[] CurChatInfo = new string[5];



        //매칭을 위한 선언부
        public static string username;
        public int usersex;
        public string docID; //도큐먼트 고유ID 참조하기 위해 필요
        public string channelID; //채팅방 정보 저장
        public int countMembers;
        public int fcount;
        public int mcount;
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

        // 타이머 코드를 위한 변수 선언
        public float time_current = 5f;
        public GameObject Dialog_Matching_ReMatching; //매칭실패시 띄우는 다이얼로그

        

        //채팅방 이름 더블체크
        public static string nowChatName;
        public static bool isDouble;

        // Start is called before the first frame update
        void Start()
        {
            //don't destroy 처리
            DontDestroyOnLoad(this.gameObject);

            //채팅방 이름 랜덤 생성을 위한 텍스트 파싱 함수 호출
            ReadCSV();

            //도큐먼트 id초기화를 위한 과정// 필요함
            docID = "";
            //countMembers = 0; //채팅방멤버수 초기화
            fcount = 0; //여성멤버수 초기화
            mcount = 0; //남성멤버수 초기화

            //매칭 과정을 위한 테스트설정
            username = PlayerPrefs.GetString("GCName");
            //username = "솔비";
            usersex = FirebaseManager.sex;
            Debug.Log("현재 로그인 유저 닉네임 : " + username);
            Debug.Log("현재 로그인 유저 성별 : " + usersex);
        }

        // Update is called once per frame
        void Update()
        {
            if (Prefebs.getGCID.clickCard == true)
            {
                GetMSG();
            }
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
        public void matchingOn()
        {
            CollectionReference roomRef = FirebaseManager.db.Collection(GAMECHAT_ROOM); //채팅룸 컬렉션 참조
            Query allroomRef = roomRef;
            allroomRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot allroomSnapshot = task.Result;
                foreach (DocumentSnapshot doc in allroomSnapshot.Documents)
                {
                    Dictionary<string, object> docDictionary = doc.ToDictionary();
                    docID = doc.Id; //채팅방이름 저장
                    channelID = docDictionary[CHANNELID].ToString(); //채팅방아이디 저장

                    gameSceneManager.chatRname = docID; //그룹챗 씬에서 로드할 채팅방 이름 저장
                    gameSceneManager.chatRID = channelID; //그룹챗 씬에서 로드할 채널 ID 저장
                    

                    List<object> memberList = (List<object>)docDictionary[MEMBER];
                    string open = docDictionary[ISOPEN].ToString();
                    Debug.Log(docID + open);

                    if (open == "False")
                    {
                        fcount = 0;
                        mcount = 0;
                        countMembers = 0;
                        foreach (Dictionary<string, object> m in memberList)
                        {
                            if (m[SEX].ToString() == "1") { mcount++; }
                            else { fcount++; }  
                        }
                        Debug.Log("기존 남성수 " + mcount + "+ 기존 여성수 " + fcount);

                        Dictionary<string, object> addUser = new Dictionary<string, object>
                {
                    { NICKNAME , username },
                    { SEX , usersex }
                };
                        string isopen = "";
                        if (usersex == 2 && fcount <= 2)
                        {
                            roomRef.Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
                            fcount++;
                            Debug.Log("여성수:" + fcount);

                            countMembers = mcount + fcount;

                            Debug.Log("업데이트된 전체유저수" + countMembers);

                            if (countMembers == 6) //내가 들어오고 유저수를 셌을 때 6명이면 채팅방 오픈/액티브를 트루로 바꿈
                            {
                                Debug.Log("6명 채워짐");
                                //전체유저수가 6명이면 채팅방의 오픈 여부를 true로 바꿈
                                roomRef.Document(docID).UpdateAsync(ISOPEN, true); //채팅방 열림
                                roomRef.Document(docID).UpdateAsync(ISACTIVE, true); //채팅방 활성화
                                roomRef.Document(docID).UpdateAsync(OPENTIME, System.DateTime.Now.ToString()); //채팅방 열린 시간 기록
                                isopen = "True";
                            }
                            DocumentReference docRef = roomRef.Document(docID);
                            listener = docRef.Listen(snapshot =>
                            {
                                if (snapshot.Exists)
                                {
                                    Debug.Log("콜백");
                                    if (isopen == "True") //채팅방 오픈 값 트루이면
                                    {
                                        listener.Stop();
                                        Invoke("showChatRoom", 5f);
                                    }
                                }

                                else
                                {
                                    Debug.Log(string.Format("문서가 존재하지 않습니다!", snapshot.Id));
                                    listener.Stop();
                                    Dialog_Matching_ReMatching.SetActive(true); //매칭실패 다이얼로그
                                }

                            });

                            return;

                        }
                        else if (usersex == 1 && mcount <= 2)
                        {
                            roomRef.Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
                            mcount++;
                            Debug.Log("남성수" + mcount);

                            countMembers = mcount + fcount;

                            Debug.Log("업데이트된 전체유저수" + countMembers);

                            if (countMembers == 6) //내가 들어오고 유저수를 셌을 때 6명이면 채팅방 오픈/액티브를 트루로 바꿈
                            {
                                Debug.Log("6명 채워짐");
                                //전체유저수가 6명이면 채팅방의 오픈 여부를 true로 바꿈
                                roomRef.Document(docID).UpdateAsync(ISOPEN, true); //채팅방 열림
                                roomRef.Document(docID).UpdateAsync(ISACTIVE, true); //채팅방 활성화
                                roomRef.Document(docID).UpdateAsync(OPENTIME, System.DateTime.Now.ToString()); //채팅방 열린 시간 기록
                                isopen = "True";
                            }

                            DocumentReference docRef = roomRef.Document(docID);
                            listener = docRef.Listen(snapshot =>
                            {
                                if (snapshot.Exists)
                                {
                                    Debug.Log("콜백");
                                    if (isopen == "True") //채팅방 오픈 값 트루이면
                                    {
                                        listener.Stop();
                                        Invoke("showChatRoom", 5f);
                                    }
                                }

                                else
                                {
                                    Debug.Log(string.Format("문서가 존재하지 않습니다!", snapshot.Id));
                                    listener.Stop();
                                    Dialog_Matching_ReMatching.SetActive(true); //매칭실패 다이얼로그
                                }

                            });

                            return;
                        }
                    }
                }

                //makeNewRoom();
                Debug.Log("들어갈수 있는방 없음");
                makeChatRoomName();
                return;
            });

        }

        void showChatRoom()
        {
            gameSceneManager.LoadScene_GroupChat();

        }

        void makeNewRoom() //채팅방 생성
        {
            StartCoroutine("CreateChatR");
        }
        
        async void makeNewRoom2()
        {
            channelID = newChatRoom["ChannelID"].ToString(); //채팅방ID 저장
            docID = newChatRoom["ChannelName"].ToString(); //채팅방이름 저장
            gameSceneManager.chatRname = docID;
            gameSceneManager.chatRID = channelID;
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
            await addmrRef.SetAsync(room);
            await FirebaseManager.db.Collection(GAMECHAT_ROOM).Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
            Debug.Log("채팅방 문서 생성됨");

            listener2 = addmrRef.Listen(snapshot =>
            {
                if (snapshot.Exists)
                {
                    Dictionary<string, object> doc = snapshot.ToDictionary();
                    string isopen = doc[ISOPEN].ToString();
                    //gameSceneManager.IDoTime.Add(doc[CHANNELID].ToString(), doc[ISOPEN].ToString());
                    Debug.Log(isopen);
                    Debug.Log("새로운 문서 업데이트");
                    if (isopen == "True")
                    {
                        listener2.Stop();
                        Invoke("showChatRoom", 5f);
                    }
                }
                else
                {
                    Debug.Log(string.Format("새로 생성한 문서가 존재하지 않습니다!", snapshot.Id)); //재매칭 시도해야됨
                    listener2.Stop();
                    Dialog_Matching_ReMatching.SetActive(true);
                    newChatRoom.Clear();//채널id랑 채팅방이름 저장한 딕셔너리 초기화
                }

            });
        }


        #endregion


        //임의의 형용사 + 명사 7자 이하의 채팅방 이름 생성 함수
        //스트링 타입의 (adj + " " + noun)를 반환 "형용사 한칸띄고 명사"
        public async Task makeChatRoomName()
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
                    await CRdoubleCheck(adj + " " + noun);
                    Debug.Log("함수 실행 후 ######" + isDouble.ToString());
                    
                    if (isDouble == false)
                    {

                        Debug.Log("이즈 더블 폴스임 "+adj + " " + noun);
                        nowChatName = (adj + " " + noun);
                        CallCreatCR();
                        break;

                    }
                    else if(isDouble ==true)
                    {
                        Debug.Log("######같은 채팅방이름이라 다시 호출");
                    }

                    
                }
            }


        }

        public static async Task CRdoubleCheck(string name)
        {
            DocumentReference docRef = FirebaseManager.db.Collection(GAMECHAT_ROOM).Document(name);
            await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists) 
                {
                    isDouble = true; //해당 이름이 이미 있음
                    Debug.Log("채팅방 이름 이미 있음 : " + name);
                    Debug.Log(isDouble.ToString());
;               }
                else
                {
                    isDouble = false;
                    Debug.Log("채팅방 이름 없음");
                }
            });
        }

        public void CallCreatCR()
        {
            StartCoroutine("CreateChatR");
        }

        IEnumerator CreateChatR()
        {
            string url = "https://dashboard-api.gamechat.naverncp.com/v1/api/project/e3558324-2d64-47d0-bd7a-6fa362824bd7/channel";
            string APIKey = "ec31cc21b559da9eb19eaec2dadcd50ed786857a740a561d";



            WWWForm form = new WWWForm();
            //makeChatRoomName();
            Debug.Log("####여기 이름있냐?####     " + nowChatName);
            //string projectID = "e3558324 - 2d64 - 47d0 - bd7a - 6fa362824bd7";
            //form.AddField("projectId", projectID);
            form.AddField("name", nowChatName);


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
                string result = www.downloadHandler.text.Substring(22, 36);

                newChatRoom.Add("ChannelID", result);//채널 id
                newChatRoom.Add("ChannelName", nowChatName);//채팅방이름
                Debug.Log(result);
                Debug.Log(nowChatName);

                makeNewRoom2();
                
            }
        }
        
        public void sendMSG()
        {
            GameChat.sendMessage(CList[0].id, "메시지 전송");
        }

        public void GetMSG()
        {
            chatlistSceneManager.isEqualName();
            gameSceneManager.LoadScene_GroupChat();
            Prefebs.getGCID.clickCard = false;

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
            
            TimeSpan goTime = DateTime.Now - Convert.ToDateTime(openTime);
            
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