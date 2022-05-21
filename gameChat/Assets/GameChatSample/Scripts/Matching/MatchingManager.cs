using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using FireStoreScript;
using System.Threading;
using System.Threading.Tasks;

public class MatchingManager : MonoBehaviour
{
    public string username;
    public int usersex;
    public string docID; //도큐먼트 고유ID 참조하기 위해 필요
    public int count;
    //public List<string> member;

    public ListenerRegistration listener;
    public ListenerRegistration listener2;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //username = PlayerPrefs.GetString("name");
        //sex = PlayerPrefs.GetString("여");
        username = "솔비";
        usersex = 2;
        Debug.Log("현재 로그인 유저 닉네임 : " + username);
        Debug.Log("현재 로그인 유저 성별 : " + usersex);
        docID = "";
        
    }
    
    public static string GAMECHAT_ROOM = "gameChatRoom";
    public static string ISOPEN = "isOpen";
    public static string ISACTIVE = "isActive";
    public static string MEMBER = "member";
    public static string NICKNAME = "nickName";
    public static string SEX = "sex";
    public static string CHANNELID = "channelID";
    public static string CREATETIME = "createTime";
    public static string OPENTIME = "openTime";
    //public ListenerRegistration listener;

    //문서의 데이터가 변경될 때마다 전체유저수 불러와서 전체유저수가 6이 되면 매칭종료
   
    public async void matchingOn()
    {
        CollectionReference roomRef = FirebaseManager.db.Collection(GAMECHAT_ROOM); //채팅룸 컬렉션 참조
        Query allroomRef = roomRef;
        await allroomRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allroomSnapshot = task.Result;
            foreach(DocumentSnapshot doc in allroomSnapshot.Documents)
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

                    count = fcount + mcount;

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
                        }

                        else
                        {
                            Debug.Log(string.Format("문서가 존재하지 않습니다!", snapshot.Id)); //재매칭 시도해야됨
                            //matchingOn();
                            listener.Stop();
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
        //StartCoroutine("CreateChatR"); //채팅방ID 생성
        string channelID = "asdf"; //채팅방ID 저장
        docID = "아무이름"; //채팅방이름 저장
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

            //listener.Stop();
        });
    }
    
}
