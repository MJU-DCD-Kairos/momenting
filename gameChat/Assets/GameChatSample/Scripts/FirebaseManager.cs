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

    public void Start()
    {
        //���Ŵ��� �ı� ������ ���� �ڵ�
        DontDestroyOnLoad(this.gameObject);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            //var dependencyStatus = task.Result;
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("DB ���� ����");
                db = FirebaseFirestore.DefaultInstance; //Cloud Firestore �ν��Ͻ� �ʱ�ȭ
                
                //makeUserData();
                //makeTokenDB();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
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
            Debug.Log("���̵� �Է����� �ʾҽ��ϴ�.");
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
            Debug.Log("���̵� �Է����� �ʾҽ��ϴ�.");
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
            Debug.Log("���̵� �Է����� �ʾҽ��ϴ�.");
            return;
        }
        else
        {
            Debug.Log("������ū: " + Token.text);
        }

    }
    public async void Onclick_LoadData()
    {//��Ī��ư ������ ���� ��ǲ�ʵ忡 �г��� �Է� ���� �ϱ� (�г������� ��ȸ)
        myname = Name.text;
        await LoadData(); //���� ���� �ҷ�����
        LocalData(); //���� ���� ���� ����

    }

    private async Task LoadData() //���̾���DB���� �������� �ҷ����� �Լ�
    {
        Query userRef = db.Collection("userInfo").WhereEqualTo("name", myname); //�Է��� �г��Ӱ� ��ġ�ϴ� ���� ã�Ƽ� ����
        QuerySnapshot snapshot = await userRef.GetSnapshotAsync();
        foreach (DocumentSnapshot doc in snapshot.Documents)
        {
            Dictionary<string, object> docDictionary = doc.ToDictionary();
            myname = docDictionary["name"] as string;
            sex = docDictionary["sex"] as string;
        }
        Debug.Log("�г��� : " + myname);
        Debug.Log("���� : " + sex);
    }
    /*
    async Task LocalData() //���������� ���� ����
    {
        token = "qpiubf92qqq8g2fco6qo943gafkugbskvubjhgqo34"; //��ū���ڿ� �����ϴ� ����

        PlayerPrefs.SetString("token", token); //��ū
        PlayerPrefs.SetString("name", myname); //�̸�
        PlayerPrefs.SetString("sex", sex); //����
        PlayerPrefs.SetInt("age", age); //����
        PlayerPrefs.SetString("mbti", mbti); //�𷡾�����
        PlayerPrefs.SetInt("mannerLevel", mannerLevel); //�ųʵ��
        //PlayerPrefs.SetString("isActive", "false");
        PlayerPrefs.Save();
    }
    */
    void LocalData() //���������� ���� ����
    {
        token = "qpiubf92qqq8g2fco6qo943gafkugbskvubjhgqo34"; //��ū���ڿ� �����ϴ� ����

        PlayerPrefs.SetString("token", token); //��ū
        PlayerPrefs.SetString("name", myname); //�̸�
        PlayerPrefs.SetString("sex", sex); //����
        PlayerPrefs.SetInt("age", age); //����
        PlayerPrefs.SetString("mbti", mbti); //�𷡾�����
        PlayerPrefs.SetInt("mannerLevel", mannerLevel); //�ųʵ��
        //PlayerPrefs.SetString("isActive", "false");
        PlayerPrefs.Save();
    }
    void makeTokenDB() //�������̵�� ��ū�� ��Ƴ��� db ����
    {
        Query allUTokenQuery = db.Collection("userToken");
        allUTokenQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allToken = task.Result;
            foreach (DocumentSnapshot UserToken in allToken.Documents) //��� ����id (uid) �ҷ����� �ݺ�
            {
                Debug.Log(string.Format(UserToken.Id)); 
                
                //������ �ִ��� Ȯ��ġ �����Ƿ� �� �����͸� ���� ������ �����ϴ� ��� ���
                //���� token�� īī�� �α��� �� �߱޹޴� ��ū�� �����ϵ��� �����ؾ���
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
        if(Name.text.Trim() != "" && Name.text.Trim() == myname) //��ū���� �ٲ�� ��
        {
            Debug.Log("�α��οϷ�!");

        }

        /*
            CollectionReference user = db.Collection("Users"); //Users �÷��� ����
            Query uidquery = user.WhereEqualTo("uid", ID.text); //uid�� �Է��� ID�� ��ġ�ϴ� ���� ã��
            uidquery.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
            {
                foreach (DocumentSnapshot docSnapshot in querySnapshotTask.Result.Documents)
                {
                    Debug.Log(string.Format("�������̵�", docSnapshot.Id));
                    if (docSnapshot.Id == ID.text)
                    {
                        Debug.Log("��ġ�ϴ� ���̵� �ֽ��ϴ�.");
                    }
                    else
                    {
                        Debug.Log("��ġ�ϴ� ���̵� �����ϴ�!");
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

        db.Collection("userInfo").AddAsync(user).ContinueWithOnMainThread(task =>
        {
            DocumentReference addUserInfo = task.Result;
            Debug.Log(string.Format("�߰��� ���� ID: {0}.", addUserInfo.Id));
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

