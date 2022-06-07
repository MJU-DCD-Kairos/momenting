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
        Dictionary<string, List<object>> testDict = FirebaseManager.KWList;

        //Dictionary�� Ű�� ���鼭 Ű�� ���� Ű���� ����Ʈ ���̸�ŭ ������Ʈ ����, �ش� ���� ����
        foreach (string Key in testDict.Keys)
        {
            for (int l = 0; l < testDict[Key].Count; l++)
            {
                GameObject ListContent = Instantiate(Resources.Load("Prefabs/MyKeyword")) as GameObject;
                ListContent.transform.SetParent(ContentParents.transform, false);

                Color color;
                ColorUtility.TryParseHtmlString(Key, out color);//""�ȿ� DB���� �޾ƿ� ����ڵ� �־ rgb��ȯ �� ����
                                                                //Ű���� ī�װ� ����

                
                ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().color = color;

                //Ű���� ����
                ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Text>().text = testDict[Key][l].ToString();//"Ű���� ���� �׽�Ʈ";���⿡ DB���� �޾ƿ� Ű���带 string���� ����

                //Ű���� ����
                ListContent.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
            }
        }
           
        //FOR ( INT I = 0; I<5; I++)
        //{
            

        //    GAMEOBJECT LISTCONTENT = INSTANTIATE(RESOURCES.LOAD("PREFABS/MYKEYWORD")) AS GAMEOBJECT;
        //    LISTCONTENT.TRANSFORM.SETPARENT(CONTENTPARENTS.TRANSFORM, FALSE);

        //    COLOR COLOR;
        //    COLORUTILITY.TRYPARSEHTMLSTRING("#001130", OUT COLOR);//""�ȿ� DB���� �޾ƿ� ����ڵ� �־ RGB��ȯ �� ����
        //                                                          //Ű���� ī�װ� ����
        //    LISTCONTENT.TRANSFORM.GETCHILD(0).TRANSFORM.GETCHILD(0).TRANSFORM.GETCHILD(0).GAMEOBJECT.GETCOMPONENT<IMAGE>().COLOR = COLOR;

        //    //Ű���� ����
        //    LISTCONTENT.TRANSFORM.GETCHILD(0).TRANSFORM.GETCHILD(0).TRANSFORM.GETCHILD(1).GAMEOBJECT.GETCOMPONENT<TEXT>().TEXT = "Ű���� ���� �׽�Ʈ";//���⿡ DB���� �޾ƿ� Ű���带 STRING���� ����

        //    //Ű���� ����
        //    LISTCONTENT.TRANSFORM.GETCHILD(1).GAMEOBJECT.GETCOMPONENT<TEXT>().TEXT = "Ű���� ���� �׽�Ʈ";
        //}
        
    }






}

