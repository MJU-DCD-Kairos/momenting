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


    // Start is called before the first frame update
    void Start()
    {
        //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

        makeRList();
        //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
        backToHome.onClick.AddListener(gSM.LoadScene_Home);
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

        ui.transform.GetChild(2).GetComponent<Text>().text = RoomTitleText; //ä�ù� �̸�
        ui.transform.GetChild(4).transform.GetChild(1).GetComponent<Text>().text = RoomTimerText; //�����ð�
        ui.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = RoomNewMSGCount.ToString(); //���� ��
        ui.transform.GetChild(3).GetComponent<Text>().text = RoomContentsText; //��Ʈ ä�ó���
    }

    public void GetCurrentChatCount(string id)
    {
        GameChatSample.NewChatManager.getChannelID();//firebase���� id�� �����;���. ������ id�� string���� ���� ����

        RoomNewMSGCount = GameChatSample.NewChatManager.getCurrentMSG(id);
        getLostTime();

        
    }

    public string getLostTime()
    {
        string cTime = GameChatSample.NewChatManager.CList[0].created_at.Substring(14, 5);//�����ð�
        string nTime = DateTime.Now.ToString("u").Substring(12, 5);//����ð�
        TimeSpan goTime = DateTime.Parse(cTime) - DateTime.Parse(nTime);
        TimeSpan lostTime = DateTime.Parse("20:00") - DateTime.Parse(goTime.ToString());
        return lostTime.ToString();
    }

}
