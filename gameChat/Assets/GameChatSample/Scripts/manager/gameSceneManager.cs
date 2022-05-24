using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using GameChatUnity;
using GameChatUnity.SimpleJSON;
using GameChatUnity.SocketIO;

public class gameSceneManager : MonoBehaviour
{
    //���÷��� Ÿ�̸� �ڵ带 ���� ���� ����
    private float time_current = 3f;
    private float time_Max = 3f;
    private bool isEnded;
    public GameObject title;
    public GameObject splash;

    public string creatChatCode;


    //ä�ø���Ʈ���� ä�� �޽��� �ε�� �Ѿ �� ����ϴ� ä�ù� �̸� �� ä�� ID
    public static string chatRname;
    public static string chatRID;



    //GameObject TodayQ;



    //ä�ù� �̸� �����Լ��� ���� ����
    //csv������ �ܺο��� �ν����Ϳ��� ���� ������ �� �ֵ��� ����_ä�ù� �̸� ����
    public TextAsset csvfile;

    //������ ������ �־��� UI�ؽ�Ʈ ������Ʈ�� �ν����ͷ� �����ޱ����� ����_ä�ù� �̸� �ؽ�Ʈ
    public Text ChatRoomNameText;

    ////CSV������ �� ������ �ν����ͻ󿡼� �Է��ϱ� ���� �ۺ� ���� ����_ä�ù� �̸��� ���� �۾�
    //public int tableSize;
    //string randomCRN;

    ////�� ���� ������ Ŭ���� ����
    //[System.Serializable]
    //public class chatRoomName
    //{
    //    public string Adjective;
    //    public string Noun;
    //}

    ////����Ʈ�� ������ Ŭ���� ����
    //[System.Serializable]
    //public class CRNList
    //{
    //    public chatRoomName[] CRN;
    //}

    ////�� Ŭ������ ������� �迭 ���� ����
    //public CRNList ChatRoomNameList = new CRNList();





    // Start is called before the first frame update
    void Start()
    {
        //���Ŵ��� �ı� ������ ���� �ڵ�
        DontDestroyOnLoad(this.gameObject);
        //TodayQ = GameObject.Find("TodayQ").GetComponent<>();
        //GameChat.initialize("e3558324-2d64-47d0-bd7a-6fa362824bd7");
        //PlayerPrefs.SetString("LastMSGID", "62860f665d257d31a779d5cf");
        


    }

    // Update is called once per frame
    void Update()
    {
        //���÷��� Ÿ�̸� �۵� �ڵ�
        if (null != GameObject.Find("isTitle"))
        {
            Check_Timer();
        }
        

    }

    //�� �������� ���ϴ� ���� �ε�
    static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    //Ÿ��Ʋ �� ȣ��
    public void LoadScene_Title()
    {
        LoadScene("Title");
    }

    //Ȩ �� ȣ��
    public void LoadScene_Home()
    {
        LoadScene("Home");
        //PlayerPrefs.SetString("GCName", "hj");
    }

    //ê����Ʈ �� ȣ��
    public void LoadScene_ChatList()
    {
        LoadScene("ChatList");
    }

    //���Ϲڽ� �� ȣ��
    public void LoadScene_MailBox()
    {
        LoadScene("MailBox");
    }

    //���������� �� ȣ��
    public void LoadScene_MyProfile_Sample3()
    {
        LoadScene("MyProfile_Sample3");
        Debug.Log("�����ʾ� ȣ���ư Ŭ��");
    }
   
    //�ϴ��ϴ�ȭ �� ȣ��
    public void LoadScene_PersonalChat()
    {
        LoadScene("PersonalChat");
    }

    //�𷡾��׽�Ʈ �� ȣ��
    public void LoadScene_TypeTest()
    {
        LoadScene("TypeTest");
    }

    //��üä�ù� �� ȣ��
    public void LoadScene_SampleScene_Login()
    {
        LoadScene("SampleScene_Login");

    }

    public static void LoadScene_GroupChat()
    {
        LoadScene("SampleScene_Main");
    }

    //ȸ������ �� ȣ��
    public void LoadScene_SignUp()
    {
        LoadScene("SignUp");
    }

    //�α��� �� ȣ��
    public void LoadScene_SignIn()
    {
        LoadScene("SignIn");
    }

    //���� �� ȣ��
    public void LoadScene_Setting()
    {
        LoadScene("Setting");
    }


    //���÷��� ȭ�� ������ ���� Ÿ�̸� �ڵ�
    
    private void Check_Timer()
    {

        if (0 < time_current)
        {
            time_current -= Time.deltaTime;
            //Debug.Log(time_current);
        }
        else if (!isEnded)
        {
            End_Timer();
        }


    }

    private void End_Timer()
    {
        Debug.Log("Splah Timer End");
        time_current = 0;
        title.SetActive(true);
        splash.SetActive(false);
        isEnded = true;
    }

    


    private void Reset_Timer()
    {
        time_current = time_Max;
        isEnded = false;
        Debug.Log("Start");
    }



    //ä�ù� �̸� ���������� ���� csv�Ľ��ڵ�
    //public void ReadCSV()
    //{
    //    //������ CSV������ ,�� ���ʹ����� �Ľ�
    //    string[] CSVdata = csvfile.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);

    //    ChatRoomNameList.CRN = new chatRoomName[tableSize];

    //    for (int i = 0; i < tableSize - 1; i++)
    //    {
    //        ChatRoomNameList.CRN[i] = new chatRoomName();
    //        ChatRoomNameList.CRN[i].Adjective = (CSVdata[2 * (i + 1)]);
    //        ChatRoomNameList.CRN[i].Noun = (CSVdata[2 * (i + 1) + 1]);
    //    }

    //}
   
    //public string makeChatRoomName()
    //{
    //    while (true)
    //    {
    //        int AdjNum = UnityEngine.Random.Range(1, tableSize);
    //        int NounNum = UnityEngine.Random.Range(1, tableSize);

    //        string adj = ChatRoomNameList.CRN[AdjNum].Adjective;
    //        string noun = ChatRoomNameList.CRN[NounNum].Noun;

    //        int adjNum = adj.Length;
    //        int nounNum = noun.Length;
    //        Debug.Log("���������" + (adjNum + nounNum + 1));
    //        //Debug.Log(adjNum);
    //        //Debug.Log(nounNum);

    //        if (adjNum + nounNum + 1 < 8)
    //        {
    //            Debug.Log(adj + " " + noun);
    //            return (adj + " " + noun);
    //        }
    //    }

    //}



    //public string CreatChatCode()
    //{
    //    GameChat.getChannels(0, 1, (List<Channel> Channels, GameChatException exception) =>
    //    {
    //        if (exception != null)
    //        {
    //            Debug.Log("getChannels Exception Log => " + exception.ToJson());
    //            return;
    //        }
    //        foreach (Channel elem in Channels)
    //        {
    //            if (null != elem)
    //            {
    //                List<string> chatList = new List<string>();
    //                chatList.Add(elem.ToString());
    //                creatChatCode = chatList[0];
    //                Debug.Log("creatChatCode" + creatChatCode);

    //            }
    //            else
    //            {
    //                Debug.Log("elem����");
    //            }
                
    //        }   
    //    });
    //    return (creatChatCode);
    //}

}
