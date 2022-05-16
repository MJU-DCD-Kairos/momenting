using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.UI;
using System;

public class FirebaseManager : MonoBehaviour
{
    public FirebaseFirestore db;

    public InputField ID;
    public InputField PW;
    public string uid;
    public string upw;

    void Start()
    {
        //씬매니저 파괴 방지를 위한 코드
        DontDestroyOnLoad(this.gameObject);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            //var dependencyStatus = task.Result;
            if (task.Result == DependencyStatus.Available)
            {
                //app = Firebase.FirebaseApp.DefaultInstance;
                db = FirebaseFirestore.DefaultInstance; //Cloud Firestore 인스턴스 초기화
                Debug.Log("DB 연결 성공");
                //useridDB();
                //ReadData();
                //LoadData();
                makeUserData();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });

    }
    
    //QuerySnapshot snapshot = await.userRef.GetSnapshotAsync();
    //Debug.Log(user1);
    //DocumentReference docRef = db.Collection("Users").Document("Person"); //Users 컬렉션에서 "Person"문서 불러오기
    //QuerySnapshot snapshot = await userRef.GetSnapshotAsync(); //문서 안에 있는 data 가져오라고 서버에 요청
    /*
    foreach (DocumentSnapshot document in snapshot.Documents)
    {
        Dictionary<string, object> documentDictionary = document.ToDictionary(); //받은 컬렉션에서 .document라는 속성을 통해각 문서들에 접근. 각 문서를 dictionary로 받아서 .todictionary를 통해 바꿔주면 수월하게 데이터를 처리할 수 있다.
        Debug.Log("name:  " + documentDictionary["name"] as string);
        if (documentDictionary.ContainsKey("name"))
        {
            Debug.Log("t")
        }
    }
    */

    public void ReadData()
    {
        
        CollectionReference userRef = db.Collection("Users"); //유저들의의 데이터가 저장된 컬렉션 불러오기
        Debug.Log(userRef.GetType());
        
        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            
            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                Debug.Log(string.Format("age {0}",documentDictionary["age"]));
                Debug.Log(string.Format("age: {0}", document.Id));
                
                Debug.Log(string.Format("introduction: {1}", document.Id));
                Debug.Log(string.Format("mannerScore: {2}", document.Id));
                Debug.Log(string.Format("mbti: {3}", document.Id));
                Debug.Log(string.Format("name: {4}", document.Id));
                Debug.Log(string.Format("sex: {5}", document.Id));
                Debug.Log(string.Format("state: {6}", document.Id));
                Debug.Log(string.Format("todayQ: {7}", document.Id));
                Debug.Log(string.Format("uid: {8}", document.Id));
            }
        });
        
    }

    public void LoadData()
    {
        DocumentReference docRef = db.Collection("Users").Document("Person");
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                
                Dictionary<string, object> city = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in city)
                {
                    Debug.Log(string.Format("{0}: {1}", pair.Key, pair.Value));
                }
            }
            else
            {
                Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
    }
    
    public void iID() //inputID 함수 호출
    {
        inputID(ID.text);
    }

    public void iPW() //inputPW 함수 호출
    {
        inputPW(PW.text);
    }

    public void inputID(string text) //아이디 인풋 데이터 받아오기
    {
        if (text.Trim() == "")
        {
            Debug.Log("아이디를 입력하지 않았습니다.");
            return;
        }
        else
        {
          
            Debug.Log("유저아이디: " + ID.text);
        }
        
    }
    public void inputPW (string text) //비밀번호 인풋 데이터 받아오기
    {
        if(text.Trim() == "")
        {
            Debug.Log("비밀번호를 입력하지 않았습니다.");
            return;
        }
        else
        {
            Debug.Log("유저비밀번호: " + PW.text); 
        }
        
    }

    public void makeUserData() //새로운유저 DB 생성
    {
        if (ID.text.Trim() != "" && PW.text.Trim() != "")
        {
            Debug.Log("인풋필드 입력받기 성공");
            useridDB();
            //ImgDB();
            profileDB();
            keywordDB();
            mannerDB();
            sendRequestDB();
            reportDB();

        }
    }
    
    public void useridDB() //새로운유저 기본정보 생성
    {
        DocumentReference userRef = db.Collection("Users").Document(ID.text);
        userRef.SetAsync(new Dictionary<string, object>()
        {
            {"state", null },
            {"todayQ" , null  },
            {"pw", PW.text },
            {"uid", ID.text },
            {"ispass", null },
            {"signUpDate", null },
            {"recentAccess", null },
            {"mbti", null }
        });

        //불러오기
        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> User = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in User)
                {
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                }
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
    }

    public void sendRequestDB() //받은신청, 보낸신청 DB
    {
        string docname = ID.text + "sentRequest"; //문서 id 지정
        DocumentReference userRef = db.Collection("Users").Document(ID.text).Collection("sentRequest").Document(docname); //기본정보DB 참조
        userRef.SetAsync(new Dictionary<string, object>() //생성
        {
            {"uid", null },
            {"isSend" , null  },
            {"matchingState", null },
            {"matchTime", null }

        });
    }
    public void chatDB() //채팅 DB 
    {

    }

    public void ImgDB() //사진 DB
    {

    }
    public void profileDB() //기본프로필 DB 생성 (닉네임,나이,성별,한줄소개,매너등급)
    {
        string docname = ID.text + "profile"; //문서 id 지정
        DocumentReference userRef = db.Collection("Users").Document(ID.text).Collection("profile").Document(docname); //기본정보DB 참조
        userRef.SetAsync(new Dictionary<string, object>() //생성
        {
            {"name", null },
            {"age" , null  },
            {"sex", null },
            {"introduction", null },
            {"mannerScore", null }
        });
    }
    public void keywordDB() //키워드 DB
    {
        string docname = ID.text + "keyword"; //문서 id 지정
        DocumentReference userRef = db.Collection("Users").Document(ID.text).Collection("keyword").Document(docname); //기본정보DB 참조
        userRef.SetAsync(new Dictionary<string, object>() //생성
        {
            {"kw1_name", null },
            {"kw1_description" , null  },
            {"kw1_ratio", null },
            {"kw2_name", null },
            {"kw2_description", null },
            {"kw2_ratio", null },
            {"kw3_name", null },
            {"kw3_description", null },
            {"kw3_ratio", null },
            {"kw4_name", null },
            {"kw4_description", null },
            {"kw4_ratio", null },
            {"kw5_name", null },
            {"kw5_description", null },
            {"kw5_ratio", null }

        });

    }
    public void reportDB() //신고 DB
    {
        string docname = ID.text + "report"; //문서 id 지정
        DocumentReference userRef = db.Collection("Users").Document(ID.text).Collection("report").Document(docname); //기본정보DB 참조
        userRef.SetAsync(new Dictionary<string, object>() //생성
        {
            {"uid", null },
            {"reason" , null }

        });
    }
    public void mbtiType() //모래알유형 DB
    {

    }

    public void mannerDB()
    {
        string docname = ID.text + "mannerRate"; //문서 id 지정
        DocumentReference userRef = db.Collection("Users").Document(ID.text).Collection("mannerRate").Document(docname); //기본정보DB 참조
        userRef.SetAsync(new Dictionary<string, object>() //생성
        {
            {"uid", null },
            {"score" , null }

        });
    }
}

