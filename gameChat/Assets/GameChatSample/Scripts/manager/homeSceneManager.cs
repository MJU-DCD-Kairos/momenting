using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameChatSample;
using FireStoreScript;

public class homeSceneManager : MonoBehaviour
{
    //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
    //NewChatManager NCM;
    public TextAsset csvFile;

    //��Īȭ�� Ȱ��ȭ ���� üũ�� ���� ������Ʈ ����
    public GameObject MatchingPage;

    //���Խ��� ����
    public GameObject ispasscan;
    public GameObject HomeCan;

    gameSceneManager gSM;
    FirebaseManager FbM;
    
    //������ ���� �亯 ���� üũ
    

    //onclick �̿��� ��ư ����
    public Button ProfileBtn;
    public Button MailBox;
    public Button ChatList;


    // Start is called before the first frame update
    void Start()
    {
        //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();
        FbM = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();
        //NCM = GameObject.Find("GameSceneManager").GetComponent<NewChatManager>();
        //NewChatManager.getChannelID();

        //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
        ProfileBtn.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
        MailBox.onClick.AddListener(gSM.LoadScene_MailBox);
        ChatList.onClick.AddListener(gSM.LoadScene_ChatList);
        //FirebaseManager.LoadData();
        if(FirebaseManager.ispass == "false")
        {
            ispasscan.SetActive(true);
            HomeCan.SetActive(false);
        }
        else
        {
            ispasscan.SetActive(false);
            HomeCan.SetActive(true);
        }
      

    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)  // �÷��� ���� .
        {
            if (Input.GetKey(KeyCode.Escape)) // Ű ���� �ڵ� ��ȣ�� �޾ƿ��°�.
            {
                //SceneManager.LoadScene("�ڷΰ� �� �̸� "); // ������ �̵� .
                Application.Quit(); // �� ���� .(������)            �������� �̵��̳� ������ �����ϳ� ���Ͻô°��� ����Ͻø� �˴ϴ�.
            }
        }

    }

    
    //������ ���� �亯 �� ���̾�α� �ȶ����
    //public void isTodayQdoneTrue()
    //{
    //    PlayerPrefs.SetInt("todayQdone",1);
    //    PlayerPrefs.SetInt("todayQanswer", 1);
    //}

    ////������ ���� ���� ��� ����
    //public void isTodayQdoneFalse()
    //{
    //    if(1!= PlayerPrefs.GetInt("todayQanswer"))
    //    {
    //        PlayerPrefs.SetInt("todayQdone", 0);
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt("todayQdone", 1);
    //    }
        
    //}

}
