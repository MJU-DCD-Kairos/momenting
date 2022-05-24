using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class myprofileSceneManager : MonoBehaviour
{
    //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
    gameSceneManager gSM;
    public Button backToHome;
    public Button goToChatList;
    public Button goToSetting;
    public Button goToTest;
    public string GCN;
    public Text txtName;



    void Awake()
    {
        GCN = "";
        GCN = PlayerPrefs.GetString("GCName");
        Debug.Log(GCN + "�ҷ���");
    }
    // Start is called before the first frame update
    void Start()
    {
        //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

        //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
        backToHome.onClick.AddListener(gSM.LoadScene_Home); 
        goToChatList.onClick.AddListener(gSM.LoadScene_ChatList);
        goToSetting.onClick.AddListener(gSM.LoadScene_Setting);
        goToTest.onClick.AddListener(gSM.LoadScene_TypeTest);

        //������ �̸� �޾ƿ���   
        txtName.text = GCN;
    }

    // Update is called once per frame
    void Update()  //������Ʈ ������ �־������ .
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








}

