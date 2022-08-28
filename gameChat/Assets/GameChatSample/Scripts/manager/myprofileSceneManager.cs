using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FireStoreScript;
using System.Text;
using System;
using editprofile;
using System.Threading.Tasks;

namespace myprofile
{
    public class myprofileSceneManager : MonoBehaviour
    {
        //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
        gameSceneManager gSM;
        public Button backToHome;
        public Button goToChatList;
        public Button goToSetting;
        public Button goToTest;
        public string GCN;
        public Text txtName;
        public Text txtAge;
        public Text txtIntro;
        public Text txtSex;
        

        //Ű���� �ҷ��������� ������ ����
        //public GameObject KWlistPrefabs;
        
        public GameObject KWParents; //Ű���� ����Ʈ 4���� ���� �־��ִ� �θ� ��ü
        public GameObject KWContents0; //Ű���� ����Ʈ 1��° ��
        public GameObject KWContents1; //Ű���� ����Ʈ 2��° ��
        public GameObject KWContents2; //Ű���� ����Ʈ 3��° ��
        public GameObject KWContents3; //Ű���� ����Ʈ 4��° ��







        void Awake()
        {
            GCN = "";
            GCN = PlayerPrefs.GetString("GCName");
            Debug.Log(GCN + "�ҷ���");
        }
        // Start is called before the first frame update
        void Start()
        {
            //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
            gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

            //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
            backToHome.onClick.AddListener(gSM.LoadScene_Home);
            goToChatList.onClick.AddListener(gSM.LoadScene_ChatList);
            goToSetting.onClick.AddListener(gSM.LoadScene_Setting);
            goToTest.onClick.AddListener(gSM.LoadScene_TypeTest);
            
            //������ �̸� �޾ƿ���   
            txtName.text = GCN;
            txtAge.text = FirebaseManager.age;
            txtIntro.text = FirebaseManager.myintroduction;
            if (FirebaseManager.sex == 1)
            {
                txtSex.text = "��";
            }
            else
            {
                txtSex.text = "��";
            }

            setUserKW();
        }

        // Update is called once per frame
        void Update()  //������Ʈ ������ �־������ .
        {
            if (Application.platform == RuntimePlatform.Android)  // �÷��� ���� .
            {
                if (Input.GetKey(KeyCode.Escape)) // Ű ���� �ڵ� ��ȣ�� �޾ƿ��°�.
                {
                    SceneManager.LoadScene("Home"); // ������ �̵� .
                                                    //Application.Quit(); // �� ���� .(������)            �������� �̵��̳� ������ �����ϳ� ���Ͻô°��� ����Ͻø� �˴ϴ�.
                }
            }
        }

        //���� �� ������ ��ε�
        public void LoadProfile() 
        {
            if (MyProfileEditManager.EDIT_INTRO == null)
            {
                Invoke("LoadProfile", 0.1f);
            }
            else
            {
                Debug.Log("������ �� == " + MyProfileEditManager.EDIT_INTRO);
                txtIntro.text = MyProfileEditManager.EDIT_INTRO;
                //firebase_LoadKW();
                //if (FirebaseManager.KWList == null)
                //{
                //    Invoke("firebase_LoadKW", 0.1f);
                //}
                //else
                //{
                //    setUserKW();
                //}
                
            }
        }

        public void firebase_LoadKW()
        {
            GameObject.FindGameObjectWithTag("firebaseManager").GetComponent<FirebaseManager>().LoadKW();
            if (FirebaseManager.KWList == null)
            {
                Debug.Log("KWList ���޾ƿ�");
                Invoke("firebase_LoadKW", 0.1f);
            }
            else
            {
                Invoke("setUserKW", 0.5f);
            }
        }

        //������ Ű���带 �����ϴ� �Լ�
        public void setUserKW() //�۾���
        {
            //����Ʈ ������ ����
            for (int j = 0; j < 4; j++)
            {
                GameObject KWprefab = KWParents.transform.GetChild(j).gameObject;
                if (0 < KWprefab.transform.childCount)
                {
                    for (int n = 0; n < KWprefab.transform.childCount; n++)
                    {
                        GameObject.Destroy(KWprefab.transform.GetChild(n).gameObject);
                    }
                }
            }
            //Ű���� Ĩ������ ���� ��
            float sum0 = -32;
            float sum1 = -32;
            float sum2 = -32;
            //Ű���带 ���° ���̾ƿ��� ��ġ�Ұ��� �Ǻ��ϱ� ���� ����
            //bool layout0 = false;
            //bool layout1 = false;
            //bool layout2 = false;

            List<float> KW_Wid = new List<float>();

            Debug.Log("KWList �޾ƿ�");
            GameObject ListContent;

            for (int i = 0; i < FirebaseManager.KWList.Count; i++)
            {
                if ((FirebaseManager.KWList[i].ToString().Length == 1) || FirebaseManager.KWList[i].ToString() == "IT") { KW_Wid.Add(36 + 187 + 32); }
                else if (FirebaseManager.KWList[i].ToString().Length == 2) { KW_Wid.Add(36 + 246 + 32); }
                else if (FirebaseManager.KWList[i].ToString().Length == 3)
                {
                    if (FirebaseManager.KWList[i].ToString() == "SNS") { KW_Wid.Add(36 + 253 + 32); }
                    else { KW_Wid.Add(36 + 305 + 32); }
                }
                else if (FirebaseManager.KWList[i].ToString().Length == 4)
                {
                    if (FirebaseManager.KWList[i].ToString() == "MBTI") { KW_Wid.Add(36 + 283 + 32); }
                    else { KW_Wid.Add(36 + 364 + 32); }
                }
                else if (FirebaseManager.KWList[i].ToString().Length == 5) { KW_Wid.Add(36 + 423 + 32); }
                else { KW_Wid.Add(36 + 482 + 32); }



                //float wid = ListContent.transform.GetComponent<RectTransform>().rect.width;
                //float wid = KWContents0.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.x;

                //Debug.Log("Ĩ ��: " + wid);
                //for (int n = 0; n < KWContents0.transform.childCount; n++) //1��° �ٿ� �� �ڸ� �ִ��� ���
                //{
                //    sum = sum + wid;
                //    Debug.Log(wid);
                //    Debug.Log("��: " + sum);
                //    //sum = sum + KWContents0.transform.GetChild(n).gameObject.GetComponent<RectTransform>().rect.width + 32;

                //    Debug.Log("1��° ��: " + sum);
                //}
                //if (sum >= 1312) //1��° �ٿ� �ڸ� ������
                //{
                //    sum = -32; //�� �ʱ�ȭ
                //    for (int q = 0; q < KWContents1.transform.childCount; q++) //2��° �ٿ� �� �ڸ� �ִ��� ���
                //    {
                //        sum = sum + KWContents1.transform.GetChild(n).GetComponent<RectTransform>().rect.width + 32;
                //        Debug.Log("2��° ��: " + sum);
                //        if (sum >= 1312) //2��° �ٿ� �ڸ� ������
                //        {
                //            sum = -32; //�� �ʱ�ȭ
                //            for (int j = 0; j < KWContents2.transform.childCount; j++) //3��° �ٿ� �� �ڸ� �ִ��� ���
                //            {
                //                sum = sum + KWContents2.transform.GetChild(n).GetComponent<RectTransform>().rect.width + 32;
                //                Debug.Log("3��° ��: " + sum);
                //                if (sum >= 1312) //3��° �ٿ� �ڸ� ������
                //                {
                //                    ListContent.transform.SetParent(KWContents3.transform, false); //4��° �ٿ� ������Ʈ �־��ֱ�
                //                }
                //                else //3��° �ٿ� �ڸ� ������
                //                {
                //                    ListContent.transform.SetParent(KWContents2.transform, false); //3��° �ٿ� ������Ʈ �־��ֱ�
                //                }
                //            }

                //        }
                //        else //2��° �ٿ� �ڸ� ������
                //        {
                //            ListContent.transform.SetParent(KWContents1.transform, false); //2��° �ٿ� ������Ʈ �־��ֱ�
                //        }
                //    }

                //}
                //else //1��° �ٿ� �ڸ� ������
                //{
                //    ListContent.transform.SetParent(KWContents0.transform, false); //1��° �ٿ� ������Ʈ �־��ֱ�
                //}


            }
            for (int n = 0; n < KW_Wid.Count; n++)
            {
                ListContent = Instantiate(Resources.Load("Prefabs/KeywordPrefs/C_KW")) as GameObject;
                //Ű���� ����
                ListContent.transform.GetChild(0).GetComponent<Text>().text = "#" + FirebaseManager.KWList[n].ToString();

                if (sum0 + KW_Wid[n] <= 1312)
                {
                    sum0 = sum0 + KW_Wid[n];
                    ListContent.transform.SetParent(KWContents0.transform, false);
                    Debug.Log("0: " + sum0);
                }
                else
                {
                    if (sum1 + KW_Wid[n] <= 1312)
                    {
                        sum1 = sum1 + KW_Wid[n];
                        ListContent.transform.SetParent(KWContents1.transform, false);
                        Debug.Log("1: " + sum1);
                    }
                    else
                    {
                        if (sum2 + KW_Wid[n] <= 1312)
                        {
                            sum2 = sum2 + KW_Wid[n];
                            ListContent.transform.SetParent(KWContents2.transform, false);
                            Debug.Log("2: " + sum2);
                        }
                        else
                        {
                            ListContent.transform.SetParent(KWContents3.transform, false);
                        }
                    }
                }
            }
            //firebase_LoadKW();
            //if (FirebaseManager.KWList == null)
            //{
            //    Debug.Log("KWList ���޾ƿ�");
            //    Invoke("firebase_LoadKW", 0.1f);
            //}

            //Debug.Log("setUserKW �����");
            //dictionary�� Ű�� ���鼭 Ű�� ���� Ű���� ����Ʈ ���̸�ŭ ������Ʈ ����, �ش� ���� ����
            //foreach (string key in testdict.keys)
            //{
            //    for (int l = 0; l < testdict[key].count; l++)
            //    {
            //        gameobject listcontent = instantiate(resources.load("prefabs/mykeyword")) as gameobject;
            //        listcontent.transform.setparent(contentparents.transform, false);

            //        color color;
            //        colorutility.tryparsehtmlstring(key, out color);//""�ȿ� db���� �޾ƿ� ����ڵ� �־ rgb��ȯ �� ����
            //                                                        //Ű���� ī�װ� ����


            //        listcontent.transform.getchild(0).transform.getchild(0).transform.getchild(0).gameobject.getcomponent<image>().color = color;

            //        //Ű���� ����
            //        listcontent.transform.getchild(0).transform.getchild(0).transform.getchild(1).gameobject.getcomponent<text>().text = testdict[key][l].tostring();//"Ű���� ���� �׽�Ʈ";���⿡ db���� �޾ƿ� Ű���带 string���� ����

            //        //Ű���� ����
            //        listcontent.transform.getchild(1).gameobject.getcomponent<text>().text = "";
            //    }
            //}

        }






    }
}

