using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameChatUnity;
using GameChatUnity.SimpleJSON;
using GameChatUnity.SocketIO;
using System.Threading;
using System.Threading.Tasks;

using CM;
using AS;
using FireStoreScript;
using Firebase.Firestore;
using Firebase.Extensions;

using UnityEngine.Networking;
using Prefebs;
using CLCM;

namespace GameChatSample
{
    public class NewChatManager : MonoBehaviour
    {

        //�ʿ� ���� ����
        [Header("��Ī")]
        //��Ī ��Ÿ��
        public bool canMatching = true;




        [Header("ä�ù� �̸� ���� ����")]
        public TextAsset CRnameCSVfile;
        public static string newCRname = "";
        public static int tableSize = 104;
        string randomCRN;
        //�� Ŭ������ ������� �迭 ���� ����
        public static CRNList ChatRoomNameList = new CRNList();

        //�� ���� ������ Ŭ���� ����
        //[System.Serializable]
        [SerializeField]
        public class chatRoomName
        {
            public string Adjective;
            public string Noun;
        }

        [SerializeField]
        public class CRNList
        {
            public chatRoomName[] CRN;
        }
      

        //ä�ø���Ʈ �޾ƿͼ� id�� �������� ���� ����Ʈ, �̸� �ʱ�ȭ
        public static string currentCRname = "";
        public static List<Channel> CList = new List<Channel>();


        //chatmanager�� �����޾ƿ��� ���� ����
        public ChatManager chatManager;

        public static string CCName;
        public static string[] CurChatInfo = new string[5];



        //��Ī�� ���� �����
        public static string username;
        public int usersex;
        public string docID; //��ť��Ʈ ����ID �����ϱ� ���� �ʿ�
        public string channelID; //ä�ù� ���� ����
        public int countMembers;
        public int fcount;
        public int mcount;
        public ListenerRegistration listener;
        public ListenerRegistration listener2;

        public static Dictionary<string, object> newChatRoom = new Dictionary<string, object>();

        //��Ī�� ���� ��� �����
        public static string GAMECHAT_ROOM = "gameChatRoom";
        public static string ISOPEN = "isOpen";
        public static string ISACTIVE = "isActive";
        public static string MEMBER = "member";
        public static string NICKNAME = "nickName";
        public static string SEX = "sex";
        public static string CHANNELID = "channelID";
        public static string CREATETIME = "createTime";
        public static string OPENTIME = "openTime";

        // Ÿ�̸� �ڵ带 ���� ���� ����
        public float time_current = 5f;
        public GameObject Dialog_Matching_ReMatching; //��Ī���н� ���� ���̾�α�

        

        //ä�ù� �̸� ����üũ
        public static string nowChatName;
        public static bool isDouble;

        // Start is called before the first frame update
        void Start()
        {
            //don't destroy ó��
            DontDestroyOnLoad(this.gameObject);

            //ä�ù� �̸� ���� ������ ���� �ؽ�Ʈ �Ľ� �Լ� ȣ��
            ReadCSV();

            //��ť��Ʈ id�ʱ�ȭ�� ���� ����// �ʿ���
            docID = "";
            //countMembers = 0; //ä�ù����� �ʱ�ȭ
            fcount = 0; //��������� �ʱ�ȭ
            mcount = 0; //��������� �ʱ�ȭ

            //��Ī ������ ���� �׽�Ʈ����
            username = PlayerPrefs.GetString("GCName");
            //username = "�ֺ�";
            usersex = FirebaseManager.sex;
            Debug.Log("���� �α��� ���� �г��� : " + username);
            Debug.Log("���� �α��� ���� ���� : " + usersex);
        }

        // Update is called once per frame
        void Update()
        {
            if (Prefebs.getGCID.clickCard == true)
            {
                GetMSG();
            }
        }

        //ä�ù� �̸� �������� �� ä�ù� ���� �Լ�
        //ä�ù� �̸� �ߺ�üũ ��� �߰� �ʿ�

        //���� ä�ù� �̸� ������ ���� csv���� �Ľ� �Լ�
        void ReadCSV()
        {
            //������ CSV������ ,�� ���ʹ����� �Ľ�
            string[] CSVdata = CRnameCSVfile.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);

            ChatRoomNameList.CRN = new chatRoomName[tableSize];

            for (int i = 0; i < tableSize - 1; i++)
            {
                ChatRoomNameList.CRN[i] = new chatRoomName();
                ChatRoomNameList.CRN[i].Adjective = (CSVdata[2 * (i + 1)]);
                ChatRoomNameList.CRN[i].Noun = (CSVdata[2 * (i + 1) + 1]);
            }
        }

        #region Matching
        public void matchingOn()
        {
            CollectionReference roomRef = FirebaseManager.db.Collection(GAMECHAT_ROOM); //ä�÷� �÷��� ����
            Query allroomRef = roomRef;
            allroomRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot allroomSnapshot = task.Result;
                foreach (DocumentSnapshot doc in allroomSnapshot.Documents)
                {
                    Dictionary<string, object> docDictionary = doc.ToDictionary();
                    docID = doc.Id; //ä�ù��̸� ����
                    channelID = docDictionary[CHANNELID].ToString(); //ä�ù���̵� ����

                    gameSceneManager.chatRname = docID; //�׷�ê ������ �ε��� ä�ù� �̸� ����
                    gameSceneManager.chatRID = channelID; //�׷�ê ������ �ε��� ä�� ID ����
                    

                    List<object> memberList = (List<object>)docDictionary[MEMBER];
                    string open = docDictionary[ISOPEN].ToString();
                    Debug.Log(docID + open);

                    if (open == "False")
                    {
                        fcount = 0;
                        mcount = 0;
                        countMembers = 0;
                        foreach (Dictionary<string, object> m in memberList)
                        {
                            if (m[SEX].ToString() == "1") { mcount++; }
                            else { fcount++; }  
                        }
                        Debug.Log("���� ������ " + mcount + "+ ���� ������ " + fcount);

                        Dictionary<string, object> addUser = new Dictionary<string, object>
                {
                    { NICKNAME , username },
                    { SEX , usersex }
                };
                        string isopen = "";
                        if (usersex == 2 && fcount <= 2)
                        {
                            roomRef.Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
                            fcount++;
                            Debug.Log("������:" + fcount);

                            countMembers = mcount + fcount;

                            Debug.Log("������Ʈ�� ��ü������" + countMembers);

                            if (countMembers == 6) //���� ������ �������� ���� �� 6���̸� ä�ù� ����/��Ƽ�긦 Ʈ��� �ٲ�
                            {
                                Debug.Log("6�� ä����");
                                //��ü�������� 6���̸� ä�ù��� ���� ���θ� true�� �ٲ�
                                roomRef.Document(docID).UpdateAsync(ISOPEN, true); //ä�ù� ����
                                roomRef.Document(docID).UpdateAsync(ISACTIVE, true); //ä�ù� Ȱ��ȭ
                                roomRef.Document(docID).UpdateAsync(OPENTIME, System.DateTime.Now.ToString()); //ä�ù� ���� �ð� ���
                                isopen = "True";
                            }
                            DocumentReference docRef = roomRef.Document(docID);
                            listener = docRef.Listen(snapshot =>
                            {
                                if (snapshot.Exists)
                                {
                                    Debug.Log("�ݹ�");
                                    if (isopen == "True") //ä�ù� ���� �� Ʈ���̸�
                                    {
                                        listener.Stop();
                                        Invoke("showChatRoom", 5f);
                                    }
                                }

                                else
                                {
                                    Debug.Log(string.Format("������ �������� �ʽ��ϴ�!", snapshot.Id));
                                    listener.Stop();
                                    Dialog_Matching_ReMatching.SetActive(true); //��Ī���� ���̾�α�
                                }

                            });

                            return;

                        }
                        else if (usersex == 1 && mcount <= 2)
                        {
                            roomRef.Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
                            mcount++;
                            Debug.Log("������" + mcount);

                            countMembers = mcount + fcount;

                            Debug.Log("������Ʈ�� ��ü������" + countMembers);

                            if (countMembers == 6) //���� ������ �������� ���� �� 6���̸� ä�ù� ����/��Ƽ�긦 Ʈ��� �ٲ�
                            {
                                Debug.Log("6�� ä����");
                                //��ü�������� 6���̸� ä�ù��� ���� ���θ� true�� �ٲ�
                                roomRef.Document(docID).UpdateAsync(ISOPEN, true); //ä�ù� ����
                                roomRef.Document(docID).UpdateAsync(ISACTIVE, true); //ä�ù� Ȱ��ȭ
                                roomRef.Document(docID).UpdateAsync(OPENTIME, System.DateTime.Now.ToString()); //ä�ù� ���� �ð� ���
                                isopen = "True";
                            }

                            DocumentReference docRef = roomRef.Document(docID);
                            listener = docRef.Listen(snapshot =>
                            {
                                if (snapshot.Exists)
                                {
                                    Debug.Log("�ݹ�");
                                    if (isopen == "True") //ä�ù� ���� �� Ʈ���̸�
                                    {
                                        listener.Stop();
                                        Invoke("showChatRoom", 5f);
                                    }
                                }

                                else
                                {
                                    Debug.Log(string.Format("������ �������� �ʽ��ϴ�!", snapshot.Id));
                                    listener.Stop();
                                    Dialog_Matching_ReMatching.SetActive(true); //��Ī���� ���̾�α�
                                }

                            });

                            return;
                        }
                    }
                }

                //makeNewRoom();
                Debug.Log("���� �ִ¹� ����");
                makeChatRoomName();
                return;
            });

        }

        void showChatRoom()
        {
            gameSceneManager.LoadScene_GroupChat();

        }

        void makeNewRoom() //ä�ù� ����
        {
            StartCoroutine("CreateChatR");
        }
        
        async void makeNewRoom2()
        {
            channelID = newChatRoom["ChannelID"].ToString(); //ä�ù�ID ����
            docID = newChatRoom["ChannelName"].ToString(); //ä�ù��̸� ����
            gameSceneManager.chatRname = docID;
            gameSceneManager.chatRID = channelID;
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
            await addmrRef.SetAsync(room);
            await FirebaseManager.db.Collection(GAMECHAT_ROOM).Document(docID).UpdateAsync(MEMBER, FieldValue.ArrayUnion(addUser));
            Debug.Log("ä�ù� ���� ������");

            listener2 = addmrRef.Listen(snapshot =>
            {
                if (snapshot.Exists)
                {
                    Dictionary<string, object> doc = snapshot.ToDictionary();
                    string isopen = doc[ISOPEN].ToString();
                    //gameSceneManager.IDoTime.Add(doc[CHANNELID].ToString(), doc[ISOPEN].ToString());
                    Debug.Log(isopen);
                    Debug.Log("���ο� ���� ������Ʈ");
                    if (isopen == "True")
                    {
                        listener2.Stop();
                        Invoke("showChatRoom", 5f);
                    }
                }
                else
                {
                    Debug.Log(string.Format("���� ������ ������ �������� �ʽ��ϴ�!", snapshot.Id)); //���Ī �õ��ؾߵ�
                    listener2.Stop();
                    Dialog_Matching_ReMatching.SetActive(true);
                    newChatRoom.Clear();//ä��id�� ä�ù��̸� ������ ��ųʸ� �ʱ�ȭ
                }

            });
        }


        #endregion


        //������ ����� + ��� 7�� ������ ä�ù� �̸� ���� �Լ�
        //��Ʈ�� Ÿ���� (adj + " " + noun)�� ��ȯ "����� ��ĭ��� ���"
        public async Task makeChatRoomName()
        {
            while (true)
            {
                int AdjNum = UnityEngine.Random.Range(1, tableSize);
                int NounNum = UnityEngine.Random.Range(1, tableSize);

                string adj = ChatRoomNameList.CRN[AdjNum].Adjective;
                string noun = ChatRoomNameList.CRN[NounNum].Noun;

                int adjNum = adj.Length;
                int nounNum = noun.Length;
                Debug.Log("���������" + (adjNum + nounNum + 1));
                //Debug.Log(adjNum);
                //Debug.Log(nounNum);

                if (adjNum + nounNum + 1 < 8)
                {
                    await CRdoubleCheck(adj + " " + noun);
                    Debug.Log("�Լ� ���� �� ######" + isDouble.ToString());
                    
                    if (isDouble == false)
                    {

                        Debug.Log("���� ���� ������ "+adj + " " + noun);
                        nowChatName = (adj + " " + noun);
                        CallCreatCR();
                        break;

                    }
                    else if(isDouble ==true)
                    {
                        Debug.Log("######���� ä�ù��̸��̶� �ٽ� ȣ��");
                    }

                    
                }
            }


        }

        public static async Task CRdoubleCheck(string name)
        {
            DocumentReference docRef = FirebaseManager.db.Collection(GAMECHAT_ROOM).Document(name);
            await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists) 
                {
                    isDouble = true; //�ش� �̸��� �̹� ����
                    Debug.Log("ä�ù� �̸� �̹� ���� : " + name);
                    Debug.Log(isDouble.ToString());
;               }
                else
                {
                    isDouble = false;
                    Debug.Log("ä�ù� �̸� ����");
                }
            });
        }

        public void CallCreatCR()
        {
            StartCoroutine("CreateChatR");
        }

        IEnumerator CreateChatR()
        {
            string url = "https://dashboard-api.gamechat.naverncp.com/v1/api/project/e3558324-2d64-47d0-bd7a-6fa362824bd7/channel";
            string APIKey = "ec31cc21b559da9eb19eaec2dadcd50ed786857a740a561d";



            WWWForm form = new WWWForm();
            //makeChatRoomName();
            Debug.Log("####���� �̸��ֳ�?####     " + nowChatName);
            //string projectID = "e3558324 - 2d64 - 47d0 - bd7a - 6fa362824bd7";
            //form.AddField("projectId", projectID);
            form.AddField("name", nowChatName);


            //�� url ��û�� POST ��û
            UnityWebRequest www = UnityWebRequest.Post(url, form);

            www.SetRequestHeader("x-api-key", APIKey);
            //www.SetRequestHeader("content-type", "application/json");

            //��û�� ���� ������ ��ٸ�
            yield return www.SendWebRequest();

            //Debug.Log("���� ������ ä�ù� ���̵�� : " + www.result);
            //result���� success��� �ߴ� ��Ȳ

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("����");
                string result = www.downloadHandler.text.Substring(22, 36);

                newChatRoom.Add("ChannelID", result);//ä�� id
                newChatRoom.Add("ChannelName", nowChatName);//ä�ù��̸�
                Debug.Log(result);
                Debug.Log(nowChatName);

                makeNewRoom2();
                
            }
        }
        
        public void sendMSG()
        {
            GameChat.sendMessage(CList[0].id, "�޽��� ����");
        }

        public void GetMSG()
        {
            chatlistSceneManager.isEqualName();
            gameSceneManager.LoadScene_GroupChat();
            Prefebs.getGCID.clickCard = false;

        }

        
        public static int CountingNewMSG(string Channel_ID)
        {
            GameChat.getMessages(Channel_ID, 0, 1, "", "", "");
            //ä��id�� �޾ƿͼ� �ֱ� �޽��� ���� ��, ������ ī����
            int newMSGint = 0;
            return newMSGint;

        }

        public static int count;
        public static int setNewMSGCount(string id)
        {
            GameChat.getMessages(id, 0, 100, "", "", "", (List<Message> Messages, GameChatException Exception) =>
            {

                if (Exception != null)
                {
                    // Error �ڵ鸵
                    return;
                }

                int count = 0;
                foreach (Message elem in Messages)
                {
                    string curMsgID = elem.message_id;
                    if (curMsgID != PlayerPrefs.GetString("LastMSGID"))
                    {
                        count += 1;
                        Debug.Log("������ �޽���: " + count);
                    }
                }

                CurChatInfo[4] = count.ToString();
            });
            return count;
        }


        public static string curMsg = "";
        public static void getCurMSG(string id, System.Action<string, string> callBack)
        {
            GameChat.getMessages(id, 0, 1, "", "", "", (List<Message> Messages, GameChatException Exception) =>
            {

                if (Exception != null)
                {
                    // Error �ڵ鸵
                    
                }

                foreach (Message elem in Messages)
                {
                    curMsg = elem.content;
                    Debug.Log(elem.content);

                }

                callBack(id, curMsg);
                Debug.Log("getCurMSG-Msg: " + curMsg);
            });    
            
        }

        public static string curRoomName = "";
        public static string getCurRoomName(string id)
        {
            Debug.Log("getCurRoomName-id : " + id);
            GameChat.getChannel(id, null, (Channel channel, GameChatException Exception) =>
            {
                Debug.LogError("#### id ###: " + id);
                Debug.LogError("#### channel ###: " + channel);
                if (Exception != null)
                {
                    Debug.Log(Exception.message);
                    Debug.Log(Exception.code);
                    //���� �ڵ鸵
                    Debug.Log("get channel ����");
                    return;
                }
                CurChatInfo[0] = channel.name;
                Debug.Log(channel.created_at);


                CurChatInfo[2] = channel.created_at.Substring(11, 8).ToString();
                CurChatInfo[3] = channel.created_at.Substring(0, 10).ToString();

                //getLostTime();


            });

            return curRoomName;
        }





        public static string getLostTime(string openTime)
        {
            
            TimeSpan goTime = DateTime.Now - Convert.ToDateTime(openTime);
            
            if (goTime.Days <= 0)
            {
                if (goTime.Hours == 0)
                {
                    if (goTime.Minutes > 20)
                    {
                        //CurChatInfo[5] = "����";
                        return "����";
                    }
                    else
                    {
                        return (20 - goTime.Minutes).ToString() + "��";
                        //CurChatInfo[5] = (20 - goTime.Minutes).ToString() + "��";
                        //20�п��� ��� �ð� ���ֱ�
                    }
                }
                else
                {
                    //CurChatInfo[5] = "����";
                    return "����";
                }

            }
            else
            {
                return "����";
                //CurChatInfo[5] = "����";
            }

        }

    }
}