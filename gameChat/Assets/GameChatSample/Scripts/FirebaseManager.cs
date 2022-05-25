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
using System;



namespace FireStoreScript {
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseFirestore db;

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
        public static string age;
        public string mbti;
        public static string ispass;
        public int mannerLevel;


        public bool isActive;
        public bool isInMatchingDB;


        //키워드를 저장하기 위한 선언부
        //키워드 카테고리별 리스트
        public List<string> tendencyKW = new List<string>();
        public List<string> interestKW = new List<string>();
        public List<string> lifestyleKW = new List<string>();

        //리스트를 색상 핵사코드값으로 저장할 딕셔너리 생성
        public Dictionary<string, string> KWdict = new Dictionary<string, string>();


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

            GCN = "";

            GCN = PlayerPrefs.GetString("GCName");
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




        //리스트에 저장된 선택 키워드를 카테고리 구분하여 리스트에 넣음
        public void KWcategory()
        {
            for (int i = 0; i < getKeywordList.saveKWlist.Count; i++)
            {
                if (getKeywordList.saveKWlist[i] == "상큼발랄" ||
                    getKeywordList.saveKWlist[i] == "32차원" ||
                    getKeywordList.saveKWlist[i] == "긍정맨" ||
                    getKeywordList.saveKWlist[i] == "핵인싸" ||
                    getKeywordList.saveKWlist[i] == "일잘러" ||
                    getKeywordList.saveKWlist[i] == "잠만보" ||
                    getKeywordList.saveKWlist[i] == "감성충만" ||
                    getKeywordList.saveKWlist[i] == "센스쟁이" ||
                    getKeywordList.saveKWlist[i] == "분위기메이커" ||
                    getKeywordList.saveKWlist[i] == "수다쟁이" ||
                    getKeywordList.saveKWlist[i] == "과제마스터" ||
                    getKeywordList.saveKWlist[i] == "유머러스" ||
                    getKeywordList.saveKWlist[i] == "박학다식" ||
                    getKeywordList.saveKWlist[i] == "인싸중에아싸" ||
                    getKeywordList.saveKWlist[i] == "아싸중에인싸")
                {
                    //성향 카테고리 #ff8550
                    tendencyKW.Add(getKeywordList.saveKWlist[i]);
                    //KWdict.Add(getKeywordList.saveKWlist[i], "#ff8550");
                }
                else if (getKeywordList.saveKWlist[i] == "얼리버드" ||
                    getKeywordList.saveKWlist[i] == "저녁형" ||
                    getKeywordList.saveKWlist[i] == "올빼미형" ||
                    getKeywordList.saveKWlist[i] == "프로집콕러" ||
                    getKeywordList.saveKWlist[i] == "프로외출러" ||
                    getKeywordList.saveKWlist[i] == "밥보다 잠" ||
                    getKeywordList.saveKWlist[i] == "하루다섯끼" ||
                    getKeywordList.saveKWlist[i] == "워라밸" ||
                    getKeywordList.saveKWlist[i] == "워커홀릭" ||
                    getKeywordList.saveKWlist[i] == "벼락치기"
                    )
                {
                    //관심 카테고리 #7043c0
                    interestKW.Add(getKeywordList.saveKWlist[i]);
                    //KWdict.Add(getKeywordList.saveKWlist[i], "#7043c0");
                }
                else
                {
                    //생활패턴 카테고리 #001130
                    lifestyleKW.Add(getKeywordList.saveKWlist[i]);
                    //KWdict.Add(getKeywordList.saveKWlist[i], "#001130");
                }
            }
            //for문 종료 후 해당 리스트들을 dict에 저장
            //KWdict.Add("#ff8550", tendencyKW);
            //KWdict.Add("#7043c0", interestKW);
            //KWdict.Add("#001130", lifestyleKW);
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


            ArrayList KWarray = new ArrayList();
            for (int i = 0; i < tendencyKW.Count; i++)
            {
                string kw = tendencyKW[i];
                KWarray.Add(kw);
            }

            DocumentReference docRef = db.Collection("usreInfo").Document(myname);
            Dictionary<string, object> docData = new Dictionary<string, object>
            {

            };
            docData.Add("tendencyKW", KWarray);
            db.Collection("userInfo").Document(myname).SetAsync(docData);


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
            //{ "keyWord", KWdict.ToDictionary}
        };

            
            //성향 딕셔너리 생성
            //Dictionary<string, object> KWdictArray1 = new Dictionary<string, object> {

            //};
            ////관심사 딕셔너리 생성
            //Dictionary<string, object> KWdictArray2 = new Dictionary<string, object> {
            //    };
            ////라이프스타일 딕셔너리 생성
            //Dictionary<string, object> KWdictArray3 = new Dictionary<string, object> {
            //};

            //Dictionary<string, object> KWdictForFS = new Dictionary<string, object> {
            //    //{ "#ff8550", KWdictArray1},
            //    //    { "#7043c0", KWdictArray2},
            //    //    { "#001130", KWdictArray3}
            //    { "#ff8550", new List<object>() { } },
            //    { "#7043c0", new List<object>() { } },
            //    { "#001130", new List<object>() { } }

            //};

            //KWdictForFS["keyWord"] = KWdictArray1;
            //db.Collection("userInfo").Document(myname).UpdateAsync("keyWord",FieldValue.ArrayUnion(KWdictForFS));
            //KWdictForFS["keyWord"] = KWdictArray2;
            //db.Collection("userInfo").Document(myname).UpdateAsync("keyWord", FieldValue.ArrayUnion(KWdictForFS));
            //KWdictForFS["keyWord"] = KWdictArray3;
            //db.Collection("userInfo").Document(myname).UpdateAsync("keyWord", FieldValue.ArrayUnion(KWdictForFS));

            Dictionary<string, object> user2 = new Dictionary<string, object> {
            { "name", myname}
                };


      
            db.Collection("report").Document(myname).SetAsync(user2);
            PlayerPrefs.SetString("GCName", myname);

            


        }
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


       

        public static bool isCheckedCRname = false;
        public static async Task<bool> CRnameDoubleCheck(object name)
        {
            DocumentReference docRef = db.Collection("gameChatRoom").Document("chatRoomNameDoubleCheck");
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                int bcount = 0;
                Dictionary<string, object> crArray = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in crArray)
                {
                    Debug.Log(pair.Key);
                    Debug.Log(pair.Value);
                    foreach (string crName in (List<string>)pair.Value)
                    {
                        Debug.LogError("foreach문 진입 확인 로그");
                        if (crName.Equals(name.ToString()) == false)
                        {
                            bcount += 0; //기존 이름과 같지 않음일때는 0을 더해줌
                            
                        }
                        else
                        {
                            bcount += 1; //기존 이름과 같은 이름일때는 1을 더해줌
                            break;
                        }

                    }
                    Debug.LogError("#####isCheckedCRname#####" + isCheckedCRname);
                }
                if (bcount > 0)
                {
                    isCheckedCRname = false;
                    Debug.LogError("#####false#####" + isCheckedCRname);
                }
                else
                {
                    isCheckedCRname = true;
                    Debug.LogError("#####true#####" + isCheckedCRname);
                }
                Debug.Log("카운트  " + bcount);
            }
            else
            {
                //Console.WriteLine("Document {0} does not exist!", snapshot.Id);
            }
            return isCheckedCRname;
        }

        /*
        public string ChannelID;

        public static LoadData_Chat() //파이어스토어DB에서 유저정보 불러오는 함수
        {
            //해당하는 도큐먼트에서 channel ID를 반환하는 함수를 작성해야함
            //doc.TryGetValue<>(, )
        }*/

    }
}



