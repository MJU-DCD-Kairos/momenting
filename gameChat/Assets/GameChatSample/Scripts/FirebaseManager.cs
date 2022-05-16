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
    public static FirebaseFirestore db;

    public InputField ID;
    public InputField PW;
    public string uid;
    public string upw;

    public void Start()
    {
        //씬매니저 파괴 방지를 위한 코드
        DontDestroyOnLoad(this.gameObject);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            //var dependencyStatus = task.Result;
            if (task.Result == DependencyStatus.Available)
            {
                //app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log("DB 연결 성공");
                db = FirebaseFirestore.DefaultInstance; //Cloud Firestore 인스턴스 초기화
                //ReadData();
                //LoadData();
                makeUserData();
                make_uidDB();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
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


    void make_uidDB() //유저아이디와 토큰만 모아놓는 db 생성
    {
        Query allUidQuery = db.Collection("Users");
        allUidQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allUid = task.Result;
            foreach (DocumentSnapshot UsersDoc in allUid.Documents) //모든 문서id (uid) 불러오기 반복
            {
                Debug.Log(string.Format(UsersDoc.Id)); 
                
                //유저아이디만 모아놓는 컬렉션에 uid 문서 생성
                //문서가 있는지 확실치 않으므로 새 데이터를 기존 문서와 병합하는 방식 사용
                //추후 token에 카카오 로그인 시 발급받는 토큰을 저장하도록 수정해야함
                DocumentReference uidDoc = db.Collection("useridDB").Document(UsersDoc.Id);
                Dictionary<string, object> update = new Dictionary<string, object>
                {
                    {"uid", UsersDoc.Id },
                    {"token" , null }
                };
                uidDoc.SetAsync(update, SetOptions.MergeAll);
            }
        });
    }
    public void Udata() //유저데이터 로컬 저장
    {
        //PlayerPrefs.SetString("name", );
        //PlayerPrefs.SetString("sex", );
        PlayerPrefs.SetString("uid", ID.text);
        PlayerPrefs.Save();

    }

    public void Signin()
    {
        Debug.Log("버튼 누름");
        Udata();
        Debug.Log(PlayerPrefs.GetString("uid"));

        /*
        if (ID.text.Trim() != "" && PW.text.Trim() != "")
        {
            Query allUidQuery = db.Collection("useridDB").WhereEqualTo("uid",ID.text); //uid가 입력한 아이디와 일치하는 문서 반환
            Debug.Log(allUidQuery.GetType());
            if (allUidQuery != null)
            {
                Debug.Log("로그인성공");
            }
            else
            {
                Debug.Log("일치하는 아이디가 없습니다.");
            }
            /*
            allUidQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot allUid = task.Result;
                foreach (DocumentSnapshot UsersDoc in allUid.Documents) //모든 문서id (uid) 불러오기 반복
                {
                    Debug.Log(string.Format(UsersDoc.Id));

                    //유저아이디만 모아놓는 컬렉션에 uid 문서 생성
                    //문서가 있는지 확실치 않으므로 새 데이터를 기존 문서와 병합하는 방식 사용
                    //추후 token에 카카오 로그인 시 발급받는 토큰을 저장하도록 수정해야함
                    DocumentReference uidDoc = db.Collection("useridDB").Document(UsersDoc.Id);
                    Dictionary<string, object> update = new Dictionary<string, object>
                {
                    {"uid", UsersDoc.Id },
                    {"token" , null }
                };
                    uidDoc.SetAsync(update, SetOptions.MergeAll);
                }
            });


            CollectionReference user = db.Collection("Users"); //Users 컬렉션 참조
            Query uidquery = user.WhereEqualTo("uid", ID.text); //uid가 입력한 ID와 일치하는 쿼리 찾기
            uidquery.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
            {
                foreach (DocumentSnapshot docSnapshot in querySnapshotTask.Result.Documents)
                {
                    Debug.Log(string.Format("유저아이디", docSnapshot.Id));
                    if (docSnapshot.Id == ID.text)
                    {
                        Debug.Log("일치하는 아이디가 있습니다.");
                    }
                    else
                    {
                        Debug.Log("일치하는 아이디가 없습니다!");
                    }
                }
            });*/

            /*
            uidquery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot snapshot = task.Result;
                Debug.Log(String.Format("Document data for {0} document:", snapshot));
                if (string.Format(snapshot) = ID.text.Trim())
            });
            
            
        }*/
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

