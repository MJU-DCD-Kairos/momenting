using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using FireStoreScript;


public class MatchingManager : MonoBehaviour
{
    //public static FirebaseFirestore db;
    private FirebaseManager db;
    public string username; 
    public string sex; 
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        db = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();
        username = PlayerPrefs.GetString("name");
        sex = PlayerPrefs.GetString("sex");

        Debug.Log("���� �α��� ���� �г��� : " + username);
        Debug.Log("���� �α��� ���� ���� : " + sex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddUser() //���� �� �ѹ��� ���� (��ĪDB)
    {
        
        DocumentReference Mref = db.db.Collection("matchingUsers").Document(username);
        Mref.SetAsync(new Dictionary<string, object>()
        {
            {"name", username},
            {"sex", sex },
            {"isActive", false }
        });
    }

    public void OnclickMatching() //��Ī��ư ������ �� ȣ���� �Լ�
    {
        //AddUser();
        isActive = true; //��Ī���ɿ��θ� true�� �ٲ�

        Query query = db.db.Collection("matchingUsers").WhereEqualTo("name", username);
        query.GetSnapshotAsync().ContinueWithOnMainThread((QuerySnapshotTask) =>
        {
            foreach(DocumentSnapshot doc in QuerySnapshotTask.Result)
            {
                Debug.Log(string.Format(doc.Id));
            }

        });

        DocumentReference userRef = db.db.Collection("matchingUsers").Document(username);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            {"isActive", true }
        };
        userRef.UpdateAsync(updates).ContinueWithOnMainThread(task => {
            Debug.Log(isActive);
        });

        matching();

    }
    async void matching() //MatchingRoom DB
    {
        //string roomId = null; //ä�ù�id 

        if(PlayerPrefs.GetString("sex") == "��") //������ �����̶��
        {
            Query femaleRef = db.db.Collection("matchingRoom").WhereEqualTo("female", false); //���� ���� 3���� �� ���� ���� �� ã��
            
            if (femaleRef == null) //���� ������ 3���� �� ���� ���� ���� ���ٸ�
            {
                count = 1;
                count_f = 1;
                count_m = 0;
                female = false;
                male = false;
                m1 = username;
                m2 = null;
                m3 = null;
                m4 = null;
                m5 = null;
                m6 = null;

                makeRoom(); //���ο� �� ����

            }

            else if (femaleRef != null) //���� ������ 3���� �� ���� ���� ���� �ִٸ�
            {
                await femaleRef.GetSnapshotAsync().ContinueWithOnMainThread((QuerySnapshotTask) =>
                {
                    foreach (DocumentSnapshot roomdoc in QuerySnapshotTask.Result.Documents)
                    {
                        Debug.Log("���������� 3�� �̸��� �� : " + roomdoc.Id);
                    }
                });
            }
        }

        else if (PlayerPrefs.GetString("sex") == "��") //������ �����̶��
        {
            Query maleRef = db.db.Collection("matchingRoom").WhereEqualTo("male", false); //���� ���� 3���� �� ���� ���� �� ã��
            
            if (maleRef == null) //���� ������ 3���� �� ���� ���� ���� ���ٸ�
            {
                count = 1;
                count_f = 1;
                count_m = 0;
                female = false;
                male = false;
                m1 = username;
                m2 = null;
                m3 = null;
                m4 = null;
                m5 = null;
                m6 = null;

                makeRoom(); //���ο� �� ����

            }

            else if(maleRef != null) //���� ������ 3���� �� ���� ���� ���� �ִٸ�
            {
                await maleRef.GetSnapshotAsync().ContinueWithOnMainThread((QuerySnapshotTask) =>
                {
                    foreach (DocumentSnapshot roomdoc in QuerySnapshotTask.Result.Documents)
                    {
                        Debug.Log("���������� 3�� �̸��� �� : " + roomdoc.Id);
                    }
                });
                
            }
        }
        
    }


    public int count;
    public int count_f;
    public int count_m;
    public bool female;
    public bool male;
    public string m1;
    public string m2;
    public string m3;
    public string m4;
    public string m5;
    public string m6;

    public void makeRoom() //ä�ù� ����
    {
        Dictionary<string, object> room = new Dictionary<string, object>
        {
            {"count", count },
            {"f_count", count_f },
            { "m_count", count_m},
            {"female" , female },
            {"male" , male },
            {"m1" , m1 },
            {"m2" , m2 },
            {"m3" , m3 },
            {"m4" , m4 },
            {"m5" , m5 },
            {"m6" , m6 }
        };
        db.db.Collection("matchingRoom").AddAsync(room).ContinueWithOnMainThread(task =>
        {
            DocumentReference addRoom = task.Result;
            Debug.Log(string.Format(addRoom.Id));
        });
        
    }
}
