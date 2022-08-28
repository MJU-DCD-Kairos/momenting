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
        
        public GameObject KWParents; //키워드 리스트 4줄을 전부 넣어주는 부모 개체
        public GameObject KWContents0; //키워드 리스트 1번째 줄
        public GameObject KWContents1; //키워드 리스트 2번째 줄
        public GameObject KWContents2; //키워드 리스트 3번째 줄
        public GameObject KWContents3; //키워드 리스트 4번째 줄







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

        public void firebase_LoadKW()
        {
            GameObject.FindGameObjectWithTag("firebaseManager").GetComponent<FirebaseManager>().LoadKW();
            if (FirebaseManager.KWList == null)
            {
                Debug.Log("KWList 못받아옴");
                Invoke("firebase_LoadKW", 0.1f);
            }
            else
            {
                Invoke("setUserKW", 0.5f);
            }
        }

        //유저의 키워드를 생성하는 함수
        public void setUserKW() //작업중
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
            //키워드 칩스들의 폭의 합
            float sum0 = -32;
            float sum1 = -32;
            float sum2 = -32;
            //키워드를 몇번째 레이아웃에 배치할건지 판별하기 위한 변수
            //bool layout0 = false;
            //bool layout1 = false;
            //bool layout2 = false;

            List<float> KW_Wid = new List<float>();

            Debug.Log("KWList 받아옴");
            GameObject ListContent;

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



                //float wid = ListContent.transform.GetComponent<RectTransform>().rect.width;
                //float wid = KWContents0.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.x;

                //Debug.Log("칩 폭: " + wid);
                //for (int n = 0; n < KWContents0.transform.childCount; n++) //1번째 줄에 들어갈 자리 있는지 계산
                //{
                //    sum = sum + wid;
                //    Debug.Log(wid);
                //    Debug.Log("합: " + sum);
                //    //sum = sum + KWContents0.transform.GetChild(n).gameObject.GetComponent<RectTransform>().rect.width + 32;

                //    Debug.Log("1번째 줄: " + sum);
                //}
                //if (sum >= 1312) //1번째 줄에 자리 없으면
                //{
                //    sum = -32; //합 초기화
                //    for (int q = 0; q < KWContents1.transform.childCount; q++) //2번째 줄에 들어갈 자리 있는지 계산
                //    {
                //        sum = sum + KWContents1.transform.GetChild(n).GetComponent<RectTransform>().rect.width + 32;
                //        Debug.Log("2번째 줄: " + sum);
                //        if (sum >= 1312) //2번째 줄에 자리 없으면
                //        {
                //            sum = -32; //합 초기화
                //            for (int j = 0; j < KWContents2.transform.childCount; j++) //3번째 줄에 들어갈 자리 있는지 계산
                //            {
                //                sum = sum + KWContents2.transform.GetChild(n).GetComponent<RectTransform>().rect.width + 32;
                //                Debug.Log("3번째 줄: " + sum);
                //                if (sum >= 1312) //3번째 줄에 자리 없으면
                //                {
                //                    ListContent.transform.SetParent(KWContents3.transform, false); //4번째 줄에 오브젝트 넣어주기
                //                }
                //                else //3번째 줄에 자리 있으면
                //                {
                //                    ListContent.transform.SetParent(KWContents2.transform, false); //3번째 줄에 오브젝트 넣어주기
                //                }
                //            }

                //        }
                //        else //2번째 줄에 자리 있으면
                //        {
                //            ListContent.transform.SetParent(KWContents1.transform, false); //2번째 줄에 오브젝트 넣어주기
                //        }
                //    }

                //}
                //else //1번째 줄에 자리 있으면
                //{
                //    ListContent.transform.SetParent(KWContents0.transform, false); //1번째 줄에 오브젝트 넣어주기
                //}


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
                    Debug.Log("0: " + sum0);
                }
                else
                {
                    if (sum1 + KW_Wid[n] <= 1312)
                    {
                        sum1 = sum1 + KW_Wid[n];
                        ListContent.transform.SetParent(KWContents1.transform, false);
                        Debug.Log("1: " + sum1);
                    }
                    else
                    {
                        if (sum2 + KW_Wid[n] <= 1312)
                        {
                            sum2 = sum2 + KW_Wid[n];
                            ListContent.transform.SetParent(KWContents2.transform, false);
                            Debug.Log("2: " + sum2);
                        }
                        else
                        {
                            ListContent.transform.SetParent(KWContents3.transform, false);
                        }
                    }
                }
            }
            //firebase_LoadKW();
            //if (FirebaseManager.KWList == null)
            //{
            //    Debug.Log("KWList 못받아옴");
            //    Invoke("firebase_LoadKW", 0.1f);
            //}

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

