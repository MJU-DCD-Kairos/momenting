using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FireStoreScript;
using System.Text;
using System;

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
    public Text txtAge;
    public Text txtIntro;
    public Text txtSex;

    //Ű���� �ҷ��������� ������ ����
    //public GameObject KWlistPrefabs;
    //����Ʈ�� �־��ִ� �θ� ��ü
    public GameObject ContentParents;




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
        txtAge.text = FirebaseManager.age;
        txtIntro.text = FirebaseManager.myintroduction;
        if(FirebaseManager.sex == 1)
        {
            txtSex.text = "��";
        }
        else
        {
            txtSex.text = "��";
        }

        setUserKW();



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


    //������ Ű���带 �����ϴ� �Լ�
    public void setUserKW()
    {
        //db���� �޾ƿ� Dict<string, List<string>> ���¸� �޾ƿ�

        //#~�����ڵ� ������ ����Ʈ�� ����
        //����Ʈ�� ���̸�ŭ for�ݺ������� initiate �Լ�
        
        for( int i = 0; i<5; i++)
        {
            GameObject ListContent = Instantiate(Resources.Load("Prefabs/MyKeyword")) as GameObject;
            ListContent.transform.SetParent(ContentParents.transform, false);

            Color color;
            ColorUtility.TryParseHtmlString("#001130", out color);//""�ȿ� DB���� �޾ƿ� ����ڵ� �־ rgb��ȯ �� ����
                                                                  //Ű���� ī�װ� ����
            ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().color = color;

            //Ű���� ����
            ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Text>().text = "Ű���� ���� �׽�Ʈ";//���⿡ DB���� �޾ƿ� Ű���带 string���� ����

            //Ű���� ����
            ListContent.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Ű���� ���� �׽�Ʈ";
        }
        
    }






}

