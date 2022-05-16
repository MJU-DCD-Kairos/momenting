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
        //���Ŵ��� �ı� ������ ���� �ڵ�
        DontDestroyOnLoad(this.gameObject);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            //var dependencyStatus = task.Result;
            if (task.Result == DependencyStatus.Available)
            {
                //app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log("DB ���� ����");
                db = FirebaseFirestore.DefaultInstance; //Cloud Firestore �ν��Ͻ� �ʱ�ȭ
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
    
    public void iID() //inputID �Լ� ȣ��
    {
        inputID(ID.text);
    }

    public void iPW() //inputPW �Լ� ȣ��
    {
        inputPW(PW.text);
    }

    public void inputID(string text) //���̵� ��ǲ ������ �޾ƿ���
    {
        if (text.Trim() == "")
        {
            Debug.Log("���̵� �Է����� �ʾҽ��ϴ�.");
            return;
        }
        else
        {
          
            Debug.Log("�������̵�: " + ID.text);
        }
        
    }
    public void inputPW (string text) //��й�ȣ ��ǲ ������ �޾ƿ���
    {
        if(text.Trim() == "")
        {
            Debug.Log("��й�ȣ�� �Է����� �ʾҽ��ϴ�.");
            return;
        }
        else
        {
            Debug.Log("������й�ȣ: " + PW.text); 
        }
        
    }


    void make_uidDB() //�������̵�� ��ū�� ��Ƴ��� db ����
    {
        Query allUidQuery = db.Collection("Users");
        allUidQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allUid = task.Result;
            foreach (DocumentSnapshot UsersDoc in allUid.Documents) //��� ����id (uid) �ҷ����� �ݺ�
            {
                Debug.Log(string.Format(UsersDoc.Id)); 
                
                //�������̵� ��Ƴ��� �÷��ǿ� uid ���� ����
                //������ �ִ��� Ȯ��ġ �����Ƿ� �� �����͸� ���� ������ �����ϴ� ��� ���
                //���� token�� īī�� �α��� �� �߱޹޴� ��ū�� �����ϵ��� �����ؾ���
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
    public void Udata() //���������� ���� ����
    {
        //PlayerPrefs.SetString("name", );
        //PlayerPrefs.SetString("sex", );
        PlayerPrefs.SetString("uid", ID.text);
        PlayerPrefs.Save();

    }

    public void Signin()
    {
        Debug.Log("��ư ����");
        Udata();
        Debug.Log(PlayerPrefs.GetString("uid"));

        /*
        if (ID.text.Trim() != "" && PW.text.Trim() != "")
        {
            Query allUidQuery = db.Collection("useridDB").WhereEqualTo("uid",ID.text); //uid�� �Է��� ���̵�� ��ġ�ϴ� ���� ��ȯ
            Debug.Log(allUidQuery.GetType());
            if (allUidQuery != null)
            {
                Debug.Log("�α��μ���");
            }
            else
            {
                Debug.Log("��ġ�ϴ� ���̵� �����ϴ�.");
            }
            /*
            allUidQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot allUid = task.Result;
                foreach (DocumentSnapshot UsersDoc in allUid.Documents) //��� ����id (uid) �ҷ����� �ݺ�
                {
                    Debug.Log(string.Format(UsersDoc.Id));

                    //�������̵� ��Ƴ��� �÷��ǿ� uid ���� ����
                    //������ �ִ��� Ȯ��ġ �����Ƿ� �� �����͸� ���� ������ �����ϴ� ��� ���
                    //���� token�� īī�� �α��� �� �߱޹޴� ��ū�� �����ϵ��� �����ؾ���
                    DocumentReference uidDoc = db.Collection("useridDB").Document(UsersDoc.Id);
                    Dictionary<string, object> update = new Dictionary<string, object>
                {
                    {"uid", UsersDoc.Id },
                    {"token" , null }
                };
                    uidDoc.SetAsync(update, SetOptions.MergeAll);
                }
            });


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
        if (ID.text.Trim() != "" && PW.text.Trim() != "")
        {
            Debug.Log("��ǲ�ʵ� �Է¹ޱ� ����");
            useridDB();
            //ImgDB();
            profileDB();
            keywordDB();
            mannerDB();
            sendRequestDB();
            reportDB();

        }
    }
    
    public void useridDB() //���ο����� �⺻���� ����
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

        //�ҷ�����
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
}

