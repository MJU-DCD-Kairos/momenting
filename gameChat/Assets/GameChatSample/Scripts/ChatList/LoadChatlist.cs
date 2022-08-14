using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FireStoreScript;
using Firebase.Firestore;
using System.Threading.Tasks;
using Firebase.Extensions;
using GameChatSample;
using System;
using RQProfile;

namespace LoadCL
{
    public class LoadChatlist : MonoBehaviour
    {

        //public static bool clickCLicon;
        public GameObject RQListUI;
        public GameObject prefeb_SM;
        //public void OnMouseUpAsButton()
        //{
        //    //clickCLicon = true;
        //    getRQList();
        //}

        private void Start()
        {
            getRQList();
        }

        #region RQList

        public static List<string> RQList = new List<string>(); //받은신청 불러와 저장하기 위한 리스트
        
        public async Task getRQList() //디비에서 받은신청 리스트 가져오기
        {
            DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN);
            await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, object> doc = snapshot.ToDictionary();
                    List<object> RequestList = (List<object>)doc["RQ"];

                    if (RequestList != null)
                    {
                        foreach (Dictionary<string, object> RQs in RequestList)
                        {
                            if (RQs["state"].ToString() == "N" || RQs["state"].ToString() == "C") //state가 N이나 C이면
                            {
                                RQList.Add(RQs[NewChatManager.NICKNAME].ToString()); //받은신청 리스트에 있는 유저 닉네임을 리스트에 추가

                                Debug.Log(RQs[NewChatManager.NICKNAME].ToString() + "를 리스트에 추가함");

                                //리스트 프리팹 생성
                                GameObject prefeb = Resources.Load("Prefabs/List_Received") as GameObject;
                                GameObject badge = prefeb.transform.GetChild(0).gameObject;
                                if (RQs["state"].ToString() == "C") //확인한 적이 한번이라도 있으면
                                {
                                    badge.SetActive(false); //뱃지 비활성화
                                }
                                GameObject ui = Instantiate(prefeb);

                                ui.transform.SetParent(GameObject.Find("Group_Received").transform, false);

                                //유저 닉네임을 프리팹의 텍스트 컴포넌트에 넣기
                                ui.transform.GetChild(3).GetComponent<Text>().text = RQs[NewChatManager.NICKNAME].ToString();

                            }
                            
                        }
                    }
                }

                else
                {
                    Debug.Log("XXXXX 받은신청 없음 XXXXX");
                }
            });
        }

        public GameObject SeeMoreParents;

        public void Onclick_SeeMore()
        {
            Load_SeeMore();
        }

        public async Task Load_SeeMore()
        {
            DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN);
            await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, object> doc = snapshot.ToDictionary();
                    List<object> RequestList = (List<object>)doc["RQ"];


                    if (RQList != null)
                    {
                        foreach (Dictionary<string, object> RQs in RequestList)
                        {
                            if (RQs["state"].ToString() == "N" || RQs["state"].ToString() == "C") //state가 N이나 C이면
                            {
                                
                                //리스트 프리팹 생성
                                prefeb_SM = Instantiate(Resources.Load("Prefabs/List_Received_SeeMore") as GameObject);
                                prefeb_SM.transform.SetParent(GameObject.Find("Content_SeeMore").transform, false);

                                //텍스트 UI 
                                string RQsex = "";
                                if (RQs["sex"].ToString() == "1") { RQsex = "남"; } //성별 숫자에서 한글로 바꿔주기
                                else { RQsex = "여"; }

                                //string currentTime = System.DateTime.Now.ToString("h:mm:ss");
                                //string questTime = RQs["time"].ToString();
                                //Debug.Log(currentTime);
                                //Debug.Log(questTime);

                                //DateTime currentTime = DateTime.Parse();
                                //System.DateTime questTime = System.Convert.ToDateTime("2012/05/07 08:00"); // 시작시간
                                //System.DateTime currentTime = System.Convert.ToDateTime("2012/05/10 10:20"); // 현재시간( 완료 시간 )

                                //System.TimeSpan timeCal = currentTime - questTime; // 시간차 계산

                                //int timeCalDay = timeCal.Days;//날짜 차이
                                //int timeCalHour = timeCal.Hours; //시간차이
                                //int timeCalMinute = timeCal.Minutes;// 분 차이

                                //Debug.Log(timeCalDay);
                                //Debug.Log(timeCalHour);
                                //Debug.Log(timeCalMinute);



                                //System.DateTime time = System.DateTime.Now;
                                //Debug.Log(time.ToString("hh:mm tt")); // 시간 / 분 / 오전오후
                                //Debug.Log(time.ToString("MM/dd/yyyy")); // 월

                                //Text Information = prefeb_SM.transform.GetChild(2).GetComponent<Text>();
                                GameObject Information = prefeb_SM.transform.GetChild(2).gameObject;
                                Information.transform.GetChild(0).GetComponent<Text>().text = RQs["nickName"].ToString(); //닉네임
                                Information.transform.GetChild(1).GetComponent<Text>().text = RQs["age"].ToString() + " " + RQsex; //나이, 성별
                                //Information.transform.GetChild(2).GetComponent<Text>().text = ; //시간
                                prefeb_SM.transform.GetChild(3).GetComponent<Text>().text = RQs["Info"].ToString(); //한줄소개

                            }

                        }
                    }
                }
            });
        }
            
        
        //리스트를 넣어주는 부모 개체
        //public GameObject ContentParents;

        //async void setRQList() //채팅방리스트에 받은신청 띄우기
        //{
        //    //db에서 받아온 Dict<string, List<string>> 형태를 받아옴
        //    Dictionary<string, List<object>> testDict = FirebaseManager.KWList;

        //    //Dictionary의 키를 돌면서 키가 가진 키워드 리스트 길이만큼 오브젝트 생성, 해당 내용 대입
        //    foreach (string Key in testDict.Keys)
        //    {
        //        for (int l = 0; l < testDict[Key].Count; l++)
        //        {
        //            GameObject ListContent = Instantiate(Resources.Load("Prefabs/MyKeyword")) as GameObject;
        //            ListContent.transform.SetParent(ContentParents.transform, false);

        //            Color color;
        //            ColorUtility.TryParseHtmlString(Key, out color);//""안에 DB에서 받아온 헥사코드 넣어서 rgb변환 후 찍음
        //                                                            //키워드 카테고리 색상


        //            ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().color = color;

        //            //키워드 글자
        //            ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Text>().text = testDict[Key][l].ToString();//"키워드 적용 테스트";여기에 DB에서 받아온 키워드를 string으로 찍음

        //            //키워드 설명
        //            ListContent.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
        //        }
        //    }

        //}
        #endregion
    }
}
