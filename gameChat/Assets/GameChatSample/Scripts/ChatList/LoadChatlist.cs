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
        public static List<string> RQList = new List<string>(); //������û �ҷ��� �����ϱ� ���� ����Ʈ

        private void Start()
        {
            RQList.Clear();
            setRQList();
        }

        #region RQList

        
        public async Task setRQList() //������û(ê����Ʈ ����ȭ��) ����Ʈ ����
        {
            //����Ʈ ������ ����
            //if (0 < GameObject.Find("Group_Received").transform.childCount)
            //{
            //    for (int n = 0; n < (GameObject.Find("Group_Received").transform.childCount); n++)
            //    {
            //        GameObject.Destroy(GameObject.Find("Group_Received").transform.GetChild(n).gameObject);
            //    }
            //}
            CollectionReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ");
            List<object> myRQList = new List<object>() {"N", "C"};
            Query query = RQRef.WhereIn("state", myRQList); //state�� N�̳� C�� ���� ����

            if(query != null)
            {
                await query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Dictionary<string, object> docDictionary = doc.ToDictionary();
                        RQList.Add(docDictionary["nickName"].ToString()); //�г��� ����Ʈ�� �߰�
                        Debug.Log(docDictionary["nickName"].ToString() + "�� ����Ʈ�� �߰���");

                        //����Ʈ ������ ����
                        GameObject prefab = Resources.Load("Prefabs/List_Received") as GameObject;
                        GameObject badge = prefab.transform.GetChild(0).gameObject;
                        if (docDictionary["state"].ToString() == "C") //Ȯ���� ���� �ѹ��̶� ������
                        {
                            badge.SetActive(false); //���� ��Ȱ��ȭ
                        }
                        else
                        {
                            badge.SetActive(true); //���� Ȱ��ȭ
                        }

                        GameObject ui = Instantiate(prefab);

                        ui.transform.SetParent(GameObject.Find("Group_Received").transform, false);

                        //���� �г����� �������� �ؽ�Ʈ ������Ʈ�� �ֱ�
                        ui.transform.GetChild(3).GetComponent<Text>().text = docDictionary["nickName"].ToString();
                    }

                });
            }

            else
            {
                Debug.Log("XXXXX ������û ���� XXXXX");
            }

        }

        public GameObject SeeMoreParents;

        public void Onclick_SeeMore()
        {
            Load_SeeMore();
        }

        public async Task Load_SeeMore()
        {
            //����Ʈ ������ ����
            if (0 < GameObject.Find("Content_SeeMore").transform.childCount)
            {
                for (int n = 0; n < (GameObject.Find("Content_SeeMore").transform.childCount); n++)
                {
                    GameObject.Destroy(GameObject.Find("Content_SeeMore").transform.GetChild(n).gameObject);
                }
            }

            CollectionReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ");
            List<object> myRQList = new List<object>() { "N", "C" };
            Query query = RQRef.WhereIn("state", myRQList); //state�� N�̳� C�� ���� ����

            if (query != null)
            {
                await query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Dictionary<string, object> docDictionary = doc.ToDictionary();
                        //����Ʈ ������ ����
                        prefeb_SM = Instantiate(Resources.Load("Prefabs/List_Received_SeeMore") as GameObject);
                        prefeb_SM.transform.SetParent(GameObject.Find("Content_SeeMore").transform, false);

                        //�ؽ�Ʈ UI 
                        string RQsex = "";
                        if (docDictionary["sex"].ToString() == "1") { RQsex = "��"; } //���� ���ڿ��� �ѱ۷� �ٲ��ֱ�
                        else { RQsex = "��"; }

                        //string currentTime = System.DateTime.Now.ToString("h:mm:ss");
                        //string questTime = RQs["time"].ToString();
                        //Debug.Log(currentTime);
                        //Debug.Log(questTime);

                        //DateTime currentTime = DateTime.Parse();
                        //System.DateTime questTime = System.Convert.ToDateTime("2012/05/07 08:00"); // ���۽ð�
                        //System.DateTime currentTime = System.Convert.ToDateTime("2012/05/10 10:20"); // ����ð�( �Ϸ� �ð� )

                        //System.TimeSpan timeCal = currentTime - questTime; // �ð��� ���

                        //int timeCalDay = timeCal.Days;//��¥ ����
                        //int timeCalHour = timeCal.Hours; //�ð�����
                        //int timeCalMinute = timeCal.Minutes;// �� ����

                        //Debug.Log(timeCalDay);
                        //Debug.Log(timeCalHour);
                        //Debug.Log(timeCalMinute);



                        //System.DateTime time = System.DateTime.Now;
                        //Debug.Log(time.ToString("hh:mm tt")); // �ð� / �� / ��������
                        //Debug.Log(time.ToString("MM/dd/yyyy")); // ��

                        //Text Information = prefeb_SM.transform.GetChild(2).GetComponent<Text>();
                        GameObject Information = prefeb_SM.transform.GetChild(2).gameObject;
                        Information.transform.GetChild(0).GetComponent<Text>().text = docDictionary["nickName"].ToString(); //�г���
                        Information.transform.GetChild(1).GetComponent<Text>().text = docDictionary["age"].ToString() + " " + RQsex; //����, ����
                                                                                                                           //Information.transform.GetChild(2).GetComponent<Text>().text = ; //�ð�
                        prefeb_SM.transform.GetChild(3).GetComponent<Text>().text = docDictionary["Info"].ToString(); //���ټҰ�

                    }

                });
            }

            else
            {
                Debug.Log("XXXXX ������û ���� XXXXX");
            }

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
