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
    private FirebaseManager db;
    public string username; 
    public string sex; 
    public bool isActive;

    public string gamechatchannel; //게임챗 채팅방 채널 ID 받아올 변수

    CollectionReference matchingRoomRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoom");

    // Start is called before the first frame update
    void Start()
    {
        db = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();
        //username = PlayerPrefs.GetString("name");
        //sex = PlayerPrefs.GetString("sex");
        //username = "나영";
        //sex = "여";
        Debug.Log("현재 로그인 유저 닉네임 : " + username);
        Debug.Log("현재 로그인 유저 성별 : " + sex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddUser() //가입 시 한번만 실행(매칭DB에 문서 새로 생성해줌) -> 이 db가 필요있나? 어차피 닉네임, 성별은 playterprefs로 저장되고, isActive는 아예 필요없어보임
    {
        
        DocumentReference Mref = FireStoreScript.FirebaseManager.db.Collection("matchingUsers").Document(username);
        Mref.SetAsync(new Dictionary<string, object>()
        {
            {"name", username},
            {"sex", sex },
            {"isActive", false }
        });
    }

    public void OnclickMatching() //매칭버튼 눌렀을 때 호출할 함수
    {
        //Debug.Log(PlayerPrefs.GetString("username"));
        //AddUser();
        isActive = true; //매칭가능여부를 true로 바꿈

        /*
        Query query = db.db.Collection("matchingUsers").WhereEqualTo("name", username);
        query.GetSnapshotAsync().ContinueWithOnMainThread((QuerySnapshotTask) =>
        {
            foreach(DocumentSnapshot doc in QuerySnapshotTask.Result)
            {
                Debug.Log(string.Format(doc.Id));
            }

        });

        DocumentReference userRef = db.db.Collection("matchingUsers").Document(username);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            {"isActive", true }
        };
        userRef.UpdateAsync(updates).ContinueWithOnMainThread(task => {
            Debug.Log(isActive);
        });
        */
        matching();

    }

    
    void matching() //MatchingRoom DB
    {
        //string roomId = null; //채팅방id 

        if(sex == "여") //유저가 여성이라면
        {
            //Debug.Log("방찾기");
            Query femaleRef = matchingRoomRef.WhereEqualTo("female", false).Limit(1); //여성 유저 2명 이하인 방 중에 1개만 반환 
            ListenerRegistration listener = femaleRef.Listen(snapshot =>
            {
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {

                    Debug.Log("join 가능한 방 : " + doc.Id);

                    //doc.GetValue < "count" > //count의 value 받아와야함 
                    count = count + 1; //현재 전체 유저 수 + 1 
                    DocumentReference docRef = matchingRoomRef.Document(doc.Id);
                    Dictionary<string, object> newUser = new Dictionary<string, object>
                    {
                        
                        {"count", count}, //전체 유저 수 1 올려주기
                        { "count_f" , count_f} //여성 유저 수 1 올려주기
                    };
                    docRef.UpdateAsync(newUser); //count 정보 업데이트
                    
                    count = 1;
                    count_f = 1;
                    count_m = 0;
                    female = false;
                    male = false;
                    m1 = username;
                    m2 = null;
                    m3 = null;
                    m4 = null;
                    m5 = null;
                    m6 = null;

                    makeRoom(); //새로운 방 생성

                }
                /*
                if (snapshot != null)
                {
                    foreach(DocumentSnapshot doc in snapshot.Documents)
                    {
                        Debug.Log("join 가능한 방 : " + doc.Id);
                    }
                    
                }
                else
                {
                    count = 1;
                    count_f = 1;
                    count_m = 0;
                    female = false;
                    male = false;
                    m1 = username;
                    m2 = null;
                    m3 = null;
                    m4 = null;
                    m5 = null;
                    m6 = null;

                    makeRoom(); //새로운 방 생성
                    Debug.Log("새로운 방 생성 필요");

                }*/
            });
            listener.Stop();
            /*
            if (femaleRef != null) //여성 유저가 3명이 다 차지 않은 방이 있다면
            {
                ListenerRegistration listener = femaleRef.Listen(snapshot =>
                {
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Debug.Log("join 가능한 방 : " + doc.Id);
                    }

                });
                listener.Stop();
            }
            else //여성 유저가 3명이 다 차지 않은 방이 없다면
            {
                count = 1;
                count_f = 1;
                count_m = 0;
                female = false;
                male = false;
                m1 = username;
                m2 = null;
                m3 = null;
                m4 = null;
                m5 = null;
                m6 = null;

                makeRoom(); //새로운 방 생성
                Debug.Log("새로운 방 생성 필요");
            }*/
        }

        else if (sex == "남") //유저가 남성이라면
        {
            Query maleRef = null;
            maleRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoom").WhereEqualTo("male", false); //남성 수가 3명이 다 차지 않은 방 찾기
            ListenerRegistration listener = maleRef.Listen(snapshot =>
            {
                if (snapshot != null) //여성 유저가 3명이 다 차지 않은 방이 있다면
                {
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Debug.Log("join 가능한 방 : " + doc.Id);
                    }
                }
                else //여성 유저가 3명이 다 차지 않은 방이 없다면
                {
                    count = 1;
                    count_f = 1;
                    count_m = 0;
                    female = false;
                    male = false;
                    m1 = username;
                    m2 = null;
                    m3 = null;
                    m4 = null;
                    m5 = null;
                    m6 = null;

                    makeRoom(); //새로운 방 생성
                    Debug.Log("새로운 방 생성 필요");

                }
            });
            listener.Stop();
            /*
            if (maleRef != null) //남성 유저가 3명이 다 차지 않은 방이 있다면
            {
                ListenerRegistration listener = maleRef.Listen(snapshot =>
                {
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Debug.Log("join 가능한 방 : " + doc.Id);
                    }

                });
                listener.Stop();
            }
            else //남성 유저가 3명이 다 차지 않은 방이 없다면
            {
                count = 1;
                count_f = 0;
                count_m = 1;
                female = false;
                male = false;
                m1 = username;
                m2 = null;
                m3 = null;
                m4 = null;
                m5 = null;
                m6 = null;
                
                makeRoom(); //새로운 방 생성
                Debug.Log("새로운 방 생성 필요");
            }
            */
        }
        
    }


    public int count;
    public int count_f;
    public int count_m;
    public bool female;
    public bool male;
    public string m1;
    public string m2;
    public string m3;
    public string m4;
    public string m5;
    public string m6;

    public void makeRoom() //채팅방 생성
    {
        //string roomID = gamechatchannel; //채팅방 채널 할당

        Dictionary<string, object> room = new Dictionary<string, object>
        {
            {"count", count },
            {"f_count", count_f },
            { "m_count", count_m},
            {"female" , female },
            {"male" , male },
            {"m1" , username },
            {"m2" , m2 },
            {"m3" , m3 },
            {"m4" , m4 },
            {"m5" , m5 },
            {"m6" , m6 }
        };
        FireStoreScript.FirebaseManager.db.Collection("matchingRoom").AddAsync(room); //문서 새로 생성

        Debug.Log("채팅방 문서 생성됨");


    }
}
