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
    public FirebaseFirestore db;

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
    public bool isInMatchingDB;

    public void Start()
    {
        //씬매니저 파괴 방지를 위한 코드
        DontDestroyOnLoad(this.gameObject);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            //var dependencyStatus = task.Result;
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("파이어스토어 DB 연결 성공");
                db = FirebaseFirestore.DefaultInstance; //Cloud Firestore 인스턴스 초기화
                
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });

        //isMatchToken(); //토큰 정보 있는지 확인
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
            Debug.Log("나이를 입력하지 않았습니다.");
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
            Debug.Log("성별을 입력하지 않았습니다.");
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
            Debug.Log(" 토큰 ");
            return;
        }
        else
        {
            Debug.Log("유저토큰: " + Token.text);
        }

    }
    public async void isMatch_LoadData()
    {//매칭버튼 누르기 전에 인풋필드에 닉네임 입력 먼저 하기 (닉네임으로 조회)
        myname = Name.text;
        await LoadData(); //유저 정보 불러오기
        LocalData(); //유저 정보 로컬 저장
        isMatchToken();
    }

    private async Task LoadData() //파이어스토어DB에서 유저정보 불러오는 함수
    {
        Query userRef = db.Collection("userInfo").WhereEqualTo("name", myname); //입력한 닉네임과 일치하는 쿼리 찾아서 참조
        
        if(userRef != null) //유저 정보가 있으면
        {
            QuerySnapshot snapshot = await userRef.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                Dictionary<string, object> docDictionary = doc.ToDictionary();
                //myname = docDictionary["name"] as string;
                sex = docDictionary["sex"] as string;
            }
            Debug.Log("닉네임 : " + myname);
            Debug.Log("성별 : " + sex);
        }
        
    }
   
    void LocalData() //유저데이터 로컬 저장 (가입단에 들어감)
    {
        PlayerPrefs.SetString("name", myname); //이름
        PlayerPrefs.SetString("sex", sex); //성별
        PlayerPrefs.SetInt("age", age); //나이
        PlayerPrefs.SetString("mbti", mbti); //모래알유형
        PlayerPrefs.SetInt("mannerLevel", mannerLevel); //매너등급
        isInMatchingDB = false;
        PlayerPrefs.SetString("isInMatchingDB", isInMatchingDB.ToString());
        
        PlayerPrefs.Save();
    }
    
    async void isMatchToken() //토큰 DB에서 정보 확인 (가입단에 들어감) 
    {
        //Query allTokenQuery = null;

        Query allTokenQuery = db.Collection("userToken").WhereEqualTo("name", PlayerPrefs.GetString("name")); //닉네임으로 일치하는 쿼리 찾아서 참조
        await allTokenQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allToken = task.Result;
            foreach (DocumentSnapshot token in allToken.Documents)
            {
                Dictionary<string, object> tokenDictionary = token.ToDictionary();

                if (tokenDictionary["token"] as string != null) //토큰 정보가 있으면
                {
                    Debug.Log(string.Format(tokenDictionary["name"] as string));
                    Debug.Log(string.Format(tokenDictionary["token"] as string));
                }

                else //토큰 정보 없으면 토큰DB에 새로 추가
                {
                    CollectionReference tokenColl = db.Collection("userToken"); //토큰 콜렉션 참조
                    Dictionary<string, object> update = new Dictionary<string, object> //새로운 딕셔너리 생성
                    {
                        {"name", PlayerPrefs.GetString("name") },
                        {"token" , PlayerPrefs.GetString("token") }
                    };
                    tokenColl.AddAsync(update);
                }
                

                /*
                if (allToken != null) //토큰 정보가 있으면
                {
                    Dictionary<string, object> tokenDictionary = token.ToDictionary();

                    Debug.Log(string.Format(tokenDictionary["name"] as string));
                    Debug.Log(string.Format(tokenDictionary["token"] as string));
                }

                else //토큰 정보 없으면 토큰DB에 새로 추가
                {
                    CollectionReference tokenColl = db.Collection("userToken"); //토큰 콜렉션 참조
                    Dictionary<string, object> update = new Dictionary<string, object> //새로운 딕셔너리 생성
                    {
                        {"name", PlayerPrefs.GetString("name") },
                        {"token" , PlayerPrefs.GetString("token") }
                    };
                    tokenColl.AddAsync(update);
                }*/
            }

        });
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

        db.Collection("userInfo").Document(myname).SetAsync(user).ContinueWithOnMainThread(task =>
        {
            //Debug.Log(string.Format("추가된 문서 ID: {0}.", addUserInfo.Id));
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

