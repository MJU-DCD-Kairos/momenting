using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameChatSample;
using System.Threading.Tasks;
using FireStoreScript;
using Firebase.Firestore;
using Firebase.Extensions;
using Prefebs;

namespace CLCM
{
    public class chatlistSceneManager : MonoBehaviour
    {
        //스크립트 받아오기위한 타입 변수 선언
        gameSceneManager gSM;
        public Button backToHome;

        //그룹채팅 카드에 들어갈 게임오브젝트 참조
        public GameObject CListUI;

        //그룹채팅에 들어갈 정보 받아올 변수 선언
        public string GetID = "";//받아온channelID
        public string RoomTitleText = "샘플타이틀";
        public string RoomTimerText = "00:00";
        public string RoomContentsText = "콘텐츠내용을 입력";
        public int RoomNewMSGCount = 0;
        public string userNickName;


        public string id = "";

        //public static string Previous_Canvas;

        private void Awake()
        {

        }
        // Start is called before the first frame update
        void Start()
        {
            //don't destroy로 살려서 넘어온 게임씬매니저의 스크립트를 변수에 담음
            gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

            //Invoke("makeGCList", 0.03f);
            //버튼에 gSM의 로드씬 함수 리스너를 추가함
            backToHome.onClick.AddListener(gSM.LoadScene_Home);

            //makeGCList();

            //object cRname = "잔인한 면봉";
            //FireStoreScript.FirebaseManInager.CRnameDoubleCheck(cRname);

            InvokeRepeating("gotMyGClistInfo", 0f, 5f);
            userNickName = PlayerPrefs.GetString("GCName");
    }



        // Update is called once per frame
        void Update()
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


        //DB "gameChatRoom" Collection에 접근해서 Document별 member의 nickName에 접근하여
        //유저 닉네임과 동일한 값이 있는 Document의 id, 채널id, openTime을 가져옴
        //가져온 채널id로 최근 메시지를 가져옴
        //PlayerPrefs.GetString("GCN"); //앱이 시작할 때, 유저 정보를 로드하는 과정을 마친 후 그 변수를 사용해야함.
        public static List<string[]> gSlotList = new List<string[]>();
        public Dictionary<string, Text> gSlotMsgDict = new Dictionary<string, Text>();
        public string[] cInfoList;


        public void makeGCcard()
        {
            //Debug.Log("makeGCard됨");
            gotMyGClistInfo();
        }

        public async Task gotMyGClistInfo()
        {
            gSlotList.Clear();
            gSlotMsgDict.Clear();
            Query alldocQauery = FirebaseManager.db.Collection("gameChatRoom");

            QuerySnapshot alldocQauerySnapshot = await alldocQauery.GetSnapshotAsync();
            foreach (DocumentSnapshot docSnapShot in alldocQauerySnapshot.Documents)
            {
                Dictionary<string, object> docDictionary = docSnapShot.ToDictionary();
                List<object> memberList = (List<object>)docDictionary["member"];

                foreach (Dictionary<string, object> m in memberList)
                {

                    if (m["nickName"].ToString() == userNickName)
                    {

                        cInfoList = new string[3];

                        cInfoList[0] = docSnapShot.Id;

                        //Debug.Log("겟타입"+docDictionary["openTime"]);

                        cInfoList[1] = NewChatManager.getLostTime(docDictionary["openTime"] as string);

                        Debug.Log("DB시간 텍스트로 로드: "+NewChatManager.getLostTime(docDictionary["openTime"] as string));

                        cInfoList[2] = docDictionary["channelID"] as string;// NewChatManager.curMsg;
                        //Debug.Log(NewChatManager.getCurMSG(docDictionary["channelID"] as string));
                        gSlotList.Add(cInfoList);
                    }
                }
            }
            //Debug.Log("gotmygclistinfo끝남");
            makeGCcardList();
        }

        public void makeGCcardList()
        {
            //Debug.Log("makeGCcardList실행");
            //Debug.LogError(GameObject.Find("GCViewport").transform.childCount);
            if (0 < GameObject.Find("GCViewport").transform.childCount)
            {
                for (int n = 0; n < (GameObject.Find("GCViewport").transform.childCount); n++)
                {
                    GameObject.Destroy(GameObject.Find("GCViewport").transform.GetChild(n).gameObject);
                }
            }

            for (int i = 0; i < gSlotList.Count; i++)
            {
                //Debug.Log(gSlotList[i][0] + "/" + gSlotList[i][1] + "/" + gSlotList[i][2]);

                //프리팹 생성 후 뷰포트의 자식으로 설정
                GameObject ui = Instantiate(CListUI, GameObject.Find("GCViewport").GetComponent<RectTransform>());
                ui.transform.SetParent(GameObject.Find("GCViewport").transform);

                //채팅방 이름 넣기
                ui.transform.GetChild(3).GetComponent<Text>().text = gSlotList[i][0];

                //남은 시간 넣기
                ui.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = gSlotList[i][1];
                

                //최근 메시지 넣기
                ui.transform.GetChild(2).GetComponent<Text>().text = "";// gSlotList[i][2];
                

                gSlotMsgDict.Add(gSlotList[i][2], ui.transform.GetChild(2).GetComponent<Text>());
                NewChatManager.getCurMSG(gSlotList[i][2], OnRecvMsg);

            }
            //Debug.Log("makeGCcardList꾸ㅡㅌ남");
        }
        //public Text badge;
        //public int badgeN = 0;


        //현진추가_220905 1:1채팅
        public void makePCcardList()
        {
            //Debug.Log("makePCcardList실행");
            //Debug.LogError(GameObject.Find("PCViewport").transform.childCount);
            if (0 < GameObject.Find("PCViewport").transform.childCount)
            {
                for (int n = 0; n < (GameObject.Find("PCViewport").transform.childCount); n++)
                {
                    GameObject.Destroy(GameObject.Find("PCViewport").transform.GetChild(n).gameObject);
                }
            }

            for (int i = 0; i < pSlotList.Count; i++)
            {
                //Debug.Log(gSlotList[i][0] + "/" + gSlotList[i][1] + "/" + gSlotList[i][2]);

                //프리팹 생성 후 뷰포트의 자식으로 설정
                GameObject ui = Instantiate(CListUI, GameObject.Find("GCViewport").GetComponent<RectTransform>());
                ui.transform.SetParent(GameObject.Find("GCViewport").transform);

                //채팅방 이름 넣기
                ui.transform.GetChild(3).GetComponent<Text>().text = gSlotList[i][0];

                //남은 시간 넣기
                ui.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = gSlotList[i][1];


                //최근 메시지 넣기
                ui.transform.GetChild(2).GetComponent<Text>().text = "";// gSlotList[i][2];


                gSlotMsgDict.Add(gSlotList[i][2], ui.transform.GetChild(2).GetComponent<Text>());
                NewChatManager.getCurMSG(gSlotList[i][2], OnRecvMsg);

            }
            //Debug.Log("makeGCcardList꾸ㅡㅌ남");
        }



        void OnRecvMsg(string id, string msg)
        {
            gSlotMsgDict[id].text = msg;
           
        }


        public static void isEqualName()
        {
            for (int i = 0; i < gSlotList.Count; i++)
            {
                if (gSlotList[i][0] == getGCID.thisRoomName)
                {
                    gameSceneManager.chatRID = gSlotList[i][2];
                    //Debug.Log("#################전달ID ::" + gSlotList[i][2]);
                    gameSceneManager.chatRname = gSlotList[i][0];
                    //Debug.Log("#################전달채팅방이름 ::" + gSlotList[i][0]);
                    

                }
            }



            /*
            List<GameObject> gslotList = new List<GameObject>();
            public void makeRList()
            {
                GameObject ui = Instantiate(CListUI, GameObject.Find("GCViewport").GetComponent<RectTransform>());
                ui.transform.SetParent(GameObject.Find("GCViewport").transform);

                Debug.Log("### room_name ### : " + GameChatSample.NewChatManager.CurChatInfo[0]);
                ui.transform.GetChild(2).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[0]; //채팅방 이름
                ui.transform.GetChild(4).transform.GetChild(1).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[2]; //남은시간
                ui.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[4];//배지 수
                /*if (GameChatSample.NewChatManager.CurChatInfo[4] == "0") 
                {
                    ui.transform.GetChild(1).transform.gameObject.SetActive(false);
                }
                else
                {
                    ui.transform.GetChild(1).transform.gameObject.SetActive(true);
                }
                ui.transform.GetChild(3).GetComponent<Text>().text = GameChatSample.NewChatManager.CurChatInfo[1]; //라스트 채팅내용

                //CurChatInfo[0]이름 [1]내용 [2]생성시간 [3]생성날짜 [4]새로운메시지수

                gslotList.Add(ui);
            }

            public void GetCurrentChatCount(string id)
            {
                Debug.Log("GetCurrentChatCount시작 id는? : " + id);
                //GameChatSample.NewChatManager.getChannelID();//firebase에서 id를 가져와야함. 변수에 id를 string으로 담은 다음
                RoomNewMSGCount = GameChatSample.NewChatManager.setNewMSGCount(id);

            }

            public void getLostTime()
            {
                //string cTime = GameChatSample.NewChatManager.CList[0].created_at.Substring(11, 8);//생성시간
                string cTime= GameChatSample.NewChatManager.CurChatInfo[2];
                //string cDay = GameChatSample.NewChatManager.CList[0].created_at.Substring(0, 10);
                string cDay = GameChatSample.NewChatManager.CurChatInfo[3];
                string nTime = DateTime.Now.ToString("u").Substring(11, 8);//현재시간
                string nDay = DateTime.Now.ToString("u").Substring(0, 10);
                Debug.Log(cTime + "/" + cDay + "/" + nTime + "/" + nDay);
                TimeSpan goTime = Convert.ToDateTime(nTime) - Convert.ToDateTime(cTime);

                if (goTime.Days <= 0) 
                {
                    if (goTime.Hours == 0)
                    {
                        if (goTime.Minutes > 20)
                        {+
            string
                            RoomTimerText = "종료";
                        }
                        else
                        {
                            RoomTimerText = goTime.Minutes.ToString() + "분";
                        }
                    }
                    RoomTimerText = "종료";


                }

            }

            public void makeGCList()
            {
                id = GameChatSample.NewChatManager.CList[0].id.ToString().Replace(" ","");
                RoomContentsText = GameChatSample.NewChatManager.getCurMSG(id);
                RoomTitleText = GameChatSample.NewChatManager.getCurRoomName(id);
                GetCurrentChatCount(id);//배지 수 구하기


                Invoke("makeRList", 1f);


            }*/

        }

    }
}
