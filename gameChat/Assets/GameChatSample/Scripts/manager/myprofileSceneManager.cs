using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FireStoreScript;
using System.Text;
using System;
using editprofile;
using System.Threading.Tasks;

namespace myprofile
{
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
        

        //키워드 불러오기위한 프리팹 참조
        //public GameObject KWlistPrefabs;
        //리스트를 넣어주는 부모 개체
        public GameObject ContentParents;




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
            if (FirebaseManager.sex == 1)
            {
                txtSex.text = "남";
            }
            else
            {
                txtSex.text = "여";
            }

            setUserKW();
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

        //편집 후 프로필 재로드
        public void LoadProfile() 
        {
            if (MyProfileEditManager.EDIT_INTRO == null)
            {
                Invoke("LoadProfile", 0.1f);
            }
            else
            {
                Debug.Log("프로필 씬 == " + MyProfileEditManager.EDIT_INTRO);
                txtIntro.text = MyProfileEditManager.EDIT_INTRO;
                firebase_LoadKW();
                if (FirebaseManager.KWList == null)
                {
                    Invoke("firebase_LoadKW", 0.1f);
                }
                else
                {
                    setUserKW();
                }
                
            }
        }

        void firebase_LoadKW()
        {
            GameObject.FindGameObjectWithTag("firebaseManager").GetComponent<FirebaseManager>().LoadKW();
        }

        //유저의 키워드를 생성하는 함수
        public void setUserKW() //작업중
        {
            firebase_LoadKW();
            if (FirebaseManager.KWList == null)
            {
                Debug.Log("KWList 못받아옴");
                Invoke("firebase_LoadKW", 0.1f);
            }
            else
            {
                Debug.Log("KWList 받아옴");
                for (int i = 0; i < FirebaseManager.KWList.Count; i++)
                {
                    //Debug.Log(FirebaseManager.KWList[i]);
                    GameObject ListContent = Instantiate(Resources.Load("Prefabs/C_KW")) as GameObject;
                    ListContent.transform.SetParent(ContentParents.transform, false);

                    //키워드 글자
                    //ListContent.transform.GetChild(0).transform.getchild(0).transform.getchild(1).gameobject.getcomponent<text>().text = testdict[key][l].tostring();//"키워드 적용 테스트";여기에 db에서 받아온 키워드를 string으로 찍음
                }

            }

            //Debug.Log("setUserKW 실행됨");
            //dictionary의 키를 돌면서 키가 가진 키워드 리스트 길이만큼 오브젝트 생성, 해당 내용 대입
            //foreach (string key in testdict.keys)
            //{
            //    for (int l = 0; l < testdict[key].count; l++)
            //    {
            //        gameobject listcontent = instantiate(resources.load("prefabs/mykeyword")) as gameobject;
            //        listcontent.transform.setparent(contentparents.transform, false);

            //        color color;
            //        colorutility.tryparsehtmlstring(key, out color);//""안에 db에서 받아온 헥사코드 넣어서 rgb변환 후 찍음
            //                                                        //키워드 카테고리 색상


            //        listcontent.transform.getchild(0).transform.getchild(0).transform.getchild(0).gameobject.getcomponent<image>().color = color;

            //        //키워드 글자
            //        listcontent.transform.getchild(0).transform.getchild(0).transform.getchild(1).gameobject.getcomponent<text>().text = testdict[key][l].tostring();//"키워드 적용 테스트";여기에 db에서 받아온 키워드를 string으로 찍음

            //        //키워드 설명
            //        listcontent.transform.getchild(1).gameobject.getcomponent<text>().text = "";
            //    }
            //}

        }






    }
}

