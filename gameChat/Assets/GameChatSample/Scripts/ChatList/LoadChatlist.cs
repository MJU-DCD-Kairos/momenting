using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FireStoreScript;
using Firebase.Firestore;
using System.Threading.Tasks;
using Firebase.Extensions;
using GameChatSample;

namespace LoadCL
{
    public class LoadChatlist : MonoBehaviour
    {

        //public static bool clickCLicon;

        public void OnMouseUpAsButton()
        {
            //clickCLicon = true;
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

                    if (RQList != null)
                    {
                        foreach (Dictionary<string, object> RQs in RequestList)
                        {
                            if (RQs["state"].ToString() == "N" || RQs["state"].ToString() == "C") //state가 N이나 C이면
                            {
                                RQList.Add(RQs[NewChatManager.NICKNAME].ToString()); //받은신청 리스트에 있는 유저 닉네임을 리스트에 추가
                            }
                            
                        }

                        for (int i = 0; i < RQList.Count; i++)
                        {
                            Debug.Log(RQList[i]);
                        }

                    }
                }

                else
                {
                    Debug.Log("XXXXX 받은신청 없음 XXXXX");
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
