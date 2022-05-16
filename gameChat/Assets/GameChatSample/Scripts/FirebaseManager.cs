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
    public FirebaseFirestore db;

    public InputField ID;
    public InputField PW;
    public string uid;
    public string upw;

    void Start()
    {
        //���Ŵ��� �ı� ������ ���� �ڵ�
        DontDestroyOnLoad(this.gameObject);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            //var dependencyStatus = task.Result;
            if (task.Result == DependencyStatus.Available)
            {
                //app = Firebase.FirebaseApp.DefaultInstance;
                db = FirebaseFirestore.DefaultInstance; //Cloud Firestore �ν��Ͻ� �ʱ�ȭ
                Debug.Log("DB ���� ����");
                //useridDB();
                //ReadData();
                //LoadData();
                makeUserData();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });

    }
    
    //QuerySnapshot snapshot = await.userRef.GetSnapshotAsync();
    //Debug.Log(user1);
    //DocumentReference docRef = db.Collection("Users").Document("Person"); //Users �÷��ǿ��� "Person"���� �ҷ�����
    //QuerySnapshot snapshot = await userRef.GetSnapshotAsync(); //���� �ȿ� �ִ� data ��������� ������ ��û
    /*
    foreach (DocumentSnapshot document in snapshot.Documents)
    {
        Dictionary<string, object> documentDictionary = document.ToDictionary(); //���� �÷��ǿ��� .document��� �Ӽ��� ���ذ� �����鿡 ����. �� ������ dictionary�� �޾Ƽ� .todictionary�� ���� �ٲ��ָ� �����ϰ� �����͸� ó���� �� �ִ�.
        Debug.Log("name:  " + documentDictionary["name"] as string);
        if (documentDictionary.ContainsKey("name"))
        {
            Debug.Log("t")
        }
    }
    */

    public void ReadData()
    {
        
        CollectionReference userRef = db.Collection("Users"); //���������� �����Ͱ� ����� �÷��� �ҷ�����
        Debug.Log(userRef.GetType());
        
        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            
            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                Debug.Log(string.Format("age {0}",documentDictionary["age"]));
                Debug.Log(string.Format("age: {0}", document.Id));
                
                Debug.Log(string.Format("introduction: {1}", document.Id));
                Debug.Log(string.Format("mannerScore: {2}", document.Id));
                Debug.Log(string.Format("mbti: {3}", document.Id));
                Debug.Log(string.Format("name: {4}", document.Id));
                Debug.Log(string.Format("sex: {5}", document.Id));
                Debug.Log(string.Format("state: {6}", document.Id));
                Debug.Log(string.Format("todayQ: {7}", document.Id));
                Debug.Log(string.Format("uid: {8}", document.Id));
            }
        });
        
    }

    public void LoadData()
    {
        DocumentReference docRef = db.Collection("Users").Document("Person");
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                
                Dictionary<string, object> city = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in city)
                {
                    Debug.Log(string.Format("{0}: {1}", pair.Key, pair.Value));
                }
            }
            else
            {
                Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
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

