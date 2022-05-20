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
    public int count; //��ü������ �����ϱ� ���� �ʿ�
    public int f_count; //���������� �����ϱ� ���� �ʿ�
    public int m_count; //���������� �����ϱ� ���� �ʿ�
    public string female; //�������� �� �� �ִ��� Ȯ���ϱ� ���� �ʿ� (db������ bool���ΰ� string���� ��ȯ����)
    public string male; //�������� �� �� �ִ��� Ȯ���ϱ� ���� �ʿ� (db������ bool���ΰ� string���� ��ȯ����)
    public string docID; //��ť��Ʈ ����ID �����ϱ� ���� �ʿ�
    //public List<string> member;

    void Start()
    {
        username = PlayerPrefs.GetString("name");
        sex = PlayerPrefs.GetString("��");
        Debug.Log("���� �α��� ���� �г��� : " + username);
        Debug.Log("���� �α��� ���� ���� : " + sex);
    }
    public void OnclickMatching() //��Ī��ư ������ �� ȣ���� �Լ�
    {
        matchingOn();
        //await checkMembers();
    }
    public async void matchingOn()
    {
        CollectionReference mrRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoom"); //��Ī�� �÷��� ����

        if (sex == "��")
        {
            Query femaleRef = mrRef.WhereEqualTo("female", false).Limit(1); //���� ���� 3�� �̸��� �� �߿� 1���� ��ȯ
            QuerySnapshot snapthot = await femaleRef.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in snapthot.Documents)
            {
                Debug.Log(doc.Id);
                docID = doc.Id;

                Dictionary<string, object> docDictionary = doc.ToDictionary();
               
                count = int.Parse(docDictionary["count"].ToString());
                f_count = int.Parse(docDictionary["f_count"].ToString());
                female = docDictionary["female"].ToString(); //bool ���� string���� ��ȯ

            }
            Debug.Log("���� �������� 3���̻�����: " + female);

            if (female == "False")
            {
                Debug.Log("���� �� "+ docID + "�� join��");
                
                count = count + 1; //��ü������ +1 �÷��ֱ�
                f_count = f_count + 1; //���������� +1 �÷��ֱ�
                
                if (f_count == 3) //������������ 3���̸�
                { 
                    Dictionary<string, object> joinName = new Dictionary<string, object>
                    {
                        {"female", true } //���������� ���̻� �������� true�� �ٲ���
                    };
                    await mrRef.Document(docID).UpdateAsync(joinName); //���� �ش�濡 ���������� ���̻� ������
                }

                Dictionary<string, object> newCount = new Dictionary<string, object>
                {
                    {"count" , count },
                    {"f_count" , f_count }
                };
                await mrRef.Document(docID).UpdateAsync(newCount); //��ü������, ���������� ī��Ʈ �÷��ֱ�
                await mrRef.Document(docID).UpdateAsync("member", FieldValue.ArrayUnion(username)); //member �迭�� �����г��� �߰�
                Debug.Log("������Ʈ�� ��ü������: " + count + ", ������Ʈ�� ����������" + f_count);
            }
            else
            {
                Debug.Log("���ο� �� ����");
                
                count = 1; //��ü������ 1
                f_count = 1; //���������� 1
                m_count = 0; //���������� 0
                makeNewRoom(); //���ο� �� �����

                Debug.Log("������Ʈ�� ��ü������: " + count + ", ������Ʈ�� ����������" + f_count);
            }

        }
        else if (sex == "��")
        {
            Query maleRef = mrRef.WhereEqualTo("male", false).Limit(1); //���� ���� 3�� �̸��� �� �߿� 1���� ��ȯ
            QuerySnapshot snapthot = await maleRef.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in snapthot.Documents)
            {
                docID = doc.Id;

                Dictionary<string, object> docDictionary = doc.ToDictionary();

                count = int.Parse(docDictionary["count"].ToString());
                m_count = int.Parse(docDictionary["m_count"].ToString());
                male = docDictionary["male"].ToString(); //bool ���� string���� ��ȯ

            }
            Debug.Log("���� �������� 3���̻�����: " + male);

            if (male == "False")
            {
                Debug.Log("���� �� " + docID + "�� join��");

                count = count + 1; //��ü������ +1 �÷��ֱ�
                m_count = m_count + 1; //���������� +1 �÷��ֱ�

                if (m_count == 3) //������������ 3���̸�
                {
                    Dictionary<string, object> joinName = new Dictionary<string, object>
                    {
                        {"male", true } //���������� ���̻� �������� true�� �ٲ���
                    };
                    await mrRef.Document(docID).UpdateAsync(joinName); //���� �ش�濡 ���������� ���̻� ������
                }

                Dictionary<string, object> newCount = new Dictionary<string, object>
                {
                    {"count" , count },
                    {"m_count" , m_count }
                };
                await mrRef.Document(docID).UpdateAsync(newCount); //��ü������, ���������� ī��Ʈ �÷��ֱ�
                await mrRef.Document(docID).UpdateAsync("member", FieldValue.ArrayUnion(username)); //member �迭�� �����г��� �߰�
                Debug.Log("������Ʈ�� ��ü������: " + count + ", ������Ʈ�� ����������" + m_count);
            }
            else
            {
                Debug.Log("���ο� �� ����");

                count = 1; //��ü������ 1
                f_count = 0; //���������� 0
                m_count = 1; //���������� 1
                makeNewRoom(); //���ο� �� �����

                Debug.Log("������Ʈ�� ��ü������: " + count + ", ������Ʈ�� ����������" + m_count);
            }
        }

        //������ �����Ͱ� ����� ������ ��ü������ �ҷ��ͼ� ��ü�������� 6�� �Ǹ� ��Ī����
        ListenerRegistration listener = mrRef.Document(docID).Listen(task =>
        {
            if (task.Exists)
            {
                Dictionary<string, object> users = task.ToDictionary();
                count = int.Parse(users["count"].ToString());
            }
            else
            {
                Debug.Log(string.Format("������ ���� �������� �ʽ��ϴ�!", task.Id));
            }

            Debug.Log("��ü������: " + count);
        });
        if (count == 6)
        {
            listener.Stop();
            Debug.Log("��Ī�����");
        }
    }

    public void checkMembers() //ä�ù� 6������ Ȯ���ϴ� �Լ�
    {
        Debug.Log("checkMembers �Լ� �����");
        DocumentReference mrDocRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoom").Document(docID);
        //await mrDocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        
        ListenerRegistration listener = mrDocRef.Listen(task =>
        {
            //DocumentSnapshot docSnapshot = task.Result;
            if (task.Exists)
            {
                Dictionary<string, object> users = task.ToDictionary();
                count = int.Parse(users["count"].ToString()); 
            }
            else
            {
                Debug.Log(string.Format("������ ���� �������� �ʽ��ϴ�!", task.Id));
            }
        });
        
        Debug.Log("��ü������: " + count); 
    }

    void makeNewRoom() //ä�ù� ����
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

}
