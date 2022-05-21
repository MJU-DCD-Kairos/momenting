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
    public int usersex;
    public string docID; //��ť��Ʈ ����ID �����ϱ� ���� �ʿ�
    public int count;
    //public List<string> member;

    public ListenerRegistration listener;
    public ListenerRegistration listener2;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //username = PlayerPrefs.GetString("name");
        //sex = PlayerPrefs.GetString("��");
        username = "�ֺ�";
        usersex = 2;
        Debug.Log("���� �α��� ���� �г��� : " + username);
        Debug.Log("���� �α��� ���� ���� : " + usersex);
        docID = "";
        
    }
    
    public static string GAMECHAT_ROOM = "gameChatRoom";
    public static string ISOPEN = "isOpen";
    public static string ISACTIVE = "isActive";
    public static string MEMBER = "member";
    public static string NICKNAME = "nickName";
    public static string SEX = "sex";
    public static string CHANNELID = "channelID";
    public static string CREATETIME = "createTime";
    public static string OPENTIME = "openTime";
    //public ListenerRegistration listener;

    //������ �����Ͱ� ����� ������ ��ü������ �ҷ��ͼ� ��ü�������� 6�� �Ǹ� ��Ī����
   
    public async void matchingOn()
    {
        CollectionReference roomRef = FirebaseManager.db.Collection(GAMECHAT_ROOM); //ä�÷� �÷��� ����
        Query allroomRef = roomRef;
        await allroomRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allroomSnapshot = task.Result;
            foreach(DocumentSnapshot doc in allroomSnapshot.Documents)
            {
                docID = doc.Id;
                Debug.Log(docID);

                Dictionary<string, object> docDictionary = doc.ToDictionary();
                //Debug.Log(docDictionary[MEMBER].GetType());

                List<object> memberList = (List<object>)docDictionary[MEMBER];
                string open = docDictionary[ISOPEN].ToString();
                Debug.Log(open);

                if (open == "False")
                {
                    int fcount = 0;
                    int mcount = 0;
                    foreach (Dictionary<string, object> m in memberList)
                    {
                        //Debug.Log(m[SEX]);

                        if (m[SEX].ToString() == "1")
                        {
                            mcount++;
                        }
                        else
                        {
                            fcount++;
                        }

                    }
                    Debug.Log("������: " + mcount + " ������: " + fcount);

                    count = fcount + mcount;

                    Dictionary<string, object> addUser = new Dictionary<string, object>
                {
                    { NICKNAME , username },
                    { SEX , usersex }
                };
                    if (usersex == 2)
                    {
                        if (fcount <= 2)
                        {
                            roomRef.Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
                            fcount++;
                        }

                    }
                    else if (usersex == 1)
                    {
                        if (mcount <= 2)
                        {
                            roomRef.Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
                            mcount++;
                        }
                    }

                    if (fcount + mcount == 6)
                    {
                        //��ü�������� 6���̸� ä�ù��� ���� ���θ� true�� �ٲ�
                        roomRef.Document(docID).UpdateAsync(ISOPEN, true); //ä�ù� ����
                        roomRef.Document(docID).UpdateAsync(ISACTIVE, true); //ä�ù� Ȱ��ȭ
                        roomRef.Document(docID).UpdateAsync(OPENTIME, System.DateTime.Now.ToString()); //ä�ù� ���� �ð� ���
                    }
                    DocumentReference docRef = roomRef.Document(docID);
                    listener = docRef.Listen(snapshot =>
                    {
                        if (snapshot.Exists)
                        {
                            Debug.Log("�ݹ�");
                        }

                        else
                        {
                            Debug.Log(string.Format("������ �������� �ʽ��ϴ�!", snapshot.Id)); //���Ī �õ��ؾߵ�
                            //matchingOn();
                            listener.Stop();
                        }

                    });
                    
                    return;
                }

            }

            makeNewRoom(); // ���� ���� ����
            return;
        });

    }

    
    async void makeNewRoom() //ä�ù� ����
    {
        //StartCoroutine("CreateChatR"); //ä�ù�ID ����
        string channelID = "asdf"; //ä�ù�ID ����
        docID = "�ƹ��̸�"; //ä�ù��̸� ����
        Dictionary<string, object> addUser = new Dictionary<string, object> //member�� �߰��� ��������
                {
                    { NICKNAME , username },
                    { SEX , usersex }
                };

        Dictionary<string, object> room = new Dictionary<string, object>
        {
            { CHANNELID , channelID }, //ä�ù�ID �޾ƿͼ� �ֱ�
            { CREATETIME, System.DateTime.Now.ToString()}, //Ÿ�ӽ����� (����ð�)
            { ISACTIVE, false },
            { ISOPEN , false },
            { MEMBER , "" },
            { OPENTIME , null } //Ÿ�ӽ����� (6��� �ð�)
        };
        //���� ���� ����
        DocumentReference addmrRef = FirebaseManager.db.Collection(GAMECHAT_ROOM).Document(docID);
        await addmrRef.SetAsync(room).ContinueWithOnMainThread(task =>
        {
            Debug.Log(addmrRef.Id);
        });
        await FirebaseManager.db.Collection(GAMECHAT_ROOM).Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
        Debug.Log("ä�ù� ���� ������");

        listener2 = addmrRef.Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                Debug.Log("���ο� ���� ������Ʈ");
            }
            else
            {
                Debug.Log(string.Format("���� ������ ������ �������� �ʽ��ϴ�!", snapshot.Id)); //���Ī �õ��ؾߵ�
                listener2.Stop();
                matchingOn();
            }

            //listener.Stop();
        });
    }
    
}
