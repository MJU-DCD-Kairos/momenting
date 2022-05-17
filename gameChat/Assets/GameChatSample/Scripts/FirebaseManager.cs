using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseFirestore db;

    public InputField Name;
    public InputField Age;//db 생성 테스트를 위한 아이디 인풋필드
    public InputField Sex;
    public InputField Token;

    public string token;
    public string myname;
    public string sex;
    public int age;
    public string mbti;
    public int mannerLevel;
    public bool ispass;
    public bool isActive;

    public void Start()
    {
        //씬매니저 파괴 방지를 위한 코드
        DontDestroyOnLoad(this.gameObject);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            //var dependencyStatus = task.Result;
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("DB 연결 성공");
                db = FirebaseFirestore.DefaultInstance; //Cloud Firestore 인스턴스 초기화
                
                //makeUserData();
                //makeTokenDB();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    public void myName() { inputName(Name.text); }//inputID 함수 호출
    public void myage() { inputAge(Age.text); }
    public void mySex() { inputSex(Sex.text); }
    public void myToken() { inputToken(Token.text); }
    public void inputName(string text) //아이디 인풋 데이터 받아오기
    {
        if (text.Trim() == "")
        {
            Debug.Log("아이디를 입력하지 않았습니다.");
            return;
        }
        else
        {
            Debug.Log("유저아이디: " + Name.text);
        }
        
    }
    public void inputAge(string text) //아이디 인풋 데이터 받아오기
    {
        if (text.Trim() == "")
        {
            Debug.Log("아이디를 입력하지 않았습니다.");
            return;
        }
        else
        {
            Debug.Log("유저나이: " + Age.text);
        }

    }
    public void inputSex(string text) //아이디 인풋 데이터 받아오기
    {
        if (text.Trim() == "")
        {
            Debug.Log("아이디를 입력하지 않았습니다.");
            return;
        }
        else
        {
            Debug.Log("유저성별: " + Sex.text);
        }

    }
    public void inputToken(string text) //아이디 인풋 데이터 받아오기
    {
        if (text.Trim() == "")
        {
            Debug.Log("아이디를 입력하지 않았습니다.");
            return;
        }
        else
        {
            Debug.Log("유저토큰: " + Token.text);
        }

    }
    public async void Onclick_LoadData()
    {//매칭버튼 누르기 전에 인풋필드에 닉네임 입력 먼저 하기 (닉네임으로 조회)
        myname = Name.text;
        await LoadData(); //유저 정보 불러오기
        LocalData(); //유저 정보 로컬 저장

    }

    private async Task LoadData() //파이어스토어DB에서 유저정보 불러오는 함수
    {
        Query userRef = db.Collection("userInfo").WhereEqualTo("name", myname); //입력한 닉네임과 일치하는 쿼리 찾아서 참조
        QuerySnapshot snapshot = await userRef.GetSnapshotAsync();
        foreach (DocumentSnapshot doc in snapshot.Documents)
        {
            Dictionary<string, object> docDictionary = doc.ToDictionary();
            myname = docDictionary["name"] as string;
            sex = docDictionary["sex"] as string;
        }
        Debug.Log("닉네임 : " + myname);
        Debug.Log("성별 : " + sex);
    }
    /*
    async Task LocalData() //유저데이터 로컬 저장
    {
        token = "qpiubf92qqq8g2fco6qo943gafkugbskvubjhgqo34"; //토큰문자열 저장하는 변수

        PlayerPrefs.SetString("token", token); //토큰
        PlayerPrefs.SetString("name", myname); //이름
        PlayerPrefs.SetString("sex", sex); //성별
        PlayerPrefs.SetInt("age", age); //나이
        PlayerPrefs.SetString("mbti", mbti); //모래알유형
        PlayerPrefs.SetInt("mannerLevel", mannerLevel); //매너등급
        //PlayerPrefs.SetString("isActive", "false");
        PlayerPrefs.Save();
    }
    */
    void LocalData() //유저데이터 로컬 저장
    {
        token = "qpiubf92qqq8g2fco6qo943gafkugbskvubjhgqo34"; //토큰문자열 저장하는 변수

        PlayerPrefs.SetString("token", token); //토큰
        PlayerPrefs.SetString("name", myname); //이름
        PlayerPrefs.SetString("sex", sex); //성별
        PlayerPrefs.SetInt("age", age); //나이
        PlayerPrefs.SetString("mbti", mbti); //모래알유형
        PlayerPrefs.SetInt("mannerLevel", mannerLevel); //매너등급
        //PlayerPrefs.SetString("isActive", "false");
        PlayerPrefs.Save();
    }
    void makeTokenDB() //유저아이디와 토큰만 모아놓는 db 생성
    {
        Query allUTokenQuery = db.Collection("userToken");
        allUTokenQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allToken = task.Result;
            foreach (DocumentSnapshot UserToken in allToken.Documents) //모든 문서id (uid) 불러오기 반복
            {
                Debug.Log(string.Format(UserToken.Id)); 
                
                //문서가 있는지 확실치 않으므로 새 데이터를 기존 문서와 병합하는 방식 사용
                //추후 token에 카카오 로그인 시 발급받는 토큰을 저장하도록 수정해야함
                DocumentReference uidDoc = db.Collection("useridDB").Document(UserToken.Id);
                Dictionary<string, object> update = new Dictionary<string, object>
                {
                    {"name", UserToken.Id },
                    {"token" , null }
                };
                uidDoc.SetAsync(update, SetOptions.MergeAll);
            }
        });
    }

    public void Signin()
    {
        if(Name.text.Trim() != "" && Name.text.Trim() == myname) //토큰으로 바꿔야 됨
        {
            Debug.Log("로그인완료!");

        }

        /*
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
        if (Name.text.Trim() != "" && Name.text.Trim() == "")
        {
            Debug.Log("인풋필드 입력받기 성공");
            //makeUserInfoDB();
            //ImgDB();
            //profileDB();
            //keywordDB();
            //mannerDB();
            //sendRequestDB();
            //reportDB();

        }
    }

    public void makeUserInfoDB() //유저DB 생성 (userInfo)
    {
        myname = Name.text;
        age = int.Parse(Age.text);
        sex = Sex.text;
        token = Token.text;
        ispass = true;
        isActive = false;

        Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "name", myname }, //닉네임
            { "age" , age  }, //나이
            { "sex", sex }, //성별
            { "ispass", ispass }, //프로필통과여부 (기본 false 상태)
            { "mbti", null }, //모래알유형 
            { "isActive", isActive }, //매칭가능여부
            { "recentAccess", null }, //최근접속시간
            { "signupDate", null }, //가입일 (ispass가 true가 되면 기록
            {"token", token }, //토큰
            { "mannerLevel", 1 } //매너등급 (기본 1등급으로 시작)
        };

        db.Collection("userInfo").AddAsync(user).ContinueWithOnMainThread(task =>
        {
            DocumentReference addUserInfo = task.Result;
            Debug.Log(string.Format("추가된 문서 ID: {0}.", addUserInfo.Id));
        });

    }

    /*
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
    */
}

