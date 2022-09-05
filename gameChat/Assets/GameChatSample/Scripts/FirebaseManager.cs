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
        public InputField Age;//db ���� �׽�Ʈ�� ���� ���̵� ��ǲ�ʵ�
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

        //������ ������ DB���� �ҷ��� �������� ����� ( ���� �߰� )
        public static int myTqAnswer;
        public static int myLastTqIndex;

        public static string age;
        public static string mbti;
        public static string ispass;
        public int mannerLevel;

        //������ ���� ��ȣ ��������
        public static int todayQIndex;


        [Header("Get Else Data")]

        public static string Elseintroduction;
        public static string ElseName;
        public static int ElseSex;
        public static string ElseAge;
        public static string ElseMbti;


        public bool isActive;
        public bool isInMatchingDB;


        //Ű���带 �����ϱ� ���� �����
        //Ű���� ī�װ��� ����Ʈ
        //public List<string> tendencyKW = new List<string>();
        //public List<string> interestKW = new List<string>();
        //public List<string> lifestyleKW = new List<string>();
        
        //����Ʈ�� ���� �ٻ��ڵ尪���� ������ ��ųʸ� ����
        public Dictionary<string, string> KWdict = new Dictionary<string, string>();

        public static List<object> KWList = new List<object>();

        public enum fbRef { userInfo, matchingRoom, report, userToken, keywords, chatRoom, images, mannerRate }

        public void Awake()
        {
            //���Ŵ��� �ı� ������ ���� �ڵ�
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
            PlayerPrefs.SetString("GCName","���");
            GCN = PlayerPrefs.GetString("GCName");
            Debug.Log("�÷��̾����������� �޾ƿ� GCN: " + GCN);
            //isMatchToken(); //��ū ���� �ִ��� Ȯ��
            //Debug.Log("�����г��� : " + PlayerPrefs.GetString("name"));
            //Debug.Log("�������� : " + PlayerPrefs.GetString("sex"));

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (task.Result == DependencyStatus.Available)
                {
                    Debug.Log("���̾��� DB ���� ����");
                    db = FirebaseFirestore.DefaultInstance; //Cloud Firestore �ν��Ͻ� �ʱ�ȭ
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
                }
            }); 
            
            Invoke("LoadData", 0.3f);
            Invoke("LoadKW", 0.5f);


            //������ ���� ���� ������Ʈ �Լ� ȣ��(���� �߰�)
            Invoke("todayQupdate", 0.5f);

        }

        public void myName() { inputName(Name.text); }//inputID �Լ� ȣ��
        public void mySex() { inputSex(Sex.text); }
        public void inputName(string text) //���̵� ��ǲ ������ �޾ƿ���
        {
            if (text.Trim() == "")
            {
                Debug.Log("���̵� �Է����� �ʾҽ��ϴ�.");
                return;
            }
            else
            {
                Debug.Log("�������̵�: " + Name.text);

                myname = Name.text;
            }

        }
        public void inputSex(string text) //���̵� ��ǲ ������ �޾ƿ���
        {
            if (text.Trim() == "")
            {
                Debug.Log("������ �Է����� �ʾҽ��ϴ�.");
                return;
            }
            else
            {
                // Debug.Log("��������: " + Sex.text);
                //sex = Sex.text;
            }

        }
        public async void isMatch_LoadData()
        {
            myname = Name.text;
            await LoadData(); //���� ���� �ҷ�����
            LocalData(); //���� ���� ���� ����
            //await isMatchToken();
        }

        public async Task LoadData() //���̾���DB���� �������� �ҷ����� �Լ�
        {
            Query userRef = db.Collection("userInfo").WhereEqualTo("name", GCN); //�Է��� �г��Ӱ� ��ġ�ϴ� ���� ã�Ƽ� ����

            if (userRef != null) //���� ������ ������
            {
                Debug.Log("�������ִ�");
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

                        //������ ���� �亯 ���� �ҷ����� ( ���� �߰� )
                        myTqAnswer = int.Parse(docDictionary["TqAnswer"].ToString());
                        myLastTqIndex = int.Parse(docDictionary["lastTqIndex"].ToString());


                        //mbti = docDictionary["mbti"] as string;
                        //mannerLevel = int.Parse(docDictionary["mannerLevel"] as string);
                        Debug.Log(myname + "�� ���� �ҷ����� ���� -> " + docDictionary["sex"] as string);
                        //token = docDictionary["token"] as string;
                        //return;
                    }
                    Debug.Log("�г��� : " + GCN);
                    Debug.Log("ispass :" + ispass);
                    Debug.Log("���� : " + sex);
                    Debug.Log("���� : " + age);
                    Debug.Log("���ټҰ� : " + myintroduction);

                    //������ ���� �亯 ���� �ҷ����� ����� �α�( ���� �߰� )
                    Debug.Log("TQ �亯 ���� : " + myTqAnswer);
                    Debug.Log("������ �亯 TQ�ε��� : " + myLastTqIndex);
                });

            }
            else
            {
                Debug.Log("�������� ����");
            }
        }
        public static async Task ElseData(string userN) //���̾���DB���� �������� �ҷ����� �Լ�
        {
            Query userRef = db.Collection("userInfo").WhereEqualTo("name", userN); //�Է��� �г��Ӱ� ��ġ�ϴ� ���� ã�Ƽ� ����

            if (userRef != null) //���� ������ ������
            {

        Debug.Log("�ٸ����ã��");
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
                        Debug.Log(myname + "�� ���� �ҷ����� ���� -> " + docDictionary["sex"] as string);
                        //token = docDictionary["token"] as string;
                        //return;
                    }
                 
                    

                });
                
            }
            else
            {
                Debug.Log("�������� ����");
            }
        }
        void LocalData() //���������� ���� ���� (���Դܿ� ��)
        {
            Debug.Log("LocalData �Լ� �����");

            PlayerPrefs.SetString("name", myname); //�̸�
            //PlayerPrefs.SetString("sex", sex); //����
            //PlayerPrefs.SetInt("age", age); //����
            //PlayerPrefs.SetString("mbti", mbti); //�𷡾�����
            PlayerPrefs.SetInt("mannerLevel", mannerLevel); //�ųʵ��

            PlayerPrefs.Save();

        }

        private async Task isMatchToken() //��ū DB���� ���� Ȯ�� (���Դܿ� ��) 
        {
            CollectionReference userTokenColl = db.Collection("userToken");
            Query allTokenQuery = userTokenColl.WhereEqualTo("name", PlayerPrefs.GetString("name")); //�г������� ��ġ�ϴ� ���� ã�Ƽ� ����

            if (allTokenQuery != null)
            {
                await allTokenQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot allToken = task.Result;
                    foreach (DocumentSnapshot token in allToken.Documents)
                    {
                        Dictionary<string, object> tokenDictionary = token.ToDictionary();
                        Debug.Log(string.Format("����" + tokenDictionary["name"] as string + "�� ��ū: " + tokenDictionary["token"] as string));

                        Debug.Log("�����" + myname + "�� ��ū ��ġ Ȯ�ε�");
                    }

                });
            }
            else //��ū ���� ������ ��ūDB�� ���� �߰�
            {
                Dictionary<string, object> newTokenDic = new Dictionary<string, object>
            {
                {"name" , myname },
                {"token" , PlayerPrefs.GetString("token") }
            };

                DocumentReference newTokenDoc = userTokenColl.Document();
                await newTokenDoc.SetAsync(newTokenDic);

                Debug.Log("��ū DB ���� ������");
            };

            Debug.Log("isMatchToken �Լ� �����");
        }
        public string DCheck;
        public async void DoubleCheck() //��ū DB���� ���� Ȯ�� (���Դܿ� ��) 
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
                Debug.Log("�ߺ���");
                HText.SetActive(false);
                HTextError.SetActive(true);
                ErrorIndi.SetActive(true);
            }
            else
            {
                Debug.Log("�����");
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
            Debug.LogError("db�� Ű���� ���� �Ϸ�!");
            //var awaiter = delete_duplicatedKW().GetAwaiter();
            //awaiter.OnCompleted(() =>
            //{
            //    for (int i = 0; i < getKeywordList.saveKWlist.Count; i++)
            //    {
            //        KWList.Add(getKeywordList.saveKWlist[i]);
            //    }
            //    for (int j = 0; j < KWList.Count; j++)
            //    {
            //        Debug.Log("SAVE ���� ������ Ű����: " + KWList[j]);
            //    }
            //    db.Collection("userInfo").Document(GCN).UpdateAsync("Keyword", KWList);
            //    Debug.LogError("db�� Ű���� ���� �Ϸ�!");
            //});
            

        }
        
        //public async Task delete_duplicatedKW()
        //{
        //    await db.Collection("userInfo").Document(GCN).UpdateAsync("Keyword", FieldValue.Delete);
        //    Debug.LogError("DBŰ���� ���� �Ϸ�!");
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
                    Debug.Log("�����Ű�������");
                }
            });
            Debug.LogError("DB Ű���� �ҷ����� �Ϸ�!");
        }

        public static string MbtiType;
        public void mbtiData()
        {
            //MbtiType = PlayerPrefs.GetString("MBTIResult");
            MbtiType = TypeTestManager.MbtiType;

            Dictionary<string, object> user = new Dictionary<string, object>()
        {

            { "mbti", MbtiType }, //�𷡾����� 
            
        };

            StartCoroutine(delay(user));
        }

        
        IEnumerator delay(Dictionary<string, object> user)
        {
            yield return new WaitForSeconds(1f);
            db.Collection("userInfo").Document(GCN).SetAsync(user, SetOptions.MergeAll);
        }


        

        public string newAge;
        public void makeUserInfoDB() //����DB ���� (userInfo)
        {
            myname = Name.text;
            myintroduction = Introduction.text;

            if (SSex.options[SSex.value].text == ("����"))
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

            //���� ���ϱ�
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
            { "Introduction", myintroduction},//���ټҰ�
            { "name", myname }, //�г���
            { "age" , age  }, //����
            { "sex", sex }, //����
            { "ispass", ispass }, //������������� (�⺻ false ����)
            { "mbti", null }, //�𷡾����� 
            { "recentAccess", null }, //�ֱ����ӽð�
            { "signupDate", null }, //������ (ispass�� true�� �Ǹ� ���
            { "mannerLevel", 1 }, //�ųʵ�� (�⺻ 1������� ����)
            { "GmailAddress", GAdd},

            { "TqAnswer", 0}, //������ ���� �亯 ���� �⺻ 0(�亯 ���� ����)�� ����
            { "lastTqIndex", todayQIndex} //�亯�ؾ��ϴ� �ε����� ���� �ε����� ����
            //{ "keyWord", KWdict.ToDictionary}
        };


            db.Collection("userInfo").Document(myname).SetAsync(user);
            Dictionary<string, object> user2 = new Dictionary<string, object> {
            { "name", myname}
                };


      
            db.Collection("report").Document(myname).SetAsync(user2);
            PlayerPrefs.SetString("GCName", myname);

            


        }

        //today���� ��ü ���� ����ȭ�� ���� DB������ �о���� �Լ�
        /*
        - FirebaseManager��ũ��Ʈ�� ��ŸƮ�ϸ鼭 ������ ���� ������Ʈ ������ �ε��ϴ� �Լ��� �߰� �ۼ�
        - FirebaseManager��ũ��Ʈ���� ������Ʈ ������ �ε��ϴ� �Լ��� ������ �� ���� ���� ��¥�� ���Ͽ� ����ǥ��(bull) �׽�Ʈ
        - bull���� ���� ���� �ε��� or �ε��� + 1�� ������ ����
        - bull���� ���� �ε���+1�� ������Ʈ Ÿ�ӽ������� DB�� ���ε�
         */
        //�����ۼ�

        public async Task todayQupdate() //���̾���DB���� ������ ���� ������Ʈ ���ڸ� ������ ���� �������ڿ� ��, �ε��� ��ȣ�� ������Ʈ�ϴ� �Լ�
        {
            DocumentReference TQRef = db.Collection("userInfo").Document("todayQ"); //todayQ��ť��Ʈ ������ ������

            if (TQRef != null) //��ť��Ʈ ���� ������
            {

                await TQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    DocumentSnapshot snapshot = task.Result;
                    
                    Dictionary<string, object> docDictionary = snapshot.ToDictionary();
                    //Debug.Log("DBŸ�ӽ�Ʈ��: " + docDictionary["updateTime"].ToString());
                    //Debug.Log("DBŸ�� ����ƮŸ��: "+DateTime.Parse(docDictionary["updateTime"].ToString()));
                    //Debug.Log("DB�ε���: " + int.Parse(docDictionary["qIndex"].ToString()));
                    //Debug.Log("�� Ÿ�ӽ�����" + DateTime.Now.ToString());
                    
                    bool isEqualDay = true;
                    if(DateTime.Parse(docDictionary["updateTime"].ToString()).Date >= DateTime.Now.Date)
                    {
                        isEqualDay = true;
                        todayQIndex = int.Parse(docDictionary["qIndex"].ToString());
                        //Debug.Log("������Ʈ �ʿ����, �����ε���: " + todayQIndex);
                        
                        
                    }
                    else
                    {
                        isEqualDay = false;
                        todayQIndex = int.Parse(docDictionary["qIndex"].ToString())+1;
                        //Debug.Log("������Ʈ �ʿ���, �����ε���: " + todayQIndex);

                        Dictionary<string, object> newTQInfo = new Dictionary<string, object>
                        {
                            //������ ���� �ε���
                            { "qIndex", todayQIndex},
                            //������Ʈ ��¥
                            { "updateTime", DateTime.Now.Date.ToString() }
                        };
                        db.Collection("userInfo").Document("todayQ").SetAsync(newTQInfo, SetOptions.MergeAll);

                    }

                    //Debug.Log("DBŸ�ӽ�Ʈ��: " + docDictionary["updateTime"].ToString());
                    if (todayQIndex > myLastTqIndex)
                    {
                        Dictionary<string, object> TqAnswerState = new Dictionary<string, object>
                        {
                            //������ ���� �亯 ���� ������Ʈ, ������ �亯�� ������ �ε����� ���Ͽ� ���� �亯�� ������ ���� �ʱ�ȭ ��Ŵ
                            { "TqAnswer", 0}
                        };
                        //���¸� DB�� TqAnswer�� �������
                        db.Collection("userInfo").Document(GCN).SetAsync(TqAnswerState, SetOptions.MergeAll);
                        
                        //���������� ������Ʈ
                        myTqAnswer = 0;
                    }

                });

            }
            else
            {
                Debug.Log("��ť��Ʈ ��������");
            }
        }

        //�亯 1�� ������ �� ������ TqAnswer�� 1�� ������
        public static void setTqAnswer1()
        {
            Dictionary<string, object> TqAnswerState = new Dictionary<string, object>
                        {
                            //������ ���� �亯 ���� ������Ʈ, ������ �亯�� ������ �ε����� ���Ͽ� ���� �亯�� ������ ���� �ʱ�ȭ ��Ŵ
                            { "TqAnswer", 1},
                            { "lastTqIndex", todayQIndex}
                        };
            //���¸� DB�� TqAnswer�� �������
            Debug.Log(GCN);
            db.Collection("userInfo").Document(GCN).SetAsync(TqAnswerState, SetOptions.MergeAll);

            //���������� ������Ʈ
            myTqAnswer = 1;
        }

        //�亯 2�� ������ �� ������ TqAnswer�� 2�� ������
        public static void setTqAnswer2()
        {
            Dictionary<string, object> TqAnswerState = new Dictionary<string, object>
                        {
                            //������ ���� �亯 ���� ������Ʈ, ������ �亯�� ������ �ε����� ���Ͽ� ���� �亯�� ������ ���� �ʱ�ȭ ��Ŵ
                            { "TqAnswer", 2},
                            { "lastTqIndex", todayQIndex}
                        };
            //���¸� DB�� TqAnswer�� �������
            db.Collection("userInfo").Document(GCN).SetAsync(TqAnswerState, SetOptions.MergeAll);

            //���������� ������Ʈ
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
        //                Debug.LogError("foreach�� ���� Ȯ�� �α�");
        //                if (crName.Equals(name.ToString()) == false)
        //                {
        //                    bcount += 0; //���� �̸��� ���� �����϶��� 0�� ������
                            
        //                }
        //                else
        //                {
        //                    bcount += 1; //���� �̸��� ���� �̸��϶��� 1�� ������
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
        //        Debug.Log("ī��Ʈ  " + bcount);
        //    }
        //    else
        //    {
        //        //Console.WriteLine("Document {0} does not exist!", snapshot.Id);
        //    }
        //    return isCheckedCRname;
        //}

        /*
        public string ChannelID;

        public static LoadData_Chat() //���̾���DB���� �������� �ҷ����� �Լ�
        {
            //�ش��ϴ� ��ť��Ʈ���� channel ID�� ��ȯ�ϴ� �Լ��� �ۼ��ؾ���
            //doc.TryGetValue<>(, )
        }*/

    
}



