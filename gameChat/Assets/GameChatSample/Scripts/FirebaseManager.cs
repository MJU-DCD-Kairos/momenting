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
        public static string age;
        public string mbti;
        public static string ispass;
        public int mannerLevel;


        public bool isActive;
        public bool isInMatchingDB;


        //Ű���带 �����ϱ� ���� �����
        //Ű���� ī�װ��� ����Ʈ
        public List<string> tendencyKW = new List<string>();
        public List<string> interestKW = new List<string>();
        public List<string> lifestyleKW = new List<string>();

        //����Ʈ�� ���� �ٻ��ڵ尪���� ������ ��ųʸ� ����
        public Dictionary<string, string> KWdict = new Dictionary<string, string>();


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

            GCN = "";

            GCN = PlayerPrefs.GetString("GCName");
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




        //����Ʈ�� ����� ���� Ű���带 ī�װ� �����Ͽ� ����Ʈ�� ����
        public void KWcategory()
        {
            for (int i = 0; i < getKeywordList.saveKWlist.Count; i++)
            {
                if (getKeywordList.saveKWlist[i] == "��ŭ�߶�" ||
                    getKeywordList.saveKWlist[i] == "32����" ||
                    getKeywordList.saveKWlist[i] == "������" ||
                    getKeywordList.saveKWlist[i] == "���ν�" ||
                    getKeywordList.saveKWlist[i] == "���߷�" ||
                    getKeywordList.saveKWlist[i] == "�Ḹ��" ||
                    getKeywordList.saveKWlist[i] == "�����游" ||
                    getKeywordList.saveKWlist[i] == "��������" ||
                    getKeywordList.saveKWlist[i] == "���������Ŀ" ||
                    getKeywordList.saveKWlist[i] == "��������" ||
                    getKeywordList.saveKWlist[i] == "����������" ||
                    getKeywordList.saveKWlist[i] == "���ӷ���" ||
                    getKeywordList.saveKWlist[i] == "���дٽ�" ||
                    getKeywordList.saveKWlist[i] == "�ν��߿��ƽ�" ||
                    getKeywordList.saveKWlist[i] == "�ƽ��߿��ν�")
                {
                    //���� ī�װ� #ff8550
                    tendencyKW.Add(getKeywordList.saveKWlist[i]);
                    //KWdict.Add(getKeywordList.saveKWlist[i], "#ff8550");
                }
                else if (getKeywordList.saveKWlist[i] == "�󸮹���" ||
                    getKeywordList.saveKWlist[i] == "������" ||
                    getKeywordList.saveKWlist[i] == "�û�����" ||
                    getKeywordList.saveKWlist[i] == "�������۷�" ||
                    getKeywordList.saveKWlist[i] == "���ο��ⷯ" ||
                    getKeywordList.saveKWlist[i] == "�亸�� ��" ||
                    getKeywordList.saveKWlist[i] == "�Ϸ�ټ���" ||
                    getKeywordList.saveKWlist[i] == "�����" ||
                    getKeywordList.saveKWlist[i] == "��ĿȦ��" ||
                    getKeywordList.saveKWlist[i] == "����ġ��"
                    )
                {
                    //���� ī�װ� #7043c0
                    interestKW.Add(getKeywordList.saveKWlist[i]);
                    //KWdict.Add(getKeywordList.saveKWlist[i], "#7043c0");
                }
                else
                {
                    //��Ȱ���� ī�װ� #001130
                    lifestyleKW.Add(getKeywordList.saveKWlist[i]);
                    //KWdict.Add(getKeywordList.saveKWlist[i], "#001130");
                }
            }
            //for�� ���� �� �ش� ����Ʈ���� dict�� ����
            //KWdict.Add("#ff8550", tendencyKW);
            //KWdict.Add("#7043c0", interestKW);
            //KWdict.Add("#001130", lifestyleKW);
        }





        public void makeUserData() //���ο����� DB ����
        {
            if (Name.text.Trim() != "" && Name.text.Trim() == "")
            {
                Debug.Log("��ǲ�ʵ� �Է¹ޱ� ����");
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
            //{ "keyWord", KWdict.ToDictionary}
        };

            
            //���� ��ųʸ� ����
            //Dictionary<string, object> KWdictArray1 = new Dictionary<string, object> {

            //};
            ////���ɻ� ��ųʸ� ����
            //Dictionary<string, object> KWdictArray2 = new Dictionary<string, object> {
            //    };
            ////��������Ÿ�� ��ųʸ� ����
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
        public void sendRequestDB() //������û, ������û DB
        {
            string docname = ID.text + "sentRequest"; //���� id ����
            DocumentReference userRef = db.Collection("Users").Document(ID.text).Collection("sentRequest").Document(docname); //�⺻����DB ����
            userRef.SetAsync(new Dictionary<string, object>() //����
            {
                {"uid", null },
                {"isSend" , null  },
                {"matchingState", null },
                {"matchTime", null }

            });
        }
        public void chatDB() //ä�� DB 
        {

        }

        public void ImgDB() //���� DB
        {

        }
        public void profileDB() //�⺻������ DB ���� (�г���,����,����,���ټҰ�,�ųʵ��)
        {
            string docname = ID.text + "profile"; //���� id ����
            DocumentReference userRef = db.Collection("Users").Document(ID.text).Collection("profile").Document(docname); //�⺻����DB ����
            userRef.SetAsync(new Dictionary<string, object>() //����
            {
                {"name", null },
                {"age" , null  },
                {"sex", null },
                {"introduction", null },
                {"mannerScore", null }
            });
        }
        public void keywordDB() //Ű���� DB
        {
            string docname = ID.text + "keyword"; //���� id ����
            DocumentReference userRef = db.Collection("Users").Document(ID.text).Collection("keyword").Document(docname); //�⺻����DB ����
            userRef.SetAsync(new Dictionary<string, object>() //����
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
        public void reportDB() //�Ű� DB
        {
            string docname = ID.text + "report"; //���� id ����
            DocumentReference userRef = db.Collection("Users").Document(ID.text).Collection("report").Document(docname); //�⺻����DB ����
            userRef.SetAsync(new Dictionary<string, object>() //����
            {
                {"uid", null },
                {"reason" , null }

            });
        }
        public void mbtiType() //�𷡾����� DB
        {
       
        }

        public void mannerDB()
        {
            string docname = ID.text + "mannerRate"; //���� id ����
            DocumentReference userRef = db.Collection("Users").Document(ID.text).Collection("mannerRate").Document(docname); //�⺻����DB ����
            userRef.SetAsync(new Dictionary<string, object>() //����
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
                        Debug.LogError("foreach�� ���� Ȯ�� �α�");
                        if (crName.Equals(name.ToString()) == false)
                        {
                            bcount += 0; //���� �̸��� ���� �����϶��� 0�� ������
                            
                        }
                        else
                        {
                            bcount += 1; //���� �̸��� ���� �̸��϶��� 1�� ������
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
                Debug.Log("ī��Ʈ  " + bcount);
            }
            else
            {
                //Console.WriteLine("Document {0} does not exist!", snapshot.Id);
            }
            return isCheckedCRname;
        }

        /*
        public string ChannelID;

        public static LoadData_Chat() //���̾���DB���� �������� �ҷ����� �Լ�
        {
            //�ش��ϴ� ��ť��Ʈ���� channel ID�� ��ȯ�ϴ� �Լ��� �ۼ��ؾ���
            //doc.TryGetValue<>(, )
        }*/

    }
}



