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
        //    Debug.Log("������ ��ũ��Ʈ �����");
        //}

        //private void Update()
        //{
        //    RQprefab.transform.GetComponent<Button>().onClick.AddListener(delegate { this.onclick_saveNM(); });
        //}
        async void RQprofile()
        {
            await get_RQprofile();
        }
        async Task get_RQprofile() //db���� ������ ���� ��������
        {
            DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(RQ_nickname);
            await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, object> doc = snapshot.ToDictionary();
                //RQ_name = doc["name"] as string; //�г���
                RQ_age = doc["age"] as string; //����
                RQ_sex = doc["sex"] as string; //����
                RQ_mbti = doc["mbti"] as string; //�𷡾�����
                RQ_ML = doc["mannerLevel"] as string; //�ųʵ��
                RQ_Info = doc["Introduction"] as string; //���ټҰ�

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
                    else { Debug.Log("Ű���� ���� ����"); }

                    set_RQprofile();
                }

                else
                {
                    Debug.Log("XXXXX ������ ������ ���� XXXXX");
                }
            });
        }

        void set_RQprofile() //������ UI ����
        {
            GameObject RQprofile = GameObject.Find("ChatList").transform.Find("Profile_Received").gameObject; //������û ������ ĵ���� Ȱ��ȭ
            GameObject.Find("ChatList_Main").gameObject.SetActive(false);
            RQprofile.SetActive(true);

            //�⺻���� ����
            GameObject.Find("Txt_Name").GetComponent<Text>().text = RQ_nickname; //�г��� ����
            Debug.Log("�г���: " + RQ_nickname);
            GameObject.Find("Txt_Age").GetComponent<Text>().text = RQ_age; //���� ����
            Debug.Log("����: " + RQ_age);

            if (RQ_sex == "1") { RQ_sex = "��"; } //���� ���ڿ��� �ѱ۷� �ٲ��ֱ�
            else { RQ_sex = "��"; }
            GameObject.Find("Txt_Sex").GetComponent<Text>().text = RQ_sex; //���� ����
            Debug.Log("����: " + RQ_sex);
            GameObject.Find("Txt_Introduction").GetComponent<Text>().text = RQ_Info; //���ټҰ� ����
            Debug.Log("���ټҰ�: " + RQ_Info);
            Debug.Log("�ųʵ��: " + RQ_ML); //���߿� UI �߰��� �� ó��


            //Ű���� ����
            Dictionary<string, List<object>> testDict = KW;
            foreach (string Key in testDict.Keys)
            {
                for (int l = 0; l < testDict[Key].Count; l++)
                {
                    GameObject ContentParents = GameObject.Find("List_ProfileKeword");
                    GameObject ListContent = Instantiate(Resources.Load("Prefabs/MyKeyword")) as GameObject;
                    ListContent.transform.SetParent(ContentParents.transform, false);

                    Color color;
                    ColorUtility.TryParseHtmlString(Key, out color);//""�ȿ� DB���� �޾ƿ� ����ڵ� �־ rgb��ȯ �� ����
                                                                    //Ű���� ī�װ� ����

                    ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().color = color;

                    //Ű���� ����
                    ListContent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Text>().text = testDict[Key][l].ToString();//"Ű���� ���� �׽�Ʈ";���⿡ DB���� �޾ƿ� Ű���带 string���� ����

                    //Ű���� ����
                    ListContent.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
                }
            }

            //�𷡾����� ����
            Debug.Log("�𷡾˵��: " + RQ_mbti);

        }
    }
}
