using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameChatUnity;
using AS;
using CM;
using FireStoreScript;
using Firebase.Firestore;
using GameChatSample;

namespace groupchatManager
{
    public class groupchatSceneManager : MonoBehaviour
    {
        //스크립트 받아오기위한 타입 변수 선언
        gameSceneManager gSM;
        [SerializeField]
        ChatManager chatManager;

        public Button backToChatList;


        public Text ThisCRoomNameTitle;
        public RectTransform ContentRect;


        //불러오려는 메시지가 마지막 메시지랑 같은지 판별하기 위한 전역변수 선언
        public Message LastMSG;

        //내 말풍선 상대 말풍선 가리기 참조 위한 선언
        AreaScript LastArea;
        public GameObject MyArea, ElseArea;
        public Scrollbar scrollBar;

        public static List<string> chatRoom = new List<string>(); //채팅방 정보 저장하기 위한 리스트
        public int mynameIdx; //내 닉네임을 리스트에서 제거하기 위해 선언

        //채팅방 실시간 남은시간 업데이트
        public Text timeText;
        //채팅방 시간 종료 시 인풋필드 비활성화
        public InputField messageIF;
        public static float sec =0f;
        [Header("elseProfile")]
        public Text elseName1;
        public Text elseAge1;
        public Text elseSex1;
        public Text elseMbti1;
        public Text elseIntro1;

        public static Text elseName;
        public static Text elseAge;
        public static Text elseSex;
        public static Text elseMbti;
        public static Text elseIntro;





        // Start is called before the first frame update
        void Start()
        {
            //don't destroy로 살려서 넘어온 게임씬매니저의 스크립트를 변수에 담음
            gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();


            //버튼에 gSM의 로드씬 함수 리스너를 추가함
            backToChatList.onClick.AddListener(gSM.LoadScene_ChatList);

            //해당 채널을 벗어날 때, 구독해제
            //backToChatList.onClick.AddListener(gSM.LoadScene_ChatList);

            //메시지를 로드하며 필요한 정보(채팅방 이름, 스크롤뷰 부모 개체 찾아옴)
            ThisCRoomNameTitle.text = gameSceneManager.chatRname;

            StartCoroutine("TestMSG", gameSceneManager.chatRID);

            Invoke("ScrollDown", 1f);
            LoadUsersData();

            

        }
        void update()
        {
            // Update is called once per frame
            if (Application.platform == RuntimePlatform.Android)  // 플렛폼 정보 .
            {
                if (Input.GetKey(KeyCode.Escape)) // 키 눌린 코드 신호를 받아오는것.
                {
                    PlayerPrefs.SetString("LastMSGID", GameObject.Find("CRCode").name);
                    SceneManager.LoadScene("ChatList"); // 씬으로 이동 .
                                                        //Application.Quit(); // 씬 종료 .(나가기)            위씬으로 이동이나 종료기능 둘중하나 원하시는것을 사용하시면 됩니다.
                }
            }

            gotTime();

        }

        async void LoadUsersData()
        {
            DocumentReference reportRef = FirebaseManager.db.Collection("gameChatRoom").Document(gameSceneManager.chatRname);//채팅방db참조

            await reportRef.GetSnapshotAsync().ContinueWith(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, object> doc = snapshot.ToDictionary();
                    List<object> mList = (List<object>)doc[NewChatManager.MEMBER];
                    chatRoom.Add(gameSceneManager.chatRname);//리스트에 채팅방이름 추가
                    chatRoom.Add(gameSceneManager.chatRID);//리스트에 채팅방아이디 추가

                    foreach (Dictionary<string, object> CM in mList)
                    {
                        chatRoom.Add(CM[NewChatManager.NICKNAME].ToString()); //리스트에 채팅방에 참여한 유저 이름 추가
                    }

                    if (chatRoom != null)
                    {

                        for (int i = 0; i < chatRoom.Count; i++)
                        {
                            Debug.Log(chatRoom[i]);
                            if (chatRoom[i] == PlayerPrefs.GetString("GCName")) { mynameIdx = i; }
                        }
                        chatRoom.RemoveAt(mynameIdx); //내 닉네임을 리스트에서 삭제
                    }
                }
                else
                {
                    return;
                }
            });

        }

        void ScrollDown() => scrollBar.value = 0;

        //이전 메시지를 가져오는 함수
        public IEnumerator TestMSG(string id)
        {
            //마지막 채팅을 받아옴
            GameChat.getMessages(id, 0, 1, "", "", "", (List<Message> Messages, GameChatException Exception) =>
            {

                if (Exception != null)
                {
                // Error 핸들링
                return;
                }


                foreach (Message elem in Messages)
                {
                    LastMSG = elem;
                    Debug.Log(LastMSG.ToString());
                }
            });


            GameChat.getMessages(id, 0, 200, "", "", "asc", (List<Message> Messages, GameChatException Exception) =>
            {

                if (Exception != null)
                {
                // Error 핸들링
                return;
                }

                foreach (Message elem in Messages)
                {
                //if (LastMSG.message_id != elem.message_id)
                {
                    //Debug.LogError("@###@#@#@#@#@" + elem.content.ToString());
                    if (GameChatSample.SampleGlobalData.G_User.id == elem.sender.id)
                        {
                            chatManager.Chat(true, elem.content, "나", elem.created_at, null);
                        //Chat(bool isSend, string text, string user, Texture2D picture)

                        //AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                        //Area.transform.SetParent(ContentRect.transform, false);
                        //Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                        //Area.TextRect.GetComponent<Text>().text = elem.content;
                        ////Debug.Log(elem.content);
                        //Area.TimeText.text = elem.created_at;

                    }
                        else
                        {
                            chatManager.Chat(false, elem.content, elem.sender.id, elem.created_at, null);
                        //AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                        //Area2.transform.SetParent(ContentRect.transform, false);
                        //Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                        //Area2.TextRect.GetComponent<Text>().text = elem.content;
                        //Area2.UserText.text = elem.sender.name;
                        //Area2.TimeText.text = elem.created_at;

                    }

                    }
                //else
                //{
                //    if (GameChatSample.SampleGlobalData.G_User.id == elem.sender.id)
                //    {
                //        AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                //        Area.transform.SetParent(ContentRect.transform, false);
                //        Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                //        Area.TextRect.GetComponent<Text>().text = elem.content;
                //        Debug.Log(elem.content);
                //        Area.TimeText.text = elem.created_at;

                //    }
                //    else
                //    {
                //        AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                //        Area2.transform.SetParent(ContentRect.transform, false);
                //        Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                //        Area2.TextRect.GetComponent<Text>().text = elem.content;
                //        Area2.UserText.text = elem.sender.name;
                //        Area2.TimeText.text = elem.created_at;

                //    }
                //    break;
                //}
            }
            });
            yield return null;
        }

        public void gotTime()
        {
            TimeSpan time = DateTime.Now - Convert.ToDateTime(gameSceneManager.oTime);
            if (time.Minutes > 20f)
            {
                timeText.text = "종료";
                messageIF.interactable = false;

            }
            else
            {
                timeText.text = time.Minutes.ToString();
                sec = time.Minutes * 60f;

            }
        }

    }
}
