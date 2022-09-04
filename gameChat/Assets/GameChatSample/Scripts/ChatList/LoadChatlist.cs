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

        public GameObject RQListUI;
        public GameObject prefeb_SM;
        public static List<string> RQList = new List<string>(); //받은신청 불러와 저장하기 위한 리스트
        public string RequestTime;
        public string time_text;
        private void Start()
        {
            RQList.Clear();
            //setRQList();
            deleteDoc();
        }

        #region RQList

        async void deleteDoc()
        {
            CollectionReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ");
            Query docs = RQRef.OrderByDescending("time"); //state가 N이나 C인 문서 쿼리
            if (docs != null)
            {
                await docs.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Dictionary<string, object> docDictionary = doc.ToDictionary();
                        RequestTime = docDictionary["time"].ToString();
                        calculate_time();

                        if (time_text == "7일 지남")
                        {
                            //RQRef.Document(docDictionary["nickName"].ToString()).UpdateAsync("timeCal", "7일 지남");
                            RQRef.Document(docDictionary["nickName"].ToString()).DeleteAsync();
                        }

                    }
                });

                await setRQList();
            }

            else
            {
                Debug.Log("문서 없음");
            }
        }
        public async Task setRQList() //받은신청(챗리스트 메인화면) 리스트 생성
        {
            //리스트 프리팹 삭제
            //if (0 < GameObject.Find("Group_Received").transform.childCount)
            //{
            //    for (int n = 0; n < (GameObject.Find("Group_Received").transform.childCount); n++)
            //    {
            //        GameObject.Destroy(GameObject.Find("Group_Received").transform.GetChild(n).gameObject);
            //    }
            //}
            CollectionReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ");
            //List<object> myRQList = new List<object>() {"N", "C"};
            Query query = RQRef.OrderByDescending("time"); //state가 N이나 C인 문서 쿼리

            if(query != null)
            {
                await query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Dictionary<string, object> docDictionary = doc.ToDictionary();
                        //RQList.Add(docDictionary["nickName"].ToString()); //닉네임 리스트에 추가
                        //Debug.Log(docDictionary["nickName"].ToString() + "를 리스트에 추가함");

                        if((docDictionary["state"].ToString() == "C") || (docDictionary["state"].ToString() == "N"))
                        {
                            //리스트 프리팹 생성
                            GameObject prefab = Resources.Load("Prefabs/List_Received") as GameObject;
                            GameObject badge = prefab.transform.GetChild(0).gameObject;
                            if (docDictionary["state"].ToString() == "C") //확인한 적이 한번이라도 있으면
                            {
                                badge.SetActive(false); //뱃지 비활성화
                            }
                            else
                            {
                                badge.SetActive(true); //뱃지 활성화
                            }

                            GameObject ui = Instantiate(prefab);

                            ui.transform.SetParent(GameObject.Find("Group_Received").transform, false);

                            //유저 닉네임을 프리팹의 텍스트 컴포넌트에 넣기
                            ui.transform.GetChild(3).GetComponent<Text>().text = docDictionary["nickName"].ToString();
                        }
                        
                    }

                });
            }

            else
            {
                Debug.Log("XXXXX 받은신청 없음 XXXXX");
            }

        }

        public GameObject SeeMoreParents;

        public void Onclick_SeeMore()
        {
            Load_SeeMore();
        }

        public async Task Load_SeeMore()
        {
            //리스트 프리팹 삭제
            if (0 < GameObject.Find("Content_SeeMore").transform.childCount)
            {
                for (int n = 0; n < (GameObject.Find("Content_SeeMore").transform.childCount); n++)
                {
                    GameObject.Destroy(GameObject.Find("Content_SeeMore").transform.GetChild(n).gameObject);
                }
            }

            CollectionReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ");
            //List<object> myRQList = new List<object>() { "N", "C" };
            Query query = RQRef.OrderByDescending("time"); //state가 N이나 C인 문서 쿼리

            if (query != null)
            {
                await query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Dictionary<string, object> docDictionary = doc.ToDictionary();

                        if ((docDictionary["state"].ToString() == "C") || (docDictionary["state"].ToString() == "N"))
                        {
                            //리스트 프리팹 생성
                            prefeb_SM = Instantiate(Resources.Load("Prefabs/List_Received_SeeMore") as GameObject);
                            prefeb_SM.transform.SetParent(GameObject.Find("Content_SeeMore").transform, false);

                            //텍스트 UI 
                            string RQsex = "";
                            if (docDictionary["sex"].ToString() == "1") { RQsex = "남"; } //성별 숫자에서 한글로 바꿔주기
                            else { RQsex = "여"; }

                            RequestTime = docDictionary["time"].ToString(); //신청 시간
                            calculate_time();

                            GameObject Information = prefeb_SM.transform.GetChild(2).gameObject;
                            Information.transform.GetChild(0).GetComponent<Text>().text = docDictionary["nickName"].ToString(); //닉네임
                            Information.transform.GetChild(1).GetComponent<Text>().text = docDictionary["age"].ToString() + " " + RQsex; //나이, 성별
                            Information.transform.GetChild(2).GetComponent<Text>().text = time_text; //시간
                            prefeb_SM.transform.GetChild(3).GetComponent<Text>().text = docDictionary["Info"].ToString(); //한줄소개
                        }
                        
                    }

                });
            }

            else
            {
                Debug.Log("XXXXX 받은신청 없음 XXXXX");
            }

        }

        void calculate_time()
        {

            string currentTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            System.DateTime StartDate = System.Convert.ToDateTime(RequestTime); //신청한 시간
            System.DateTime EndDate = System.Convert.ToDateTime(currentTime); //현재 시간
            Debug.Log("DB에 저장된 시간: " + RequestTime);
            Debug.Log("변환한 DB 시간: " + StartDate);
            Debug.Log("변환한 현재 시간: " + EndDate);

            System.TimeSpan timeCal = EndDate - StartDate; //시간차 계산
            int timeCalDay = timeCal.Days; //날짜 차이
            int timeCalHour = timeCal.Hours; //시간 차이
            int timeCalMinute = timeCal.Minutes; //분 차이
            int timeCalSecond = timeCal.Seconds; //초 차이

            Debug.Log("날짜 차이: " + timeCalDay);
            Debug.Log("시간 차이: " + timeCalHour);
            Debug.Log("분 차이: " + timeCalMinute);
            Debug.Log("초 차이: " + timeCalSecond);

            int t;
            int h;
            int m;
            int s;

            if ((timeCalDay > 0) && (timeCalDay < 7))
            {
                time_text = timeCalDay + "일 전";
            }
            else if (timeCalDay > 6)
            {
                time_text = "7일 지남";
            }
            else
            {
                if((timeCalHour > 0) && (timeCalHour < 12))
                {
                    time_text = timeCalHour + "시간 전";
                }
                else
                {
                    if((timeCalMinute > 0) && (timeCalMinute < 60))
                    {
                        time_text = timeCalMinute + "분 전";
                    }
                    else
                    {
                        time_text = timeCalSecond + "초 전";
                    }
                }
            }
        }
        #endregion


        public async void RQcheck() //더보기 페이지에서 백버튼 클릭 시 state 확인해서 뱃지 비활성화
        {
            for (int n = 0; n < (GameObject.Find("Group_Received").transform.childCount); n++)
            {
                string RQname = GameObject.Find("Group_Received").transform.GetChild(n).transform.Find("Text_name").GetComponent<Text>().text;
                DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ").Document(RQname);
                await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    DocumentSnapshot snapshot = task.Result;
                    Dictionary<string, object> doc = snapshot.ToDictionary();
                    if (doc["state"] as string == "C")
                    {
                        GameObject badge = GameObject.Find("Group_Received").transform.GetChild(n).transform.GetChild(0).gameObject;
                        if (badge.activeInHierarchy) { badge.SetActive(false); } //뱃지가 활성화되어 있으면 비활성화 시킴
                        //if (badge.activeInHierarchy) etActive(false); }
                    }

                });
            }
        }
    }
}
