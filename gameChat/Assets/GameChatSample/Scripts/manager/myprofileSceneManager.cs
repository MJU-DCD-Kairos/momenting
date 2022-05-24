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
    //스크립트 받아오기위한 타입 변수 선언
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



    void Awake()
    {
        GCN = "";
        GCN = PlayerPrefs.GetString("GCName");
        Debug.Log(GCN + "불러옴");
    }
    // Start is called before the first frame update
    void Start()
    {
        //don't destroy로 살려서 넘어온 게임씬매니저의 스크립트를 변수에 담음
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

        //버튼에 gSM의 로드씬 함수 리스너를 추가함
        backToHome.onClick.AddListener(gSM.LoadScene_Home); 
        goToChatList.onClick.AddListener(gSM.LoadScene_ChatList);
        goToSetting.onClick.AddListener(gSM.LoadScene_Setting);
        goToTest.onClick.AddListener(gSM.LoadScene_TypeTest);

        //프로필 이름 받아오기   
        txtName.text = GCN;
        txtAge.text = FirebaseManager.age;
        txtIntro.text = FirebaseManager.myintroduction;
        if(FirebaseManager.sex == 1)
        {
            txtSex.text = "남";
        }
        else
        {
            txtSex.text = "여";
        }
        
        
    }

    // Update is called once per frame
    void Update()  //업데이트 문에다 넣어줘야함 .
    {
        if (Application.platform == RuntimePlatform.Android)  // 플렛폼 정보 .
        {
            if (Input.GetKey(KeyCode.Escape)) // 키 눌린 코드 신호를 받아오는것.
            {
                SceneManager.LoadScene("Home"); // 씬으로 이동 .
                //Application.Quit(); // 씬 종료 .(나가기)            위씬으로 이동이나 종료기능 둘중하나 원하시는것을 사용하시면 됩니다.
            }
        }
    }








}

