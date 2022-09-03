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
using editprofile;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

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
        
        public GameObject KWParents; //키워드 리스트 4줄을 전부 넣어주는 부모 개체
        public GameObject KWContents0; //키워드 리스트 1번째 줄
        public GameObject KWContents1; //키워드 리스트 2번째 줄
        public GameObject KWContents2; //키워드 리스트 3번째 줄
        public GameObject KWContents3; //키워드 리스트 4번째 줄
        public GameObject KWarea;
        public GameObject canvas_ED;

        //public List<string> myKW = new List<string>();
        public Button SaveBtn;
        public List<string> profileKW = new List<string>();



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
            KW_ToggleOn();
            setUserKW();
            SaveBtn.onClick.AddListener(KWedit);
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
                //firebase_LoadKW();
                //if (FirebaseManager.KWList == null)
                //{
                //    Invoke("firebase_LoadKW", 0.1f);
                //}
                //else
                //{
                //    setUserKW();
                //}
                
            }
        }
        
        public async Task Edit_SaveKW()
        {
            var awaiter = delete_duplicatedKW().GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                //int kw = 0;
                FirebaseManager.KWList.Clear();
                for (int i = 0; i < getKeywordList.saveKWlist.Count; i++)
                {
                    FirebaseManager.KWList.Add(getKeywordList.saveKWlist[i]);
                    Debug.Log("SAVE 새로 선택한 키워드: " + FirebaseManager.KWList[i]);
                    
                }
                //return kw;
                FirebaseManager.db.Collection("userInfo").Document(GCN).UpdateAsync("Keyword", FirebaseManager.KWList);
                Debug.LogError("db에 키워드 저장 완료!");
            });


        }
        //async Task setKW()
        //{

        //}

        public async Task delete_duplicatedKW()
        {
            await FirebaseManager.db.Collection("userInfo").Document(GCN).UpdateAsync("Keyword", FieldValue.Delete);
            Debug.LogError("DB키워드 삭제 완료!");
        }

        public async void KWedit()
        {
            Debug.LogError("KWedit 실행");
            var awaiter = Edit_SaveKW().GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                //setUserKW();
                Invoke("setUserKW", 0.5f);
                
            });
        }

        public void KW_ToggleOn()
        {
            //관심사
            for (int k = 1; k < 6; k++)
            { 
                GameObject Group = KWarea.transform.GetChild(k).gameObject;
                
                for (int i = 0; i < Group.transform.childCount; i++)
                {
                    string Text = Group.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text;
                    Group.transform.GetChild(i).GetComponent<Toggle>().isOn = false;

                    for (int w = 0; w < FirebaseManager.KWList.Count; w++)
                    {
                        if (Text == FirebaseManager.KWList[w].ToString())
                        {
                            Group.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
                        }
                        //else
                        //{
                        //    Group.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
                        //}
                    }
                }
            }
            //성향
            for (int k = 7; k < 14; k++)
            {
                GameObject Group = KWarea.transform.GetChild(k).gameObject;
                
                for (int i = 0; i < Group.transform.childCount; i++)
                {
                    string Text = Group.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text;
                    Group.transform.GetChild(i).GetComponent<Toggle>().isOn = false;

                    for (int w = 0; w < FirebaseManager.KWList.Count; w++)
                    {
                        if (Text == FirebaseManager.KWList[w].ToString())
                        {
                            Group.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
                        }
                        //else
                        //{
                        //    Group.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
                        //}
                    }
                }
            }
            Debug.LogError("KW_ToggleOn 실행");
        }
        
        //키워드 칩스 생성하는 함수
        async Task setUserKW()
        {
            //리스트 프리팹 삭제
            for (int j = 0; j < 4; j++)
            {
                GameObject KWprefab = KWParents.transform.GetChild(j).gameObject;
                if (0 < KWprefab.transform.childCount)
                {
                    for (int n = 0; n < KWprefab.transform.childCount; n++)
                    {
                        GameObject.Destroy(KWprefab.transform.GetChild(n).gameObject);
                    }
                }
            }
            float sum0 = -32;
            float sum1 = -32;
            float sum2 = -32;

            List<float> KW_Wid = new List<float>();

            //Debug.Log("KWList 받아옴");
            GameObject ListContent;

            if (FirebaseManager.KWList.Count == 0) //키워드가 없을때 UI
            {
                KWParents.transform.GetChild(0).gameObject.SetActive(true);
                KWContents0.SetActive(false);
                KWContents1.SetActive(false);
                KWContents2.SetActive(false);
                KWContents3.SetActive(false);

                //canvas_ED.SetActive(false);
                //canvas_ED.SetActive(true);

                KWParents.SetActive(false);
                KWParents.SetActive(true);
            }
            else
            {
                KWParents.transform.GetChild(0).gameObject.SetActive(false);
                KWContents0.SetActive(true);
                KWContents1.SetActive(true);
                KWContents2.SetActive(true);
                KWContents3.SetActive(true);

                for (int i = 0; i < FirebaseManager.KWList.Count; i++)
                {
                    if ((FirebaseManager.KWList[i].ToString().Length == 1) || FirebaseManager.KWList[i].ToString() == "IT") { KW_Wid.Add(36 + 187 + 32); }
                    else if (FirebaseManager.KWList[i].ToString().Length == 2) { KW_Wid.Add(36 + 246 + 32); }
                    else if (FirebaseManager.KWList[i].ToString().Length == 3)
                    {
                        if (FirebaseManager.KWList[i].ToString() == "SNS") { KW_Wid.Add(36 + 253 + 32); }
                        else { KW_Wid.Add(36 + 305 + 32); }
                    }
                    else if (FirebaseManager.KWList[i].ToString().Length == 4)
                    {
                        if (FirebaseManager.KWList[i].ToString() == "MBTI") { KW_Wid.Add(36 + 283 + 32); }
                        else { KW_Wid.Add(36 + 364 + 32); }
                    }
                    else if (FirebaseManager.KWList[i].ToString().Length == 5) { KW_Wid.Add(36 + 423 + 32); }
                    else { KW_Wid.Add(36 + 482 + 32); }
                }

                for (int n = 0; n < KW_Wid.Count; n++)
                {
                    ListContent = Instantiate(Resources.Load("Prefabs/KeywordPrefs/C_KW")) as GameObject;
                    //키워드 글자
                    ListContent.transform.GetChild(0).GetComponent<Text>().text = "#" + FirebaseManager.KWList[n].ToString();

                    if (sum0 + KW_Wid[n] <= 1312)
                    {
                        sum0 = sum0 + KW_Wid[n];
                        ListContent.transform.SetParent(KWContents0.transform, false);
                    }
                    else if (sum1 + KW_Wid[n] <= 1312)
                    {
                        sum1 = sum1 + KW_Wid[n];
                        ListContent.transform.SetParent(KWContents1.transform, false);
                    }
                    else if (sum2 + KW_Wid[n] <= 1312)
                    {
                        sum2 = sum2 + KW_Wid[n];
                        ListContent.transform.SetParent(KWContents2.transform, false);
                    }
                    else
                    {
                        ListContent.transform.SetParent(KWContents3.transform, false);
                    }

                }
                canvas_ED.SetActive(false);
                canvas_ED.SetActive(true);

                KWParents.SetActive(false);
                KWParents.SetActive(true);
                KW_ToggleOn();

                Debug.LogError("setUserKW 실행");
            }
            
        }

    }
}

