using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameChatSample;
using System.Threading.Tasks;
using FireStoreScript;
using Firebase.Firestore;
using Firebase.Extensions;
using Prefebs;

namespace CLCM
{
    public class chatlistSceneManager : MonoBehaviour
    {
        //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
        gameSceneManager gSM;
        public Button backToHome;

        //�׷�ä�� ī�忡 �� ���ӿ�����Ʈ ����
        public GameObject CListUI;

        //�׷�ä�ÿ� �� ���� �޾ƿ� ���� ����
        public string GetID = "";//�޾ƿ�channelID
        public string RoomTitleText = "����Ÿ��Ʋ";
        public string RoomTimerText = "00:00";
        public string RoomContentsText = "������������ �Է�";
        public int RoomNewMSGCount = 0;
        public string userNickName;


        public string id = "";

        //public static string Previous_Canvas;

        private void Awake()
        {

        }
        // Start is called before the first frame update
        void Start()
        {
            //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
            gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

            //Invoke("makeGCList", 0.03f);
            //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
            backToHome.onClick.AddListener(gSM.LoadScene_Home);

            //makeGCList();

            //object cRname = "������ ���";
            //FireStoreScript.FirebaseManInager.CRnameDoubleCheck(cRname);

            InvokeRepeating("gotMyGClistInfo", 0f, 5f);
            userNickName = PlayerPrefs.GetString("GCName");
    }



        // Update is called once per frame
        void Update()
        {
            if (Application.platform == RuntimePlatform.Android)  // �÷��� ���� .
            {
                if (Input.GetKey(KeyCode.Escape)) // Ű ���� �ڵ� ��ȣ�� �޾ƿ��°�.
                {
                    SceneManager.LoadScene("Home"); // ������ �̵� .
                                                    //Application.Quit(); // �� ���� .(������)            �������� �̵��̳� ������ �����ϳ� ���Ͻô°��� ����Ͻø� �˴ϴ�.
                }
            }

        }


        //DB "gameChatRoom" Collection�� �����ؼ� Document�� member�� nickName�� �����Ͽ�
        //���� �г��Ӱ� ������ ���� �ִ� Document�� id, ä��id, openTime�� ������
        //������ ä��id�� �ֱ� �޽����� ������
        //PlayerPrefs.GetString("GCN"); //���� ������ ��, ���� ������ �ε��ϴ� ������ ��ģ �� �� ������ ����ؾ���.
        public static List<string[]> gSlotList = new List<string[]>();
        public Dictionary<string, Text> gSlotMsgDict = new Dictionary<string, Text>();
        public string[] cInfoList;


        public void makeGCcard()
        {
            //Debug.Log("makeGCard��");
            gotMyGClistInfo();
        }

        public async Task gotMyGClistInfo()
        {
            gSlotList.Clear();
            gSlotMsgDict.Clear();
            Query alldocQauery = FirebaseManager.db.Collection("gameChatRoom");

            QuerySnapshot alldocQauerySnapshot = await alldocQauery.GetSnapshotAsync();
            foreach (DocumentSnapshot docSnapShot in alldocQauerySnapshot.Documents)
            {
                Dictionary<string, object> docDictionary = docSnapShot.ToDictionary();
                List<object> memberList = (List<object>)docDictionary["member"];

                foreach (Dictionary<string, object> m in memberList)
                {

                    if (m["nickName"].ToString() == userNickName)
                    {

                        cInfoList = new string[3];

                        cInfoList[0] = docSnapShot.Id;

                        //Debug.Log("��Ÿ��"+docDictionary["openTime"]);

                        cInfoList[1] = NewChatManager.getLostTime(docDictionary["openTime"] as string);

                        Debug.Log("DB�ð� �ؽ�Ʈ�� �ε�: "+NewChatManager.getLostTime(docDictionary["openTime"] as string));

                        cInfoList[2] = docDictionary["channelID"] as string;// NewChatManager.curMsg;
                        //Debug.Log(NewChatManager.getCurMSG(docDictionary["channelID"] as string));
                        gSlotList.Add(cInfoList);
                    }
                }
            }
            //Debug.Log("gotmygclistinfo����");
            makeGCcardList();
        }

        public void makeGCcardList()
        {
            //Debug.Log("makeGCcardList����");
            //Debug.LogError(GameObject.Find("GCViewport").transform.childCount);
            if (0 < GameObject.Find("GCViewport").transform.childCount)
            {
                for (int n = 0; n < (GameObject.Find("GCViewport").transform.childCount); n++)
                {
                    GameObject.Destroy(GameObject.Find("GCViewport").transform.GetChild(n).gameObject);
                }
            }

            for (int i = 0; i < gSlotList.Count; i++)
            {
                //Debug.Log(gSlotList[i][0] + "/" + gSlotList[i][1] + "/" + gSlotList[i][2]);

                //������ ���� �� ����Ʈ�� �ڽ����� ����
                GameObject ui = Instantiate(CListUI, GameObject.Find("GCViewport").GetComponent<RectTransform>());
                ui.transform.SetParent(GameObject.Find("GCViewport").transform);

                //ä�ù� �̸� �ֱ�
                ui.transform.GetChild(3).GetComponent<Text>().text = gSlotList[i][0];

                //���� �ð� �ֱ�
                ui.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = gSlotList[i][1];
                

                //�ֱ� �޽��� �ֱ�
                ui.transform.GetChild(2).GetComponent<Text>().text = "";// gSlotList[i][2];
                

                gSlotMsgDict.Add(gSlotList[i][2], ui.transform.GetChild(2).GetComponent<Text>());
                NewChatManager.getCurMSG(gSlotList[i][2], OnRecvMsg);

            }
            //Debug.Log("makeGCcardList�٤Ѥ���");
        }
        //public Text badge;
        //public int badgeN = 0;


        //�����߰�_220905 1:1ä��
        public void makePCcardList()
        {
            //Debug.Log("makePCcardList����");
            //Debug.LogError(GameObject.Find("PCViewport").transform.childCount);
            if (0 < GameObject.Find("PCViewport").transform.childCount)
            {
                for (int n = 0; n < (GameObject.Find("PCViewport").transform.childCount); n++)
                {
                    GameObject.Destroy(GameObject.Find("PCViewport").transform.GetChild(n).gameObject);
                }
            }

            for (int i = 0; i < pSlotList.Count; i++)
            {
                //Debug.Log(gSlotList[i][0] + "/" + gSlotList[i][1] + "/" + gSlotList[i][2]);

                //������ ���� �� ����Ʈ�� �ڽ����� ����
                GameObject ui = Instantiate(CListUI, GameObject.Find("GCViewport").GetComponent<RectTransform>());
                ui.transform.SetParent(GameObject.Find("GCViewport").transform);

                //ä�ù� �̸� �ֱ�
                ui.transform.GetChild(3).GetComponent<Text>().text = gSlotList[i][0];

                //���� �ð� �ֱ�
                ui.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = gSlotList[i][1];


                //�ֱ� �޽��� �ֱ�
                ui.transform.GetChild(2).GetComponent<Text>().text = "";// gSlotList[i][2];


                gSlotMsgDict.Add(gSlotList[i][2], ui.transform.GetChild(2).GetComponent<Text>());
                NewChatManager.getCurMSG(gSlotList[i][2], OnRecvMsg);

            }
            //Debug.Log("makeGCcardList�٤Ѥ���");
        }



        void OnRecvMsg(string id, string msg)
        {
            gSlotMsgDict[id].text = msg;
           
        }


        public static void isEqualName()
        {
            for (int i = 0; i < gSlotList.Count; i++)
            {
                if (gSlotList[i][0] == getGCID.thisRoomName)
                {
                    gameSceneManager.chatRID = gSlotList[i][2];
                    //Debug.Log("#################����ID ::" + gSlotList[i][2]);
                    gameSceneManager.chatRname = gSlotList[i][0];
                    //Debug.Log("#################����ä�ù��̸� ::" + gSlotList[i][0]);
                    

                }
            }



            /*
            List<GameObject> gslotList = new List<GameObject>();
            public void makeRList()
            {
                GameObject ui = Instantiate(CListUI, GameObject.Find("GCViewport").GetComponent<RectTransform>());
                ui.transform.SetParent(GameObject.Find("GCViewport").transform);

                Debug.Log("### room_name ### : " + GameChatSample.NewChatManager.CurChatInfo[0]);
                ui.transform.GetChild(2).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[0]; //ä�ù� �̸�
                ui.transform.GetChild(4).transform.GetChild(1).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[2]; //�����ð�
                ui.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[4];//���� ��
                /*if (GameChatSample.NewChatManager.CurChatInfo[4] == "0") 
                {
                    ui.transform.GetChild(1).transform.gameObject.SetActive(false);
                }
                else
                {
                    ui.transform.GetChild(1).transform.gameObject.SetActive(true);
                }
                ui.transform.GetChild(3).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[1]; //��Ʈ ä�ó���

                //CurChatInfo[0]�̸� [1]���� [2]�����ð� [3]������¥ [4]���ο�޽�����

                gslotList.Add(ui);
            }

            public void GetCurrentChatCount(string id)
            {
                Debug.Log("GetCurrentChatCount���� id��? : " + id);
                //GameChatSample.NewChatManager.getChannelID();//firebase���� id�� �����;���. ������ id�� string���� ���� ����
                RoomNewMSGCount = GameChatSample.NewChatManager.setNewMSGCount(id);

            }

            public void getLostTime()
            {
                //string cTime = GameChatSample.NewChatManager.CList[0].created_at.Substring(11, 8);//�����ð�
                string cTime= GameChatSample.NewChatManager.CurChatInfo[2];
                //string cDay = GameChatSample.NewChatManager.CList[0].created_at.Substring(0, 10);
                string cDay = GameChatSample.NewChatManager.CurChatInfo[3];
                string nTime = DateTime.Now.ToString("u").Substring(11, 8);//����ð�
                string nDay = DateTime.Now.ToString("u").Substring(0, 10);
                Debug.Log(cTime + "/" + cDay + "/" + nTime + "/" + nDay);
                TimeSpan goTime = Convert.ToDateTime(nTime) - Convert.ToDateTime(cTime);

                if (goTime.Days <= 0) 
                {
                    if (goTime.Hours == 0)
                    {
                        if (goTime.Minutes > 20)
                        {+
            string
                            RoomTimerText = "����";
                        }
                        else
                        {
                            RoomTimerText = goTime.Minutes.ToString() + "��";
                        }
                    }
                    RoomTimerText = "����";


                }

            }

            public void makeGCList()
            {
                id = GameChatSample.NewChatManager.CList[0].id.ToString().Replace(" ","");
                RoomContentsText = GameChatSample.NewChatManager.getCurMSG(id);
                RoomTitleText = GameChatSample.NewChatManager.getCurRoomName(id);
                GetCurrentChatCount(id);//���� �� ���ϱ�


                Invoke("makeRList", 1f);


            }*/

        }

    }
}
