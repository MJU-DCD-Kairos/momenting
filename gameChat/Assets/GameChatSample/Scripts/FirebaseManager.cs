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
    }
    
    public void myName() { inputName(Name.text); }//inputID �Լ� ȣ��
    public void myage() { inputAge(Age.text); }
    public void mySex() { inputSex(Sex.text); }
    public void myToken() { inputToken(Token.text); }
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
        }
        
    }
    public void inputAge(string text) //���̵� ��ǲ ������ �޾ƿ���
    {
        if (text.Trim() == "")
        {
            Debug.Log("���̸� �Է����� �ʾҽ��ϴ�.");
            return;
        }
        else
        {
            Debug.Log("��������: " + Age.text);
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
        }

    }
    public void inputToken(string text) //���̵� ��ǲ ������ �޾ƿ���
    {
        if (text.Trim() == "")
        {
            Debug.Log(" ��ū ");
            return;
        }
        else
        {
            Debug.Log("������ū: " + Token.text);
        }

    }
    public async void isMatch_LoadData()
    {//��Ī��ư ������ ���� ��ǲ�ʵ忡 �г��� �Է� ���� �ϱ� (�г������� ��ȸ)
        myname = Name.text;
        await LoadData(); //���� ���� �ҷ�����
        LocalData(); //���� ���� ���� ����
        isMatchToken();
    }

    private async Task LoadData() //���̾���DB���� �������� �ҷ����� �Լ�
    {
        Query userRef = db.Collection("userInfo").WhereEqualTo("name", myname); //�Է��� �г��Ӱ� ��ġ�ϴ� ���� ã�Ƽ� ����
        
        if(userRef != null) //���� ������ ������
        {
            QuerySnapshot snapshot = await userRef.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                Dictionary<string, object> docDictionary = doc.ToDictionary();
                //myname = docDictionary["name"] as string;
                sex = docDictionary["sex"] as string;
            }
            Debug.Log("�г��� : " + myname);
            Debug.Log("���� : " + sex);
        }
        
    }
   
    void LocalData() //���������� ���� ���� (���Դܿ� ��)
    {
        PlayerPrefs.SetString("name", myname); //�̸�
        PlayerPrefs.SetString("sex", sex); //����
        PlayerPrefs.SetInt("age", age); //����
        PlayerPrefs.SetString("mbti", mbti); //�𷡾�����
        PlayerPrefs.SetInt("mannerLevel", mannerLevel); //�ųʵ��
        isInMatchingDB = false;
        PlayerPrefs.SetString("isInMatchingDB", isInMatchingDB.ToString());
        
        PlayerPrefs.Save();
    }
    
    async void isMatchToken() //��ū DB���� ���� Ȯ�� (���Դܿ� ��) 
    {
        //Query allTokenQuery = null;

        Query allTokenQuery = db.Collection("userToken").WhereEqualTo("name", PlayerPrefs.GetString("name")); //�г������� ��ġ�ϴ� ���� ã�Ƽ� ����
        await allTokenQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allToken = task.Result;
            foreach (DocumentSnapshot token in allToken.Documents)
            {
                Dictionary<string, object> tokenDictionary = token.ToDictionary();

                if (tokenDictionary["token"] as string != null) //��ū ������ ������
                {
                    Debug.Log(string.Format(tokenDictionary["name"] as string));
                    Debug.Log(string.Format(tokenDictionary["token"] as string));
                }

                else //��ū ���� ������ ��ūDB�� ���� �߰�
                {
                    CollectionReference tokenColl = db.Collection("userToken"); //��ū �ݷ��� ����
                    Dictionary<string, object> update = new Dictionary<string, object> //���ο� ��ųʸ� ����
                    {
                        {"name", PlayerPrefs.GetString("name") },
                        {"token" , PlayerPrefs.GetString("token") }
                    };
                    tokenColl.AddAsync(update);
                }
                

                /*
                if (allToken != null) //��ū ������ ������
                {
                    Dictionary<string, object> tokenDictionary = token.ToDictionary();

                    Debug.Log(string.Format(tokenDictionary["name"] as string));
                    Debug.Log(string.Format(tokenDictionary["token"] as string));
                }

                else //��ū ���� ������ ��ūDB�� ���� �߰�
                {
                    CollectionReference tokenColl = db.Collection("userToken"); //��ū �ݷ��� ����
                    Dictionary<string, object> update = new Dictionary<string, object> //���ο� ��ųʸ� ����
                    {
                        {"name", PlayerPrefs.GetString("name") },
                        {"token" , PlayerPrefs.GetString("token") }
                    };
                    tokenColl.AddAsync(update);
                }*/
            }

        });
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
}

