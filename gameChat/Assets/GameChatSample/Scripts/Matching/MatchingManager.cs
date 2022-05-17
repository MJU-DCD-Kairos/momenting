using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class MatchingManager : MonoBehaviour
{
    //public static FirebaseFirestore db;
    private FirebaseManager db;
    public string username; 
    public string sex; 
    public bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        db = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();
        username = PlayerPrefs.GetString("name");
        sex = PlayerPrefs.GetString("sex");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    class User //유저 정보를 담는 클래스
    {
        public string name;
        public string sex;
        public bool isActive;


        public User(string name, string sex, bool isActive) //초기화하기 쉽게 생성자 사용
        {
            this.name = name;
            this.sex = sex;
            this.isActive = isActive;
        }
    }*/
    public void AddUser() //가입 시 한번만 실행 (매칭DB)
    {
        
        DocumentReference Mref = db.db.Collection("MatchingUsers").Document(username);
        Mref.SetAsync(new Dictionary<string, object>()
        {
            {"name", username},
            {"sex", sex },
            {"isActive", false }
        });
    }

    public void OnclickMatching() //매칭버튼 눌렀을 때 호출할 함수
    {
        isActive = true; //매칭가능여부를 true로 바꿈

        DocumentReference userRef = db.db.Collection("MatchingUsers").Document(username);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            {"isActive", isActive }
        };
        userRef.UpdateAsync(updates).ContinueWithOnMainThread(task => {
            Debug.Log(isActive);
        });

        //matching();

    }
    public void matching() //MatchingRoom DB에 들어감
    {
        string roomId = null; //채팅방id 

        DocumentReference roomRef = db.db.Collection("MatchingRoom").Document(roomId);


        //roomRef.SetAsync


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
