using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameChatSample;

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

    public string id = "";

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
        
        makeGCList();
        
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
        }*/
        ui.transform.GetChild(3).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[1]; //��Ʈ ä�ó���

        //CurChatInfo[0]�̸� [1]���� [2]�����ð� [3]������¥ [4]���ο�޽�����
    }

    public void GetCurrentChatCount(string id)
    {
        Debug.Log("GetCurrentChatCount���� id��? : " + id);
        //GameChatSample.NewChatManager.getChannelID();//firebase���� id�� �����;���. ������ id�� string���� ���� ����
        RoomNewMSGCount = GameChatSample.NewChatManager.setNewMSGCount(id);
        
    }
    /*
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
                {
                    RoomTimerText = "����";
                }
                else
                {
                    RoomTimerText = goTime.Minutes.ToString() + "��";
                }
            }
            RoomTimerText = "����";


        }

    }*/

    public void makeGCList()
    {
        id = GameChatSample.NewChatManager.CList[0].id.ToString().Replace(" ","");
        RoomContentsText = GameChatSample.NewChatManager.getCurMSG(id);
        RoomTitleText = GameChatSample.NewChatManager.getCurRoomName(id);
        GetCurrentChatCount(id);//���� �� ���ϱ�
        

        Invoke("makeRList", 1f);

     
    }

}
