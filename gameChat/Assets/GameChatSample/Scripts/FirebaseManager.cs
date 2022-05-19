
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

namespace FirestoreScript {
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseFirestore db;

        public InputField Name;
        public InputField Age;//db ���� �׽�Ʈ�� ���� ���̵� ��ǲ�ʵ�
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

        public enum fbRef { userInfo, matchingRoom, report, userToken, keywords, chatRoom, images, mannerRate}
        public void Start()
        {
            //���Ŵ��� �ı� ������ ���� �ڵ�
            DontDestroyOnLoad(this.gameObject);

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
            //var dependencyStatus = task.Result;
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

            //isMatchToken(); //��ū ���� �ִ��� Ȯ��
            //Debug.Log("�����г��� : " + PlayerPrefs.GetString("name"));
            //Debug.Log("�������� : " + PlayerPrefs.GetString("sex"));

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
                Debug.Log("��������: " + Sex.text);
                sex = Sex.text;
            }

        }
        public async void isMatch_LoadData()
        {
            myname = Name.text;
            await LoadData(); //���� ���� �ҷ�����
            LocalData(); //���� ���� ���� ����
            //await isMatchToken();
        }

        private async Task LoadData() //���̾���DB���� �������� �ҷ����� �Լ�
        {
            Query userRef = db.Collection("userInfo").WhereEqualTo("name", myname); //�Է��� �г��Ӱ� ��ġ�ϴ� ���� ã�Ƽ� ����

            if (userRef != null) //���� ������ ������
            {
                await userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Dictionary<string, object> docDictionary = doc.ToDictionary();
                        myname = docDictionary["name"] as string;
                        sex = docDictionary["sex"] as string;
                    //age = int.Parse(docDictionary["age"] as string);
                    //mbti = docDictionary["mbti"] as string;
                    //mannerLevel = int.Parse(docDictionary["mannerLevel"] as string);
                    Debug.Log(myname + "�� ���� �ҷ����� ���� -> " + docDictionary["sex"] as string);
                    //token = docDictionary["token"] as string;
                    //return;
                }
                    Debug.Log("�г��� : " + myname);
                    Debug.Log("���� : " + sex);
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
            PlayerPrefs.SetString("sex", sex); //����
            PlayerPrefs.SetInt("age", age); //����
            PlayerPrefs.SetString("mbti", mbti); //�𷡾�����
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

        public void makeUserInfoDB() //����DB ���� (userInfo)
        {
            myname = Name.text;
            age = int.Parse(Age.text);
            sex = Sex.text;
            token = Token.text;
            ispass = true;
            isActive = false;

            Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "name", myname }, //�г���
            { "age" , age  }, //����
            { "sex", sex }, //����
            { "ispass", ispass }, //������������� (�⺻ false ����)
            { "mbti", null }, //�𷡾����� 
            { "isActive", isActive }, //��Ī���ɿ���
            { "recentAccess", null }, //�ֱ����ӽð�
            { "signupDate", null }, //������ (ispass�� true�� �Ǹ� ���
            {"token", token }, //��ū
            { "mannerLevel", 1 } //�ųʵ�� (�⺻ 1������� ����)
        };

            db.Collection("userInfo").Document(myname).SetAsync(user).ContinueWithOnMainThread(task =>
            {
            //Debug.Log(string.Format("�߰��� ���� ID: {0}.", addUserInfo.Id));
        });

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

        public string passString;
        public async Task LoadData_ispass() //���̾���DB���� �������� �ҷ����� �Լ�
        {
            Query pass = db.Collection("userInfo").WhereEqualTo("ispass", passString); //�Է��� �г��Ӱ� ��ġ�ϴ� ���� ã�Ƽ� ����
            QuerySnapshot snapshot = await pass.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                Dictionary<string, object> docDictionary = doc.ToDictionary();

                passString = docDictionary["ispass"] as string;

                if ("true" == passString)

                {
                    ispass = true;
                }
                else
                {
                    ispass = false;
                }
            }

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



