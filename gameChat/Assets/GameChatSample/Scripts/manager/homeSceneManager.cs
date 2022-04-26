using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class homeSceneManager : MonoBehaviour
{
    //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
    gameSceneManager gSM;
    
    //��Ī ȭ�鿡�� ä������ �Ѿ�� �÷ο� �׽�Ʈ�� Ÿ�̸�, �Һ��� ����
    bool isMatching = true;
    // Ÿ�̸� �ڵ带 ���� ���� ����
    private float time_current = 5f;
    private float time_Max = 5f;
    private bool isEnded;
    //��Īȭ�� Ȱ��ȭ ���� üũ�� ���� ������Ʈ ����
    public GameObject MatchingPage;


    //onclick �̿��� ��ư ����
    public Button ProfileBtn;
    public Button MailBox;
    public Button ChatList;

    // Start is called before the first frame update
    void Start()
    {
        //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();
        
        //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
        ProfileBtn.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
        MailBox.onClick.AddListener(gSM.LoadScene_MailBox);
        ChatList.onClick.AddListener(gSM.LoadScene_ChatList);
    }

    // Update is called once per frame
    void Update()
    {
        if (MatchingPage.active)
        {
            Check_Timer();
        }
    }

    //��Ī �Ϸ� �� ä�� �÷ο츦 �����ϴ� ���� �ε��ϴ� �Լ�
    void matchingScene()
    {
        if(isMatching == true)
        {
            gSM.LoadScene_SampleScene_Login();
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
            Debug.Log(time_current);
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
        matchingScene();
    }


    private void Reset_Timer()
    {
        time_current = time_Max;
        isEnded = false;
        Debug.Log("Start");
    }

}
