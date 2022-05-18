using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using FireStoreScript;


public class MatchingManager : MonoBehaviour
{
    //public static FirebaseFirestore db;
    private FirebaseManager db;
    public string username; 
    public string sex; 
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        db = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();
        username = PlayerPrefs.GetString("name");
        sex = PlayerPrefs.GetString("sex");

        Debug.Log("현재 로그인 유저 닉네임 : " + username);
        Debug.Log("현재 로그인 유저 성별 : " + sex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddUser() //가입 시 한번만 실행 (매칭DB)
    {
        
        DocumentReference Mref = db.db.Collection("matchingUsers").Document(username);
        Mref.SetAsync(new Dictionary<string, object>()
        {
            {"name", username},
            {"sex", sex },
            {"isActive", false }
        });
    }

    public void OnclickMatching() //매칭버튼 눌렀을 때 호출할 함수
    {
        //AddUser();
        isActive = true; //매칭가능여부를 true로 바꿈

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

        matching();

    }
    async void matching() //MatchingRoom DB
    {
        //string roomId = null; //채팅방id 

        if(PlayerPrefs.GetString("sex") == "여") //유저가 여성이라면
        {
            Query femaleRef = db.db.Collection("matchingRoom").WhereEqualTo("female", false); //여성 수가 3명이 다 차지 않은 방 찾기
            
            if (femaleRef == null) //여성 유저가 3명이 다 차지 않은 방이 없다면
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

            }

            else if (femaleRef != null) //여성 유저가 3명이 다 차지 않은 방이 있다면
            {
                await femaleRef.GetSnapshotAsync().ContinueWithOnMainThread((QuerySnapshotTask) =>
                {
                    foreach (DocumentSnapshot roomdoc in QuerySnapshotTask.Result.Documents)
                    {
                        Debug.Log("여성유저가 3명 미만인 방 : " + roomdoc.Id);
                    }
                });
            }
        }

        else if (PlayerPrefs.GetString("sex") == "남") //유저가 남성이라면
        {
            Query maleRef = db.db.Collection("matchingRoom").WhereEqualTo("male", false); //남성 수가 3명이 다 차지 않은 방 찾기
            
            if (maleRef == null) //남성 유저가 3명이 다 차지 않은 방이 없다면
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

            }

            else if(maleRef != null) //여성 유저가 3명이 다 차지 않은 방이 있다면
            {
                await maleRef.GetSnapshotAsync().ContinueWithOnMainThread((QuerySnapshotTask) =>
                {
                    foreach (DocumentSnapshot roomdoc in QuerySnapshotTask.Result.Documents)
                    {
                        Debug.Log("남성유저가 3명 미만인 방 : " + roomdoc.Id);
                    }
                });
                
            }
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
        Dictionary<string, object> room = new Dictionary<string, object>
        {
            {"count", count },
            {"f_count", count_f },
            { "m_count", count_m},
            {"female" , female },
            {"male" , male },
            {"m1" , m1 },
            {"m2" , m2 },
            {"m3" , m3 },
            {"m4" , m4 },
            {"m5" , m5 },
            {"m6" , m6 }
        };
        db.db.Collection("matchingRoom").AddAsync(room).ContinueWithOnMainThread(task =>
        {
            DocumentReference addRoom = task.Result;
            Debug.Log(string.Format(addRoom.Id));
        });
        
    }
}
