using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameChatUnity;
using GameChatUnity.SimpleJSON;
using GameChatUnity.SocketIO;
//using GameChatUnity.Extention;

using UnityEngine.Networking;

namespace GameChatSample {
    public class NewChatManager : MonoBehaviour
    {

        //�ʿ� ���� ����
        [Header("��Ī")]
        //��Ī ��Ÿ��
        public bool canMatching = true;

        //RTDB�� �� ���� ������ Ŭ����ȭ
        public class UserInfo
        {
            public string uid = "";
            public string sex = "";
            public bool isMatching = false; //��Ī������ üũ
            public List<string> RuidList = new List<string>();
        }


        [Header("ä�ù� �̸� ���� ����")]
        public TextAsset CRnameCSVfile;
        public string newCRname = "";
        public int tableSize = 104;
        string randomCRN;
        //�� Ŭ������ ������� �迭 ���� ����
        public CRNList ChatRoomNameList = new CRNList();

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
        //�׽�Ʈ�� ��ư
        public Button CreatChatBtn;

        //ä�ø���Ʈ �޾ƿͼ� id�� �������� ���� ����Ʈ, �̸� �ʱ�ȭ
        public string currentCRname = "";
        public List<Channel> CList = new List<Channel>();


        //ä�ù� �̸��� ������ UI �ؽ�Ʈ ����
        public Text ThisCRoomNameTitle;

        //chatmanager�� �����޾ƿ��� ���� ����
        public ChatManager chatManager;

        //�� ��ǳ�� ��� ��ǳ�� ������ ���� ���� ����
        public GameObject MyArea, ElseArea;
        public RectTransform ContentRect;
        //�ҷ������� �޽����� ������ �޽����� ������ �Ǻ��ϱ� ���� �������� ����
        public Message LastMSG;
        //�����޽����� �ð��� �޾ƿ��� ���� ���� ����
        public Message xMSG;






        // Start is called before the first frame update
        void Start()
        {
            //don't destroy ó��
            DontDestroyOnLoad(this.gameObject);




            //ä�ù� �̸� ���� ������ ���� �ؽ�Ʈ �Ľ� �Լ� ȣ��
            ReadCSV();

        }

        // Update is called once per frame
        void Update()
        {

        }


        //ä�ù� �̸� �������� �� ä�ù� ���� �Լ�
        //ä�ù� �̸� �ߺ�üũ ��� �߰� �ʿ�
        #region
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

        //������ ����� + ��� 7�� ������ ä�ù� �̸� ���� �Լ�
        //��Ʈ�� Ÿ���� (adj + " " + noun)�� ��ȯ "����� ��ĭ��� ���"
        public string makeChatRoomName()
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
                    Debug.Log(adj + " " + noun);
                    return (adj + " " + noun);
                }
            }
        }


        //ä�ù� �̸��� �����Ͽ� ���ο� ä���� �����ϴ� �Լ�
        public void CallCreatCR()
        {
            StartCoroutine(CreateChatR());
        }

        public IEnumerator CreateChatR()
        {
            string url = "https://dashboard-api.gamechat.naverncp.com/v1/api/project/e3558324-2d64-47d0-bd7a-6fa362824bd7/channel";
            string APIKey = "ec31cc21b559da9eb19eaec2dadcd50ed786857a740a561d";



            WWWForm form = new WWWForm();
            string name = makeChatRoomName();
            //string projectID = "e3558324 - 2d64 - 47d0 - bd7a - 6fa362824bd7";
            //form.AddField("projectId", projectID);
            form.AddField("name", name);


            //�� url ��û�� POST ��û
            UnityWebRequest www = UnityWebRequest.Post(url, form);

            www.SetRequestHeader("x-api-key", APIKey);
            //www.SetRequestHeader("content-type", "application/json");

            //��û�� ���� ������ ��ٸ�
            yield return www.SendWebRequest();

            Debug.Log("���� ������ ä�ù� ���̵�� : " + www.result);
            //result���� success��� �ߴ� ��Ȳ

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("����");

                //DB�� ���� �����Ϳ� �ش� ä�ù� �ڵ�� �����ϴ� ���� �߰� �ʿ�
                Debug.Log(www.downloadHandler.text);
                string result = www.downloadHandler.text;
                Debug.Log(result);
                result = result.Substring(22, 36);
                GameChat.subscribe(result);
            }
        }

        #endregion

        //ä�ù� ���� �������� �Լ�
        #region

        public void getChannelID()
        {

            GameChat.getChannels(0, 1, (List<Channel> Channels, GameChatException Exception) =>
            {
                if (Exception != null)
                {
                    Debug.Log(Exception.message);
                    Debug.Log(Exception.code);
                    //���� �ڵ鸵
                    Debug.Log("get channel ����");
                    return;
                }

                foreach (Channel elem in Channels)
                {
                    Debug.Log("get channel ����");
                    CList.Add(elem);
                    for (int i = 0; i < CList.Count; i++)
                    {
                        Debug.Log(i);
                        Debug.Log("��ä�� ������ ����Ʈ�� id : " + CList[i].id);
                        currentCRname = CList[i].id.ToString();
                        Debug.Log("��ä�� ������ ����Ʈ�� rawJson : " + CList[i].rawJson);

                    }
                }
            });
            //Debug.Log(" CList[0].id: " + CList[0].id);
        }

        public void sendMSG()
        {
            GameChat.sendMessage(CList[0].id, "�޽��� ����");
        }

        public void GetMSG()
        {
            if (null != GameObject.Find("ThisRoomName"))
            {
                ThisCRoomNameTitle = GameObject.Find("ThisRoomName").GetComponent<Text>();
                ThisCRoomNameTitle.text = CList[0].name;
            }
            else
            {
                Debug.Log("�� ���� ã�� ����");
            }

            if (null != GameObject.Find("GContent"))
            {
                ContentRect = GameObject.Find("GContent").GetComponent<RectTransform>();
            }
            else
            {
                Debug.Log("GContentã�� ����");
            }

            StartCoroutine("TestMSG");
            /*

            GameChat.getMessages(CList[0].id, 0, 200, "", "", "asc", (List<Message> Messages, GameChatException Exception) => {

                if (Exception != null)
                {
                    // Error �ڵ鸵
                    return;
                }


                foreach (Message elem in Messages)
                {
                    if (elem.sender.id == SampleGlobalData.G_User.id)
                    {
                        //���� ��ǳ�� ����
                        AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                        Area.transform.SetParent(ContentRect.transform, false);
                        Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                        Area.TextRect.GetComponent<Text>().text = elem.content;
                        Debug.Log(elem.sender.name + "(��): " + elem.content);
                        Area.User = elem.sender.name;
                        //Area.UserText.text = elem.content;
                        chatManager.Fit(Area.BoxRect);
                        chatManager.Fit(Area.AreaRect);
                        chatManager.Fit(ContentRect);


                    }
                    else
                    {
                        //Ÿ���� ��ǳ�� ����
                        AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                        Area2.transform.SetParent(ContentRect.transform, false);
                        Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                        Area2.TextRect.GetComponent<Text>().text = elem.content;
                        Debug.Log(elem.sender.name + "(Ÿ��): " + elem.content);
                        Area2.User = elem.sender.name;
                        Area2.UserText.text = elem.sender.name;
                        chatManager.Fit(Area2.BoxRect);
                        chatManager.Fit(Area2.AreaRect);
                        chatManager.Fit(ContentRect);
                    }
                }
            });*/
        }

        public IEnumerator TestMSG()
        {
            //������ ä���� �޾ƿ�
            Debug.Log(CList[0].id);
            GameChat.getMessages(CList[0].id, 0, 1, "", "", "", (List<Message> Messages, GameChatException Exception) =>
            {

                if (Exception != null)
                {
                    // Error �ڵ鸵
                    return;
                }


                foreach (Message elem in Messages)
                {
                    LastMSG = elem;
                    //Debug.Log(LastMSG);
                }
            });


            GameChat.getMessages(CList[0].id, 0, 100, "", "", "asc", (List<Message> Messages, GameChatException Exception) =>
            {

                if (Exception != null)
                {
                    // Error �ڵ鸵
                    return;
                }

                int count = 0;
                foreach (Message elem in Messages)
                {
                    if (LastMSG.message_id != elem.message_id)
                    {
                        if (count == 0)
                        {
                            xMSG = elem;
                            Debug.LogError("����: " + xMSG.content);
                            if (SampleGlobalData.G_User.id == elem.sender.id)//��
                            {
                                Debug.Log("��_�ð�����_����");
                                //�޽����� �ð��� ǥ��
                                AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                                Area.transform.SetParent(ContentRect.transform, false);
                                Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                                Area.TextRect.GetComponent<Text>().text = elem.content;
                                Debug.Log(elem.content);
                                Area.TimeText.text = elem.created_at;

                                //�ؽ�Ʈ�� ���� �̻��� ��� ó��
                                float X = Area.TextRect.sizeDelta.x + 400;
                                float Y = Area.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area.BoxRect);

                                        if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area.BoxRect.sizeDelta = new Vector2(X, Y);

                            }
                            else
                            {
                                Debug.Log("Ÿ��_�ð�����_����");
                                //�޽���, �̸�, �ð�ǥ��
                                AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                                Area2.transform.SetParent(ContentRect.transform, false);
                                Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                                Area2.TextRect.GetComponent<Text>().text = elem.content;
                                Area2.UserText.text = elem.sender.name;
                                Area2.TimeText.text = elem.created_at;

                                //�ؽ�Ʈ�� ���� �̻��� ��� ó��
                                float X = Area2.TextRect.sizeDelta.x + 400;
                                float Y = Area2.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area2.BoxRect.sizeDelta = new Vector2(X - i * 2, Area2.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area2.BoxRect);

                                        if (Y != Area2.TextRect.sizeDelta.y) { Area2.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area2.BoxRect.sizeDelta = new Vector2(X, Y);

                            }
                            count = 1;
                        }
                        else
                        {
                            if (xMSG.created_at == elem.created_at)
                            {

                                //���� �޽����� �ð��� ������
                                if (SampleGlobalData.G_User.id == elem.sender.id)//��
                                {
                                    Debug.Log("��_�ð�����");
                                    //�޽����� ǥ��
                                    AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                                    Area.transform.SetParent(ContentRect.transform, false);
                                    Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                                    Area.TextRect.GetComponent<Text>().text = elem.content;
                                    Area.TimeText.text = "";
                                    Debug.Log("Ÿ�� �ð����� :  " + (xMSG.created_at.ToString() == elem.created_at.ToString()));

                                    //�ؽ�Ʈ�� ���� �̻��� ��� ó��
                                    float X = Area.TextRect.sizeDelta.x + 400;
                                    float Y = Area.TextRect.sizeDelta.y;
                                    if (Y > 700)
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                                            chatManager.Fit(Area.BoxRect);

                                            if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                        }
                                    }
                                    else Area.BoxRect.sizeDelta = new Vector2(X, Y);

                                }
                                else
                                {
                                    Debug.Log("Ÿ��_�ð�����");
                                    //�޽���, �̸� ǥ��
                                    AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                                    Area2.transform.SetParent(ContentRect.transform, false);
                                    Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                                    Area2.TextRect.GetComponent<Text>().text = elem.content;
                                    Area2.UserText.text = elem.sender.name;
                                    Area2.TimeText.text = "";
                                    Debug.Log("Ÿ�� �ð����� :  " + (xMSG.created_at.ToString() == elem.created_at.ToString()));


                                    //�ؽ�Ʈ�� ���� �̻��� ��� ó��
                                    float X = Area2.TextRect.sizeDelta.x + 400;
                                    float Y = Area2.TextRect.sizeDelta.y;
                                    if (Y > 700)
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            Area2.BoxRect.sizeDelta = new Vector2(X - i * 2, Area2.BoxRect.sizeDelta.y);
                                            chatManager.Fit(Area2.BoxRect);

                                            if (Y != Area2.TextRect.sizeDelta.y) { Area2.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                        }
                                    }
                                    else Area2.BoxRect.sizeDelta = new Vector2(X, Y);
                                }
                            }
                            else
                            {

                                if (SampleGlobalData.G_User.id == elem.sender.id)//��
                                {
                                    Debug.Log("��_�ð�����");
                                    //��: �޽����� �ð��� ǥ��
                                    AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                                    Area.transform.SetParent(ContentRect.transform, false);
                                    Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                                    Area.TextRect.GetComponent<Text>().text = elem.content;
                                    Area.TimeText.text = elem.created_at;

                                    //�ؽ�Ʈ�� ���� �̻��� ��� ó��
                                    float X = Area.TextRect.sizeDelta.x + 400;
                                    float Y = Area.TextRect.sizeDelta.y;
                                    if (Y > 700)
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                                            chatManager.Fit(Area.BoxRect);

                                            if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                        }
                                    }
                                    else Area.BoxRect.sizeDelta = new Vector2(X, Y);
                                }
                                else
                                {
                                    Debug.Log("Ÿ��_�ð�����");
                                    //�޽���, �̸�, �ð�ǥ��
                                    AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                                    Area2.transform.SetParent(ContentRect.transform, false);
                                    Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                                    Area2.TextRect.GetComponent<Text>().text = elem.content;
                                    Area2.UserText.text = elem.sender.name;
                                    Area2.TimeText.text = elem.created_at;

                                    //�ؽ�Ʈ�� ���� �̻��� ��� ó��
                                    float X = Area2.TextRect.sizeDelta.x + 400;
                                    float Y = Area2.TextRect.sizeDelta.y;
                                    if (Y > 700)
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            Area2.BoxRect.sizeDelta = new Vector2(X - i * 2, Area2.BoxRect.sizeDelta.y);
                                            chatManager.Fit(Area2.BoxRect);

                                            if (Y != Area2.TextRect.sizeDelta.y) { Area2.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                        }
                                    }
                                    else Area2.BoxRect.sizeDelta = new Vector2(X, Y);
                                }
                            }
                            //���� �޽��� �ʱ�ȭ
                            xMSG = elem;
                            Debug.LogError("����: " + xMSG.content);
                        }
                    }
                    else
                    {
                        if (SampleGlobalData.G_User.id == elem.sender.id)
                        {
                            if (xMSG.created_at == elem.created_at)
                            {
                                //��_�ð�����
                                AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                                Area.transform.SetParent(ContentRect.transform, false);
                                Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                                Area.TextRect.GetComponent<Text>().text = elem.content;
                                

                                //�ؽ�Ʈ�� ���� �̻��� ��� ó��
                                float X = Area.TextRect.sizeDelta.x + 400;
                                float Y = Area.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area.BoxRect);

                                        if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area.BoxRect.sizeDelta = new Vector2(X, Y);
                            }
                            else
                            {
                                //��_�ð�����
                                AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                                Area.transform.SetParent(ContentRect.transform, false);
                                Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                                Area.TextRect.GetComponent<Text>().text = elem.content;
                                Area.TimeText.text = elem.created_at;

                                //�ؽ�Ʈ�� ���� �̻��� ��� ó��
                                float X = Area.TextRect.sizeDelta.x + 400;
                                float Y = Area.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area.BoxRect);

                                        if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area.BoxRect.sizeDelta = new Vector2(X, Y);
                            }
                        }
                        else
                        {
                            if (xMSG.created_at == elem.created_at)
                            {
                                //Ÿ��_�ð�����
                                AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                                Area2.transform.SetParent(ContentRect.transform, false);
                                Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                                Area2.TextRect.GetComponent<Text>().text = elem.content;
                                Area2.UserText.text = elem.sender.name;
                               

                                //�ؽ�Ʈ�� ���� �̻��� ��� ó��
                                float X = Area2.TextRect.sizeDelta.x + 400;
                                float Y = Area2.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area2.BoxRect.sizeDelta = new Vector2(X - i * 2, Area2.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area2.BoxRect);

                                        if (Y != Area2.TextRect.sizeDelta.y) { Area2.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area2.BoxRect.sizeDelta = new Vector2(X, Y);
                            }
                            else
                            {
                                //Ÿ��_�ð�����
                                AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                                Area2.transform.SetParent(ContentRect.transform, false);
                                Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                                Area2.TextRect.GetComponent<Text>().text = elem.content;
                                Area2.UserText.text = elem.sender.name;
                                Area2.TimeText.text = elem.created_at;

                                //�ؽ�Ʈ�� ���� �̻��� ��� ó��
                                float X = Area2.TextRect.sizeDelta.x + 400;
                                float Y = Area2.TextRect.sizeDelta.y;
                                if (Y > 700)
                                {
                                    for (int i = 0; i < 200; i++)
                                    {
                                        Area2.BoxRect.sizeDelta = new Vector2(X - i * 2, Area2.BoxRect.sizeDelta.y);
                                        chatManager.Fit(Area2.BoxRect);

                                        if (Y != Area2.TextRect.sizeDelta.y) { Area2.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
                                    }
                                }
                                else Area2.BoxRect.sizeDelta = new Vector2(X, Y);
                            }
                        }
                    }
                }
            });
            yield return null;

            yield return null;

        }

       

        #endregion
    }
}
