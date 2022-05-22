using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameChatSample;
public class homeSceneManager : MonoBehaviour
{
    //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
    gameSceneManager gSM;
    //NewChatManager NCM;
    public TextAsset csvFile;

    //��Ī ȭ�鿡�� ä������ �Ѿ�� �÷ο� �׽�Ʈ�� Ÿ�̸�, �Һ��� ����
    //bool isMatching = true;
    // Ÿ�̸� �ڵ带 ���� ���� ����
    public float time_current = 5f;
    private float time_Max = 60f;
    //��Īȭ�� Ȱ��ȭ ���� üũ�� ���� ������Ʈ ����
    public GameObject MatchingPage;


    //������ ���� �亯 ���� üũ
    

    //onclick �̿��� ��ư ����
    public Button ProfileBtn;
    public Button MailBox;
    public Button ChatList;
    //public Button matchingBtn;

    public bool startTimer;

    // Start is called before the first frame update
    void Start()
    {
        //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();
        //NCM = GameObject.Find("GameSceneManager").GetComponent<NewChatManager>();
        //NewChatManager.getChannelID();

        //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
        ProfileBtn.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
        MailBox.onClick.AddListener(gSM.LoadScene_MailBox);
        ChatList.onClick.AddListener(gSM.LoadScene_ChatList);

        
    }

    // Update is called once per frame
    void Update()  //������Ʈ ������ �־������ .
    {
        if (Application.platform == RuntimePlatform.Android)  // �÷��� ���� .
        {
            if (Input.GetKey(KeyCode.Escape)) // Ű ���� �ڵ� ��ȣ�� �޾ƿ��°�.
            {
                //SceneManager.LoadScene("�ڷΰ� �� �̸� "); // ������ �̵� .
                Application.Quit(); // �� ���� .(������)            �������� �̵��̳� ������ �����ϳ� ���Ͻô°��� ����Ͻø� �˴ϴ�.
            }
        }

        if(startTimer == true)
        {
            time_current = time_Max; //60�ʷ� ����
            Debug.Log("Ÿ�̸� ����");
            time_current -= Time.deltaTime; //60�� Ÿ�̸� ����

            if(NewChatManager.isMatchComplete == true) //��Ī�Ϸ�Ǹ�
            {
                time_current = 3f; //3�� ���
                matchingScene(); 
            }
            else if(time_current ==0 && NewChatManager.isMatchComplete == false)//��Ī �ȵ� ���·� 60�� ������ ��
            {
                //��Ī���� DIALOG ����ֱ�
            }
        }
    }

    //��Ī �Ϸ� �� ä�� �÷ο츦 �����ϴ� ���� �ε��ϴ� �Լ�
    public void matchingScene()
    {
        startTimer = false;
        gameSceneManager. LoadScene_GroupChat();
    }

    public void setTimer()
    {
        startTimer = true;
    }

    //������ ���� �亯 �� ���̾�α� �ȶ����
    public void isTodayQdoneTrue()
    {
        PlayerPrefs.SetInt("todayQdone",1);
        PlayerPrefs.SetInt("todayQanswer", 1);
    }

    //������ ���� ���� ��� ����
    public void isTodayQdoneFalse()
    {
        if(1!= PlayerPrefs.GetInt("todayQanswer"))
        {
            PlayerPrefs.SetInt("todayQdone", 0);
        }
        else
        {
            PlayerPrefs.SetInt("todayQdone", 1);
        }
        
    }

}
