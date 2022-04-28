using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameSceneManager : MonoBehaviour
{
    //���÷��� Ÿ�̸� �ڵ带 ���� ���� ����
    private float time_current = 3f;
    private float time_Max = 3f;
    private bool isEnded;
    public GameObject title;
    public GameObject splash;
    
    //GameObject TodayQ;


    // Start is called before the first frame update
    void Start()
    {
        //���Ŵ��� �ı� ������ ���� �ڵ�
        DontDestroyOnLoad(this.gameObject);
        //TodayQ = GameObject.Find("TodayQ").GetComponent<>();

    }

    // Update is called once per frame
    void Update()
    {
        //���÷��� Ÿ�̸� �۵� �ڵ�
        Check_Timer();
    }

    //�� �������� ���ϴ� ���� �ε�
    void LoadScene(string name)
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
}
