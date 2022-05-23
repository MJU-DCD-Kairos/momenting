using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameChatSample;
public class homeSceneManager : MonoBehaviour
{
    //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
    //NewChatManager NCM;
    public TextAsset csvFile;

    //��Īȭ�� Ȱ��ȭ ���� üũ�� ���� ������Ʈ ����
    public GameObject MatchingPage;
    gameSceneManager gSM;

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
        //NewChatManager.getChannelID();

        //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
        ProfileBtn.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
        MailBox.onClick.AddListener(gSM.LoadScene_MailBox);
        ChatList.onClick.AddListener(gSM.LoadScene_ChatList);

        
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
