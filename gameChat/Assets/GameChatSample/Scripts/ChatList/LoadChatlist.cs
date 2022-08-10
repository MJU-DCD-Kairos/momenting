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

        public static List<string> RQList = new List<string>(); //������û �ҷ��� �����ϱ� ���� ����Ʈ
        
        public async Task getRQList() //��񿡼� ������û ����Ʈ ��������
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
                            if (RQs["state"].ToString() == "N" || RQs["state"].ToString() == "C") //state�� N�̳� C�̸�
                            {
                                RQList.Add(RQs[NewChatManager.NICKNAME].ToString()); //������û ����Ʈ�� �ִ� ���� �г����� ����Ʈ�� �߰�
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
                    Debug.Log("XXXXX ������û ���� XXXXX");
                }
            });
        }

        //����Ʈ�� �־��ִ� �θ� ��ü
        //public GameObject ContentParents;

        //async void setRQList() //ä�ù渮��Ʈ�� ������û ����
        //{
        //    //db���� �޾ƿ� Dict<string, List<string>> ���¸� �޾ƿ�
        //    Dictionary<string, List<object>> testDict = FirebaseManager.KWList;

        //    //Dictionary�� Ű�� ���鼭 Ű�� ���� Ű���� ����Ʈ ���̸�ŭ ������Ʈ ����, �ش� ���� ����
        //    foreach (string Key in testDict.Keys)
        //    {
        //        for (int l = 0; l < testDict[Key].Count; l++)
        //        {
        //            GameObject ListContent = Instantiate(Resources.Load("Prefabs/MyKeyword")) as GameObject;
        //            ListContent.transform.SetParent(ContentParents.transform, false);

        //            Color color;
        //            ColorUtility.TryParseHtmlString(Key, out color);//""�ȿ� DB���� �޾ƿ� ����ڵ� �־ rgb��ȯ �� ����
        //                                                            //Ű���� ī�װ� ����


        //            ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().color = color;

        //            //Ű���� ����
        //            ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Text>().text = testDict[Key][l].ToString();//"Ű���� ���� �׽�Ʈ";���⿡ DB���� �޾ƿ� Ű���带 string���� ����

        //            //Ű���� ����
        //            ListContent.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
        //        }
        //    }

        //}
        #endregion
    }
}
