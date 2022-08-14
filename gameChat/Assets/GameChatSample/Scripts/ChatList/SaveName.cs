using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using FireStoreScript;
using Firebase.Firestore;
using Firebase.Extensions;
using CLCM;

namespace RQProfile
{
    public class SaveName : MonoBehaviour
    {
        public Text nickname;
        public string RQ_nickname;
        //public string RQ_name;
        public string RQ_age;
        public string RQ_sex;
        public string RQ_mbti;
        public string RQ_ML;
        public string RQ_Info;
        public Dictionary<string, List<object>> KW = new Dictionary<string, List<object>>();

        //public GameObject RQprefab;
        public void onclick_saveNM()
        {
            KW.Clear();
            //chatlistSceneManager.Previous_Canvas = "ChatList_Main";
            RQ_nickname = nickname.text;
            RQprofile();
        }
        //private void Start()
        //{
        //    Debug.Log("프리팹 스크립트 실행됨");
        //}

        //private void Update()
        //{
        //    RQprefab.transform.GetComponent<Button>().onClick.AddListener(delegate { this.onclick_saveNM(); });
        //}
        async void RQprofile()
        {
            await get_RQprofile();
        }
        async Task get_RQprofile() //db에서 프로필 정보 가져오기
        {
            DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(RQ_nickname);
            await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, object> doc = snapshot.ToDictionary();
                //RQ_name = doc["name"] as string; //닉네임
                RQ_age = doc["age"] as string; //나이
                RQ_sex = doc["sex"] as string; //성별
                RQ_mbti = doc["mbti"] as string; //모래알유형
                RQ_ML = doc["mannerLevel"] as string; //매너등급
                RQ_Info = doc["Introduction"] as string; //한줄소개

                if ((Dictionary<string, object>)doc["KeyWord"] != null)
                    {
                        Dictionary<string, object> keywordList = (Dictionary<string, object>)doc["KeyWord"];

                        List<object> KWA = new List<object>();
                        List<object> KWB = new List<object>();
                        List<object> KWC = new List<object>();

                        foreach (KeyValuePair<string, object> pair in keywordList)
                        {
                            List<object> kwlist = (List<object>)keywordList[pair.Key];

                            for (int i = 0; i < kwlist.Count; i++)
                            {
                                if (pair.Key == "#ff8550")
                                {
                                    KWA.Add(kwlist[i]);
                                }
                                else if (pair.Key == "#7043c0")
                                {
                                    KWB.Add(kwlist[i]);
                                }
                                else if (pair.Key == "#001130")
                                {
                                    KWC.Add(kwlist[i]);
                                }
                            }

                        }
                        KW.Add("#ff8550", KWA);
                        KW.Add("#7043c0", KWB);
                        KW.Add("#001130", KWC);
                    }
                    else { Debug.Log("키워드 정보 없음"); }

                    set_RQprofile();
                }

                else
                {
                    Debug.Log("XXXXX 프로필 데이터 없음 XXXXX");
                }
            });
        }

        void set_RQprofile() //프로필 UI 세팅
        {
            GameObject RQprofile = GameObject.Find("ChatList").transform.Find("Profile_Received").gameObject; //받은신청 프로필 캔버스 활성화
            GameObject.Find("ChatList_Main").gameObject.SetActive(false);
            RQprofile.SetActive(true);

            //기본정보 세팅
            GameObject.Find("Txt_Name").GetComponent<Text>().text = RQ_nickname; //닉네임 세팅
            Debug.Log("닉네임: " + RQ_nickname);
            GameObject.Find("Txt_Age").GetComponent<Text>().text = RQ_age; //나이 세팅
            Debug.Log("나이: " + RQ_age);

            if (RQ_sex == "1") { RQ_sex = "남"; } //성별 숫자에서 한글로 바꿔주기
            else { RQ_sex = "여"; }
            GameObject.Find("Txt_Sex").GetComponent<Text>().text = RQ_sex; //성별 세팅
            Debug.Log("성별: " + RQ_sex);
            GameObject.Find("Txt_Introduction").GetComponent<Text>().text = RQ_Info; //한줄소개 세팅
            Debug.Log("한줄소개: " + RQ_Info);
            Debug.Log("매너등급: " + RQ_ML); //나중에 UI 추가한 후 처리


            //키워드 세팅
            Dictionary<string, List<object>> testDict = KW;
            foreach (string Key in testDict.Keys)
            {
                for (int l = 0; l < testDict[Key].Count; l++)
                {
                    GameObject ContentParents = GameObject.Find("List_ProfileKeword");
                    GameObject ListContent = Instantiate(Resources.Load("Prefabs/MyKeyword")) as GameObject;
                    ListContent.transform.SetParent(ContentParents.transform, false);

                    Color color;
                    ColorUtility.TryParseHtmlString(Key, out color);//""안에 DB에서 받아온 헥사코드 넣어서 rgb변환 후 찍음
                                                                    //키워드 카테고리 색상

                    ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().color = color;

                    //키워드 글자
                    ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Text>().text = testDict[Key][l].ToString();//"키워드 적용 테스트";여기에 DB에서 받아온 키워드를 string으로 찍음

                    //키워드 설명
                    ListContent.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
                }
            }

            //모래알유형 세팅
            Debug.Log("모래알등급: " + RQ_mbti);

        }
    }
}
