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
    public string username; 
    public string sex;

    //public CollectionReference matchingRoomRef;//��Ī�� �÷��� ������ ����
    public int count;
    public int f_count;
    public int m_count;
    //public bool female = true;
    public string female;
    public bool male;
    public string member;
    public string docID;
    //public List<string> member;

    void Start()
    {
        //username = "����";
        //sex = "��";
        Debug.Log("���� �α��� ���� �г��� : " + username);
        Debug.Log("���� �α��� ���� ���� : " + sex);
    }
    public void OnclickMatching() //��Ī��ư ������ �� ȣ���� �Լ�
    {
        matchingOn();
    }
    public async void matchingOn()
    {
        CollectionReference mrRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoom");
        if (sex == "��")
        {
            //Debug.Log(sex);

            Query femaleRef = mrRef.WhereEqualTo("female", false).Limit(1); //���� ���� 2�� ������ �� �߿� 1���� ��ȯ
            QuerySnapshot snapthot = await femaleRef.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in snapthot.Documents)
            {
                Debug.Log(doc.Id);
                Dictionary<string, object> docDictionary = doc.ToDictionary();
               
                count = int.Parse(docDictionary["count"].ToString());
                f_count = int.Parse(docDictionary["f_count"].ToString());
                female = docDictionary["female"].ToString();
                //member = docDictionary["member"].ToString();

                //Debug.Log("���type: " + docDictionary["member"].GetType());

            }
            Debug.Log("���� �������� 3���̻�����: " + female);
            //Debug.Log(member);
            if (female == "False")
            {
                Debug.Log("���� �濡 join��");
                
                count = count + 1; //��ü������ +1 �÷��ֱ�
                f_count = f_count + 1; //���������� +1 �÷��ֱ�
                /*
                if (f_count == 3) //������������ 3���̸�
                { 
                    Dictionary<string, object> joinName = new Dictionary<string, object>
                    {
                        {"female", true } //���������� ���̻� �������� true�� �ٲ���
                    };
                    await mrRef.Document(docID).UpdateAsync(joinName);
                }

                Dictionary<string, object> newCount = new Dictionary<string, object>
                {
                    {"count" , count },
                    {"f_count" , f_count },
                    {"member", member }
                };
                await mrRef.Document(docID).UpdateAsync(newCount);

                Debug.Log("������Ʈ�� ��ü������: " + count);
                Debug.Log("������Ʈ�� ����������: " + f_count);
                Debug.Log("������Ʈ�� �������� �� á���� ����: " + female);*/
            }
            else
            {
                Debug.Log("���ο� �� ����");
                
                count = 1; //��ü������ 1
                f_count = 1; //���������� 1
                makeNewRoom(); //���ο� �� �����

                Debug.Log("������Ʈ�� ��ü������: " + count);
                Debug.Log("������Ʈ�� ����������: " + f_count);
                Debug.Log("������Ʈ�� �������� �� á���� ����: " + female);
            }

        }
    }
    public void makeNewRoom() //ä�ù� ����
    {
        Dictionary<string, object> room = new Dictionary<string, object>
        {
            {"count", count },
            {"f_count", f_count },
            { "m_count", m_count },
            {"female" , false },
            {"male" , false },
            {"member" , new List<object>() { username } }
        };

        FireStoreScript.FirebaseManager.db.Collection("matchingRoom").AddAsync(room); //���� ���� ����
        //matchingRoomRef.AddAsync(room);
        Debug.Log("ä�ù� ���� ������");
    }

    /*
    void matching() //MatchingRoom DB
    {
        if (sex == "��") //������ �����̶��
        {
            CollectionReference matchingRoomRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoomRef");
            //if (matchingRoomRef != null)
            //{
            //    Debug.Log("�ݷ��� ���� ����");
            //}
            Query femaleRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoomRef").WhereEqualTo("female", false); //���� ���� 2�� ������ �� �߿� 1���� ��ȯ }
            //if (matchingRoomRef != null)
            //{
            //    Debug.Log("���� ���� ����");
            //}

            //Debug.Log("���� ��ȯ��");
            /*
            femaleRef.GetSnapshotAsync().ContinueWithOnMainThread((task) =>
            {
                QuerySnapshot snapshot = task.Result;
                //Debug.Log(snapshot);
                foreach(DocumentSnapshot doc in snapshot.Documents)
                {
                    Debug.Log(doc);
                    Debug.Log("join ������ �� : " + doc.Id);

                    //doc.GetValue < "count" > //count�� value �޾ƿ;��� 
                    count = count + 1; //���� ��ü ���� �� + 1  }
                    f_count = f_count + 1;
                    member.Add(username);
                    DocumentReference docRef = matchingRoomRef.Document(doc.Id);
                    Dictionary<string, object> newUser = new Dictionary<string, object>
                    {
                        {"count", count}, //��ü ���� �� 1 �÷��ֱ�
                        { "count_f" , f_count}, //���� ���� �� 1 �÷��ֱ�
                        {"member" , member } //��������Ʈ�� �����г��� �߰�
                    };
                    docRef.UpdateAsync(newUser); //count ���� ������Ʈ

                    if (f_count == 3)
                    {
                        Dictionary<string, object> femaleCount = new Dictionary<string, object>
                        {
                            { "female" , true } //���� �������� 3���̸� female�ʵ��� value�� true�� �ٲ���
                        };
                        docRef.UpdateAsync(femaleCount); //count ���� ������Ʈ
                        if (female == true)
                        {
                            count = 1;
                            f_count = 1;
                            m_count = 0;
                            female = false;
                            male = false;

                            makeRoom(); //���ο� �� ����
                        }
                    }
                }

            });
            
            ListenerRegistration listener = femaleRef.Listen(snapshot =>
            {
                Debug.Log(snapshot);
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    Debug.Log("join ������ �� : " + doc.Id);

                    //doc.GetValue < "count" > //count�� value �޾ƿ;��� 
                    count = count + 1; //���� ��ü ���� �� + 1  }
                    f_count = f_count + 1;
                    member.Add(username);
                    DocumentReference docRef = matchingRoomRef.Document(doc.Id);
                    Dictionary<string, object> newUser = new Dictionary<string, object>
                    {
                        {"count", count}, //��ü ���� �� 1 �÷��ֱ�
                        { "count_f" , f_count}, //���� ���� �� 1 �÷��ֱ�
                        {"member" , member } //��������Ʈ�� �����г��� �߰�
                    };
                    docRef.UpdateAsync(newUser); //count ���� ������Ʈ

                    if (f_count == 3)
                    {
                        Dictionary<string, object> femaleCount = new Dictionary<string, object>
                        {
                            { "female" , true } //���� �������� 3���̸� female�ʵ��� value�� true�� �ٲ���
                        };
                        docRef.UpdateAsync(femaleCount); //count ���� ������Ʈ
                        if (female == true)
                        {
                            count = 1;
                            f_count = 1;
                            m_count = 0;
                            female = false;
                            male = false;

                            makeRoom(); //���ο� �� ����
                        }
                    }
                    
                }

            });

            listener.Stop();
        }
        else if (sex == "��") //������ �����̶��
        {
            CollectionReference matchingRoomRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoomRef");
            Query maleRef = matchingRoomRef.WhereEqualTo("male", false).OrderBy("name").Limit(1); //���� ���� 2�� ������ �� �߿� 1���� ��ȯ }

            ListenerRegistration listener = maleRef.Listen(snapshot =>
            {
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    Debug.Log("join ������ �� : " + doc.Id);

                    //doc.GetValue < "count" > //count�� value �޾ƿ;��� 
                    count = count + 1; //���� ��ü ���� �� + 1  }
                    m_count = m_count + 1;
                    member.Add(username);
                    DocumentReference docRef = matchingRoomRef.Document(doc.Id);
                    Dictionary<string, object> newUser = new Dictionary<string, object>
                    {
                        {"count", count}, //��ü ���� �� 1 �÷��ֱ�
                        { "m_count" , m_count}, //���� ���� �� 1 �÷��ֱ�
                        { "member" , member }
                    };
                    docRef.UpdateAsync(newUser); //count ���� ������Ʈ

                    if (m_count == 3)
                    {
                        Dictionary<string, object> maleCount = new Dictionary<string, object>
                        {
                            { "male" , true } //���� �������� 3���̸� female�ʵ��� value�� true�� �ٲ���
                        };
                        docRef.UpdateAsync(maleCount); //count ���� ������Ʈ
                    }
                    count = 1;
                    f_count = 0;
                    m_count = 1;
                    female = false;
                    male = false;

                    makeRoom(); //���ο� �� ����

                }

            });

            listener.Stop();
        }
    
        
    }
    */



}
