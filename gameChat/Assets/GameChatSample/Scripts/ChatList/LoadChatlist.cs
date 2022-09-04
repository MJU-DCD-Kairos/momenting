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
            Query docs = RQRef.OrderByDescending("time"); //state�� N�̳� C�� ���� ����
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

                        if (time_text == "7�� ����")
                        {
                            //RQRef.Document(docDictionary["nickName"].ToString()).UpdateAsync("timeCal", "7�� ����");
                            RQRef.Document(docDictionary["nickName"].ToString()).DeleteAsync();
                        }

                    }
                });

                await setRQList();
            }

            else
            {
                Debug.Log("���� ����");
            }
        }
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
            //List<object> myRQList = new List<object>() {"N", "C"};
            Query query = RQRef.OrderByDescending("time"); //state�� N�̳� C�� ���� ����

            if(query != null)
            {
                await query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Dictionary<string, object> docDictionary = doc.ToDictionary();
                        //RQList.Add(docDictionary["nickName"].ToString()); //�г��� ����Ʈ�� �߰�
                        //Debug.Log(docDictionary["nickName"].ToString() + "�� ����Ʈ�� �߰���");

                        if((docDictionary["state"].ToString() == "C") || (docDictionary["state"].ToString() == "N"))
                        {
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
            //List<object> myRQList = new List<object>() { "N", "C" };
            Query query = RQRef.OrderByDescending("time"); //state�� N�̳� C�� ���� ����

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
                            //����Ʈ ������ ����
                            prefeb_SM = Instantiate(Resources.Load("Prefabs/List_Received_SeeMore") as GameObject);
                            prefeb_SM.transform.SetParent(GameObject.Find("Content_SeeMore").transform, false);

                            //�ؽ�Ʈ UI 
                            string RQsex = "";
                            if (docDictionary["sex"].ToString() == "1") { RQsex = "��"; } //���� ���ڿ��� �ѱ۷� �ٲ��ֱ�
                            else { RQsex = "��"; }

                            RequestTime = docDictionary["time"].ToString(); //��û �ð�
                            calculate_time();

                            GameObject Information = prefeb_SM.transform.GetChild(2).gameObject;
                            Information.transform.GetChild(0).GetComponent<Text>().text = docDictionary["nickName"].ToString(); //�г���
                            Information.transform.GetChild(1).GetComponent<Text>().text = docDictionary["age"].ToString() + " " + RQsex; //����, ����
                            Information.transform.GetChild(2).GetComponent<Text>().text = time_text; //�ð�
                            prefeb_SM.transform.GetChild(3).GetComponent<Text>().text = docDictionary["Info"].ToString(); //���ټҰ�
                        }
                        
                    }

                });
            }

            else
            {
                Debug.Log("XXXXX ������û ���� XXXXX");
            }

        }

        void calculate_time()
        {

            string currentTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            System.DateTime StartDate = System.Convert.ToDateTime(RequestTime); //��û�� �ð�
            System.DateTime EndDate = System.Convert.ToDateTime(currentTime); //���� �ð�
            Debug.Log("DB�� ����� �ð�: " + RequestTime);
            Debug.Log("��ȯ�� DB �ð�: " + StartDate);
            Debug.Log("��ȯ�� ���� �ð�: " + EndDate);

            System.TimeSpan timeCal = EndDate - StartDate; //�ð��� ���
            int timeCalDay = timeCal.Days; //��¥ ����
            int timeCalHour = timeCal.Hours; //�ð� ����
            int timeCalMinute = timeCal.Minutes; //�� ����
            int timeCalSecond = timeCal.Seconds; //�� ����

            Debug.Log("��¥ ����: " + timeCalDay);
            Debug.Log("�ð� ����: " + timeCalHour);
            Debug.Log("�� ����: " + timeCalMinute);
            Debug.Log("�� ����: " + timeCalSecond);

            int t;
            int h;
            int m;
            int s;

            if ((timeCalDay > 0) && (timeCalDay < 7))
            {
                time_text = timeCalDay + "�� ��";
            }
            else if (timeCalDay > 6)
            {
                time_text = "7�� ����";
            }
            else
            {
                if((timeCalHour > 0) && (timeCalHour < 12))
                {
                    time_text = timeCalHour + "�ð� ��";
                }
                else
                {
                    if((timeCalMinute > 0) && (timeCalMinute < 60))
                    {
                        time_text = timeCalMinute + "�� ��";
                    }
                    else
                    {
                        time_text = timeCalSecond + "�� ��";
                    }
                }
            }
        }
        #endregion


        public async void RQcheck() //������ ���������� ���ư Ŭ�� �� state Ȯ���ؼ� ���� ��Ȱ��ȭ
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
                        if (badge.activeInHierarchy) { badge.SetActive(false); } //������ Ȱ��ȭ�Ǿ� ������ ��Ȱ��ȭ ��Ŵ
                        //if (badge.activeInHierarchy) etActive(false); }
                    }

                });
            }
        }
    }
}
