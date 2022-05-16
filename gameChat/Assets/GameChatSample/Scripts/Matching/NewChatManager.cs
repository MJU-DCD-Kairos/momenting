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

    public string currentCRname = "" ;
    public Button sendMSGBtn;
    public List<Channel> CList = new List<Channel>();



    // Start is called before the first frame update
    void Start()
    {
        //don't destroy ó��
        DontDestroyOnLoad(this.gameObject);
        

        //ä�ù� �̸� ���� ������ ���� �ؽ�Ʈ �Ľ� �Լ� ȣ��
        ReadCSV();
        getChannelID();
        Debug.Log("�ֱ� ä�ù�ID : "+ CList[0].id+"/ �̸�: "+CList[0].name);
        //GameChat.sendMessage("241ad48e-cb5c-4a55-8756-b595f30324bd", "���� �׽�Ʈ �޽��� ����");
        //GameChat.sendMessage(currentCRname, "CRN �׽�Ʈ �޽��� ����");
        sendMSG();

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

    public IEnumerator CreateChatRoom()
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
                for(int i=0; i<CList.Count; i++)
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
        
        GameChat.sendMessage(CList[0].id, "�׽�Ʈ �޽��� ����");
    }
    

    #endregion
}
