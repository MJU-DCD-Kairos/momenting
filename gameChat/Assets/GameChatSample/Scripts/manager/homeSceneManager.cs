using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class homeSceneManager : MonoBehaviour
{
    //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
    gameSceneManager gSM;
    //NewChatManager NCM;
    public TextAsset csvFile;

    //��Ī ȭ�鿡�� ä������ �Ѿ�� �÷ο� �׽�Ʈ�� Ÿ�̸�, �Һ��� ����
    bool isMatching = true;
    // Ÿ�̸� �ڵ带 ���� ���� ����
    private float time_current = 5f;
    private float time_Max = 5f;
    private bool isEnded;
    //��Īȭ�� Ȱ��ȭ ���� üũ�� ���� ������Ʈ ����
    public GameObject MatchingPage;


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
        //NCM = GameObject.Find("GameSceneManager").GetComponent<NewChatManager>();
        GameChatSample.NewChatManager.getChannelID();

        //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
        ProfileBtn.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
        MailBox.onClick.AddListener(gSM.LoadScene_MailBox);
        ChatList.onClick.AddListener(gSM.LoadScene_ChatList);

        Invoke("makeGCList", 0.03f);
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
    }









    //��Ī �Ϸ� �� ä�� �÷ο츦 �����ϴ� ���� �ε��ϴ� �Լ�
    public void matchingScene()
    {
        if(isMatching == true)
        {
            gSM.LoadScene_GroupChat();
        }
        else
        {
            Reset_Timer();
            Check_Timer();
        }
        
    }

    // Ÿ�̸� �ڵ�
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
        Debug.Log("End");
        time_current = 0;
        //gSM.ReadCSV();
   
        //StartCoroutine(gSM.CreateChatRoom());
       
        //gSM.CreatChatCode();
        
        matchingScene();
    }


    private void Reset_Timer()
    {
        time_current = time_Max;
        isEnded = false;
        Debug.Log("Start");
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
