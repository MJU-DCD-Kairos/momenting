using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using FireStoreScript;
using System.Threading;
using System.Threading.Tasks;


public class MatchingManager : MonoBehaviour
{
    private FirebaseManager db;
    public string username; 
    public string sex; 
    public bool isActive;

    public string gamechatchannel; //����ê ä�ù� ä�� ID �޾ƿ� ����

    CollectionReference matchingRoomRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoom");

    // Start is called before the first frame update
    void Start()
    {
        db = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();
        //username = PlayerPrefs.GetString("name");
        //sex = PlayerPrefs.GetString("sex");
        //username = "����";
        //sex = "��";
        Debug.Log("���� �α��� ���� �г��� : " + username);
        Debug.Log("���� �α��� ���� ���� : " + sex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddUser() //���� �� �ѹ��� ����(��ĪDB�� ���� ���� ��������) -> �� db�� �ʿ��ֳ�? ������ �г���, ������ playterprefs�� ����ǰ�, isActive�� �ƿ� �ʿ�����
    {
        
        DocumentReference Mref = FireStoreScript.FirebaseManager.db.Collection("matchingUsers").Document(username);
        Mref.SetAsync(new Dictionary<string, object>()
        {
            {"name", username},
            {"sex", sex },
            {"isActive", false }
        });
    }

    public void OnclickMatching() //��Ī��ư ������ �� ȣ���� �Լ�
    {
        //Debug.Log(PlayerPrefs.GetString("username"));
        //AddUser();
        isActive = true; //��Ī���ɿ��θ� true�� �ٲ�

        /*
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
        */
        matching();

    }

    
    void matching() //MatchingRoom DB
    {
        //string roomId = null; //ä�ù�id 

        if(sex == "��") //������ �����̶��
        {
            //Debug.Log("��ã��");
            Query femaleRef = matchingRoomRef.WhereEqualTo("female", false).Limit(1); //���� ���� 2�� ������ �� �߿� 1���� ��ȯ 
            ListenerRegistration listener = femaleRef.Listen(snapshot =>
            {
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {

                    Debug.Log("join ������ �� : " + doc.Id);

                    //doc.GetValue < "count" > //count�� value �޾ƿ;��� 
                    count = count + 1; //���� ��ü ���� �� + 1 
                    DocumentReference docRef = matchingRoomRef.Document(doc.Id);
                    Dictionary<string, object> newUser = new Dictionary<string, object>
                    {
                        
                        {"count", count}, //��ü ���� �� 1 �÷��ֱ�
                        { "count_f" , count_f} //���� ���� �� 1 �÷��ֱ�
                    };
                    docRef.UpdateAsync(newUser); //count ���� ������Ʈ
                    
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
                /*
                if (snapshot != null)
                {
                    foreach(DocumentSnapshot doc in snapshot.Documents)
                    {
                        Debug.Log("join ������ �� : " + doc.Id);
                    }
                    
                }
                else
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
                    Debug.Log("���ο� �� ���� �ʿ�");

                }*/
            });
            listener.Stop();
            /*
            if (femaleRef != null) //���� ������ 3���� �� ���� ���� ���� �ִٸ�
            {
                ListenerRegistration listener = femaleRef.Listen(snapshot =>
                {
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Debug.Log("join ������ �� : " + doc.Id);
                    }

                });
                listener.Stop();
            }
            else //���� ������ 3���� �� ���� ���� ���� ���ٸ�
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
                Debug.Log("���ο� �� ���� �ʿ�");
            }*/
        }

        else if (sex == "��") //������ �����̶��
        {
            Query maleRef = null;
            maleRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoom").WhereEqualTo("male", false); //���� ���� 3���� �� ���� ���� �� ã��
            ListenerRegistration listener = maleRef.Listen(snapshot =>
            {
                if (snapshot != null) //���� ������ 3���� �� ���� ���� ���� �ִٸ�
                {
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Debug.Log("join ������ �� : " + doc.Id);
                    }
                }
                else //���� ������ 3���� �� ���� ���� ���� ���ٸ�
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
                    Debug.Log("���ο� �� ���� �ʿ�");

                }
            });
            listener.Stop();
            /*
            if (maleRef != null) //���� ������ 3���� �� ���� ���� ���� �ִٸ�
            {
                ListenerRegistration listener = maleRef.Listen(snapshot =>
                {
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Debug.Log("join ������ �� : " + doc.Id);
                    }

                });
                listener.Stop();
            }
            else //���� ������ 3���� �� ���� ���� ���� ���ٸ�
            {
                count = 1;
                count_f = 0;
                count_m = 1;
                female = false;
                male = false;
                m1 = username;
                m2 = null;
                m3 = null;
                m4 = null;
                m5 = null;
                m6 = null;
                
                makeRoom(); //���ο� �� ����
                Debug.Log("���ο� �� ���� �ʿ�");
            }
            */
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
        //string roomID = gamechatchannel; //ä�ù� ä�� �Ҵ�

        Dictionary<string, object> room = new Dictionary<string, object>
        {
            {"count", count },
            {"f_count", count_f },
            { "m_count", count_m},
            {"female" , female },
            {"male" , male },
            {"m1" , username },
            {"m2" , m2 },
            {"m3" , m3 },
            {"m4" , m4 },
            {"m5" , m5 },
            {"m6" , m6 }
        };
        FireStoreScript.FirebaseManager.db.Collection("matchingRoom").AddAsync(room); //���� ���� ����

        Debug.Log("ä�ù� ���� ������");


    }
}
