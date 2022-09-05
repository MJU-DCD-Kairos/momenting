using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.UI;   
using System.Threading;
using System.Threading.Tasks;
using groupchatManager;



namespace FireStoreScript {
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseFirestore db;

        [SerializeField]
        groupchatSceneManager gsManager;

        public InputField Name;
        public InputField Age;//db 생성 테스트를 위한 아이디 인풋필드
        public InputField Sex;
        public InputField Token;
        public InputField Introduction;
        public Dropdown SSex;
        public Dropdown AAge;
        public Dropdown MMonth;
        public Dropdown DDay;
        public GameObject DoubleCheckDim;
        public GameObject HText;
        public GameObject HTextError;
        public GameObject FinishChecker;
        public GameObject DoubleEnBtn;
        public GameObject NextBtn;
        public GameObject ErrorIndi;
        public GameObject thiss;
        public string GAdd;

        [Header("Get User Data")]
        public static string GCN;
        public static string myintroduction;
        public string token;
        public static string myname;
        public static int sex;

        //오늘의 질문을 DB에서 불러올 전역변수 선언부 ( 현진 추가 )
        public static int myTqAnswer;
        public static int myLastTqIndex;

        public static string age;
        public static string mbti;
        public static string ispass;
        public int mannerLevel;

        //오늘의 질문 번호 전역변수
        public static int todayQIndex;


        [Header("Get Else Data")]

        public static string Elseintroduction;
        public static string ElseName;
        public static int ElseSex;
        public static string ElseAge;
        public static string ElseMbti;


        public bool isActive;
        public bool isInMatchingDB;


        //키워드를 저장하기 위한 선언부
        //키워드 카테고리별 리스트
        //public List<string> tendencyKW = new List<string>();
        //public List<string> interestKW = new List<string>();
        //public List<string> lifestyleKW = new List<string>();
        
        //리스트를 색상 핵사코드값으로 저장할 딕셔너리 생성
        public Dictionary<string, string> KWdict = new Dictionary<string, string>();

        public static List<object> KWList = new List<object>();

        public enum fbRef { userInfo, matchingRoom, report, userToken, keywords, chatRoom, images, mannerRate }

        public void Awake()
        {
            //씬매니저 파괴 방지를 위한 코드
            DontDestroyOnLoad(this.gameObject);

        }
        public void OnEnable()
        {



        }
        public void SetActivee()
        {
            thiss.SetActive(true);

        }
        public void Start()
        {
            //GCN = "";
            PlayerPrefs.SetString("GCName","츄아");
            GCN = PlayerPrefs.GetString("GCName");
            Debug.Log("플레이어프랩스에서 받아온 GCN: " + GCN);
            //isMatchToken(); //토큰 정보 있는지 확인
            //Debug.Log("유저닉네임 : " + PlayerPrefs.GetString("name"));
            //Debug.Log("유저성별 : " + PlayerPrefs.GetString("sex"));

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
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
            
            Invoke("LoadData", 0.3f);
            Invoke("LoadKW", 0.5f);


            //오늘의 질문 상태 업데이트 함수 호출(현진 추가)
            Invoke("todayQupdate", 0.5f);

        }

        public void myName() { inputName(Name.text); }//inputID 함수 호출
        public void mySex() { inputSex(Sex.text); }
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

                myname = Name.text;
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
                // Debug.Log("유저성별: " + Sex.text);
                //sex = Sex.text;
            }

        }
        public async void isMatch_LoadData()
        {
            myname = Name.text;
            await LoadData(); //유저 정보 불러오기
            LocalData(); //유저 정보 로컬 저장
            //await isMatchToken();
        }

        public async Task LoadData() //파이어스토어DB에서 유저정보 불러오는 함수
        {
            Query userRef = db.Collection("userInfo").WhereEqualTo("name", GCN); //입력한 닉네임과 일치하는 쿼리 찾아서 참조

            if (userRef != null) //유저 정보가 있으면
            {
                Debug.Log("유저가있다");
                await userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Dictionary<string, object> docDictionary = doc.ToDictionary();
                        ispass = docDictionary["ispass"] as string;
                        sex = int.Parse(docDictionary["sex"].ToString());
                        age = docDictionary["age"] as string;
                        myintroduction = docDictionary["Introduction"] as string;

                        //오늘의 질문 답변 정보 불러오기 ( 현진 추가 )
                        myTqAnswer = int.Parse(docDictionary["TqAnswer"].ToString());
                        myLastTqIndex = int.Parse(docDictionary["lastTqIndex"].ToString());


                        //mbti = docDictionary["mbti"] as string;
                        //mannerLevel = int.Parse(docDictionary["mannerLevel"] as string);
                        Debug.Log(myname + "의 성별 불러오기 성공 -> " + docDictionary["sex"] as string);
                        //token = docDictionary["token"] as string;
                        //return;
                    }
                    Debug.Log("닉네임 : " + GCN);
                    Debug.Log("ispass :" + ispass);
                    Debug.Log("성별 : " + sex);
                    Debug.Log("나이 : " + age);
                    Debug.Log("한줄소개 : " + myintroduction);

                    //오늘의 질문 답변 정보 불러오기 디버깅 로그( 현진 추가 )
                    Debug.Log("TQ 답변 상태 : " + myTqAnswer);
                    Debug.Log("마지막 답변 TQ인덱스 : " + myLastTqIndex);
                });

            }
            else
            {
                Debug.Log("유저정보 없음");
            }
        }
        public static async Task ElseData(string userN) //파이어스토어DB에서 유저정보 불러오는 함수
        {
            Query userRef = db.Collection("userInfo").WhereEqualTo("name", userN); //입력한 닉네임과 일치하는 쿼리 찾아서 참조

            if (userRef != null) //유저 정보가 있으면
            {

        Debug.Log("다른사람찾음");
                await userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Dictionary<string, object> docDictionary = doc.ToDictionary();
                ElseSex = int.Parse(docDictionary["sex"].ToString());
                ElseAge = docDictionary["age"] as string;
                Elseintroduction = docDictionary["Introduction"] as string;
                        ElseMbti = docDictionary["mbti"] as string;
                        //mannerLevel = int.Parse(docDictionary["mannerLevel"] as string);
                        Debug.Log(myname + "의 성별 불러오기 성공 -> " + docDictionary["sex"] as string);
                        //token = docDictionary["token"] as string;
                        //return;
                    }
                 
                    

                });
                
            }
            else
            {
                Debug.Log("유저정보 없음");
            }
        }
        void LocalData() //유저데이터 로컬 저장 (가입단에 들어감)
        {
            Debug.Log("LocalData 함수 실행됨");

            PlayerPrefs.SetString("name", myname); //이름
            //PlayerPrefs.SetString("sex", sex); //성별
            //PlayerPrefs.SetInt("age", age); //나이
            //PlayerPrefs.SetString("mbti", mbti); //모래알유형
            PlayerPrefs.SetInt("mannerLevel", mannerLevel); //매너등급

            PlayerPrefs.Save();

        }

        private async Task isMatchToken() //토큰 DB에서 정보 확인 (가입단에 들어감) 
        {
            CollectionReference userTokenColl = db.Collection("userToken");
            Query allTokenQuery = userTokenColl.WhereEqualTo("name", PlayerPrefs.GetString("name")); //닉네임으로 일치하는 쿼리 찾아서 참조

            if (allTokenQuery != null)
            {
                await allTokenQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot allToken = task.Result;
                    foreach (DocumentSnapshot token in allToken.Documents)
                    {
                        Dictionary<string, object> tokenDictionary = token.ToDictionary();
                        Debug.Log(string.Format("유저" + tokenDictionary["name"] as string + "의 토큰: " + tokenDictionary["token"] as string));

                        Debug.Log("사용자" + myname + "의 토큰 일치 확인됨");
                    }

                });
            }
            else //토큰 정보 없으면 토큰DB에 새로 추가
            {
                Dictionary<string, object> newTokenDic = new Dictionary<string, object>
            {
                {"name" , myname },
                {"token" , PlayerPrefs.GetString("token") }
            };

                DocumentReference newTokenDoc = userTokenColl.Document();
                await newTokenDoc.SetAsync(newTokenDic);

                Debug.Log("토큰 DB 문서 생성됨");
            };

            Debug.Log("isMatchToken 함수 실행됨");
        }
        public string DCheck;
        public async void DoubleCheck() //토큰 DB에서 정보 확인 (가입단에 들어감) 
        {
            Query nameQuery = db.Collection("userInfo").WhereEqualTo("name", Name.text);
            QuerySnapshot snapshot = await nameQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                Dictionary<string, object> docDictionary = doc.ToDictionary();

                DCheck = docDictionary["name"] as string;


            }
            if (DCheck == Name.text)
            {
                Debug.Log("중복띠");
                HText.SetActive(false);
                HTextError.SetActive(true);
                ErrorIndi.SetActive(true);
            }
            else
            {
                Debug.Log("통과띠");
                DoubleCheckDim.SetActive(true);
                FinishChecker.SetActive(true);
                DoubleEnBtn.SetActive(false);
                NextBtn.SetActive(true);

            }

        }

        public async void SaveKW()
        {
            for (int i = 0; i < getKeywordList.saveKWlist.Count; i++)
            {
                KWList.Add(getKeywordList.saveKWlist[i]);
            }
            await db.Collection("userInfo").Document(GCN).UpdateAsync("Keyword", KWList);
            Debug.LogError("db에 키워드 저장 완료!");
            //var awaiter = delete_duplicatedKW().GetAwaiter();
            //awaiter.OnCompleted(() =>
            //{
            //    for (int i = 0; i < getKeywordList.saveKWlist.Count; i++)
            //    {
            //        KWList.Add(getKeywordList.saveKWlist[i]);
            //    }
            //    for (int j = 0; j < KWList.Count; j++)
            //    {
            //        Debug.Log("SAVE 새로 선택한 키워드: " + KWList[j]);
            //    }
            //    db.Collection("userInfo").Document(GCN).UpdateAsync("Keyword", KWList);
            //    Debug.LogError("db에 키워드 저장 완료!");
            //});
            

        }
        
        //public async Task delete_duplicatedKW()
        //{
        //    await db.Collection("userInfo").Document(GCN).UpdateAsync("Keyword", FieldValue.Delete);
        //    Debug.LogError("DB키워드 삭제 완료!");
        //}

        public async void LoadKW()
        {
            DocumentReference KWRef = db.Collection("userInfo").Document(GCN);
            await KWRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, object> Keywords = snapshot.ToDictionary();

                    KWList = (List<object>)Keywords["Keyword"];
                }
                else
                {
                    Debug.Log("저장된키워드없음");
                }
            });
            Debug.LogError("DB 키워드 불러오기 완료!");
        }

        public static string MbtiType;
        public void mbtiData()
        {
            //MbtiType = PlayerPrefs.GetString("MBTIResult");
            MbtiType = TypeTestManager.MbtiType;

            Dictionary<string, object> user = new Dictionary<string, object>()
        {

            { "mbti", MbtiType }, //모래알유형 
            
        };

            StartCoroutine(delay(user));
        }

        
        IEnumerator delay(Dictionary<string, object> user)
        {
            yield return new WaitForSeconds(1f);
            db.Collection("userInfo").Document(GCN).SetAsync(user, SetOptions.MergeAll);
        }


        

        public string newAge;
        public void makeUserInfoDB() //유저DB 생성 (userInfo)
        {
            myname = Name.text;
            myintroduction = Introduction.text;

            if (SSex.options[SSex.value].text == ("남자"))
            {
                int sex2 = 1;
                sex = sex2;
            }
            else
            {
                int sex2 = 2;
                sex = sex2;
            }
            //string sex2 = SSex.options[SSex.value].text;
            string age2 = AAge.options[AAge.value].text;
            string mon = MMonth.options[MMonth.value].text;
            string day = DDay.options[DDay.value].text;

            age = age2;
            //token = Token.text;
            ispass = "false";
            //isActive = false;

            //연령 구하기
            newAge = DateTime.Now.ToString("yyyy");
            Debug.Log(DateTime.Now.ToString("yyyy"));

            newAge = DateTime.Now.ToString("yyyy");
            Debug.Log(DateTime.Now.ToString("yyyy"));

            newAge = (int.Parse(newAge.ToString()) - int.Parse(age2)).ToString();
            Debug.Log("newAge1 :" + newAge);
            if (int.Parse(DateTime.Now.ToString("MM")) == int.Parse(mon))
            {
                if (int.Parse(DateTime.Now.ToString("dd")) < int.Parse(day))
                    age = newAge = (int.Parse(newAge) - 1).ToString();

            }
            else if (int.Parse(DateTime.Now.ToString("MM")) < int.Parse(mon))
            {
                age = newAge = (int.Parse(newAge) - 1).ToString();
            }
            else
            {
                age = newAge;
            }

            GAdd = PlayerPrefs.GetString("GAddress");



            Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "Introduction", myintroduction},//한줄소개
            { "name", myname }, //닉네임
            { "age" , age  }, //나이
            { "sex", sex }, //성별
            { "ispass", ispass }, //프로필통과여부 (기본 false 상태)
            { "mbti", null }, //모래알유형 
            { "recentAccess", null }, //최근접속시간
            { "signupDate", null }, //가입일 (ispass가 true가 되면 기록
            { "mannerLevel", 1 }, //매너등급 (기본 1등급으로 시작)
            { "GmailAddress", GAdd},

            { "TqAnswer", 0}, //오늘의 질문 답변 여부 기본 0(답변 안함 상태)로 지정
            { "lastTqIndex", todayQIndex} //답변해야하는 인덱스를 현재 인덱스로 지정
            //{ "keyWord", KWdict.ToDictionary}
        };


            db.Collection("userInfo").Document(myname).SetAsync(user);
            Dictionary<string, object> user2 = new Dictionary<string, object> {
            { "name", myname}
                };


      
            db.Collection("report").Document(myname).SetAsync(user2);
            PlayerPrefs.SetString("GCName", myname);

            


        }

        //today질문 전체 유저 동기화를 위해 DB정보를 읽어오는 함수
        /*
        - FirebaseManager스크립트에 스타트하면서 오늘의 질문 업데이트 시점을 로드하는 함수를 추가 작성
        - FirebaseManager스크립트에서 업데이트 시점을 로드하는 함수를 실행한 후 오늘 접속 날짜와 비교하여 상태표시(bull) 테스트
        - bull값에 따라 기존 인덱스 or 인덱스 + 1을 변수에 저장
        - bull값에 따라 인덱스+1과 업데이트 타임스탬프를 DB에 업로드
         */
        //현진작성

        public async Task todayQupdate() //파이어스토어DB에서 오늘의 질문 업데이트 일자를 가져와 현재 접속일자와 비교, 인덱스 번호를 업데이트하는 함수
        {
            DocumentReference TQRef = db.Collection("userInfo").Document("todayQ"); //todayQ도큐먼트 내용을 가져옴

            if (TQRef != null) //도큐먼트 내용 있으면
            {

                await TQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    DocumentSnapshot snapshot = task.Result;
                    
                    Dictionary<string, object> docDictionary = snapshot.ToDictionary();
                    //Debug.Log("DB타임스트링: " + docDictionary["updateTime"].ToString());
                    //Debug.Log("DB타임 데이트타임: "+DateTime.Parse(docDictionary["updateTime"].ToString()));
                    //Debug.Log("DB인덱스: " + int.Parse(docDictionary["qIndex"].ToString()));
                    //Debug.Log("내 타임스탬프" + DateTime.Now.ToString());
                    
                    bool isEqualDay = true;
                    if(DateTime.Parse(docDictionary["updateTime"].ToString()).Date >= DateTime.Now.Date)
                    {
                        isEqualDay = true;
                        todayQIndex = int.Parse(docDictionary["qIndex"].ToString());
                        //Debug.Log("업데이트 필요없음, 노출인덱스: " + todayQIndex);
                        
                        
                    }
                    else
                    {
                        isEqualDay = false;
                        todayQIndex = int.Parse(docDictionary["qIndex"].ToString())+1;
                        //Debug.Log("업데이트 필요함, 노출인덱스: " + todayQIndex);

                        Dictionary<string, object> newTQInfo = new Dictionary<string, object>
                        {
                            //오늘의 질문 인덱스
                            { "qIndex", todayQIndex},
                            //업데이트 날짜
                            { "updateTime", DateTime.Now.Date.ToString() }
                        };
                        db.Collection("userInfo").Document("todayQ").SetAsync(newTQInfo, SetOptions.MergeAll);

                    }

                    //Debug.Log("DB타임스트링: " + docDictionary["updateTime"].ToString());
                    if (todayQIndex > myLastTqIndex)
                    {
                        Dictionary<string, object> TqAnswerState = new Dictionary<string, object>
                        {
                            //오늘의 질문 답변 상태 업데이트, 마지막 답변한 질문의 인덱스와 비교하여 전날 답변한 상태의 값을 초기화 시킴
                            { "TqAnswer", 0}
                        };
                        //상태를 DB의 TqAnswer에 덮어쓰기함
                        db.Collection("userInfo").Document(GCN).SetAsync(TqAnswerState, SetOptions.MergeAll);
                        
                        //전역변수도 업데이트
                        myTqAnswer = 0;
                    }

                });

            }
            else
            {
                Debug.Log("도큐먼트 정보없음");
            }
        }

        //답변 1을 눌렀을 때 유저의 TqAnswer을 1로 변경함
        public static void setTqAnswer1()
        {
            Dictionary<string, object> TqAnswerState = new Dictionary<string, object>
                        {
                            //오늘의 질문 답변 상태 업데이트, 마지막 답변한 질문의 인덱스와 비교하여 전날 답변한 상태의 값을 초기화 시킴
                            { "TqAnswer", 1},
                            { "lastTqIndex", todayQIndex}
                        };
            //상태를 DB의 TqAnswer에 덮어쓰기함
            Debug.Log(GCN);
            db.Collection("userInfo").Document(GCN).SetAsync(TqAnswerState, SetOptions.MergeAll);

            //전역변수도 업데이트
            myTqAnswer = 1;
        }

        //답변 2을 눌렀을 때 유저의 TqAnswer을 2로 변경함
        public static void setTqAnswer2()
        {
            Dictionary<string, object> TqAnswerState = new Dictionary<string, object>
                        {
                            //오늘의 질문 답변 상태 업데이트, 마지막 답변한 질문의 인덱스와 비교하여 전날 답변한 상태의 값을 초기화 시킴
                            { "TqAnswer", 2},
                            { "lastTqIndex", todayQIndex}
                        };
            //상태를 DB의 TqAnswer에 덮어쓰기함
            db.Collection("userInfo").Document(GCN).SetAsync(TqAnswerState, SetOptions.MergeAll);

            //전역변수도 업데이트
            myTqAnswer = 2;
        }


    }
    

       

        //public static bool isCheckedCRname = false;
        //public static async Task<bool> CRnameDoubleCheck(object name)
        //{
        //    DocumentReference docRef = db.Collection("gameChatRoom").Document("chatRoomNameDoubleCheck");
        //    DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        //    if (snapshot.Exists)
        //    {
        //        int bcount = 0;
        //        Dictionary<string, object> crArray = snapshot.ToDictionary();
        //        foreach (KeyValuePair<string, object> pair in crArray)
        //        {
        //            Debug.Log(pair.Key);
        //            Debug.Log(pair.Value);
        //            foreach (string crName in (List<string>)pair.Value)
        //            {
        //                Debug.LogError("foreach문 진입 확인 로그");
        //                if (crName.Equals(name.ToString()) == false)
        //                {
        //                    bcount += 0; //기존 이름과 같지 않음일때는 0을 더해줌
                            
        //                }
        //                else
        //                {
        //                    bcount += 1; //기존 이름과 같은 이름일때는 1을 더해줌
        //                    break;
        //                }

        //            }
        //            Debug.LogError("#####isCheckedCRname#####" + isCheckedCRname);
        //        }
        //        if (bcount > 0)
        //        {
        //            isCheckedCRname = false;
        //            Debug.LogError("#####false#####" + isCheckedCRname);
        //        }
        //        else
        //        {
        //            isCheckedCRname = true;
        //            Debug.LogError("#####true#####" + isCheckedCRname);
        //        }
        //        Debug.Log("카운트  " + bcount);
        //    }
        //    else
        //    {
        //        //Console.WriteLine("Document {0} does not exist!", snapshot.Id);
        //    }
        //    return isCheckedCRname;
        //}

        /*
        public string ChannelID;

        public static LoadData_Chat() //파이어스토어DB에서 유저정보 불러오는 함수
        {
            //해당하는 도큐먼트에서 channel ID를 반환하는 함수를 작성해야함
            //doc.TryGetValue<>(, )
        }*/

    
}



