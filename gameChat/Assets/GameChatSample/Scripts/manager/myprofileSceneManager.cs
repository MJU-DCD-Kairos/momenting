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
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

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
        public GameObject KWarea;
        public GameObject canvas_Pr;

        //public Button Save_KW;
        public List<string> profileKW = new List<string>();

        //�������� �ʿ��� ����
        public Text txtIntro_edit;
        public Text txtPlaceholder;
        public static string EDIT_INTRO;
        public InputField Intro_Input;

        public Text IndicatorNum;


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
            KW_ToggleOn();
            setUserKW();
            //Save_KW.onClick.AddListener(KWedit);

            //Edit_Load_Intro();  //������������ ���ټҰ� �ε�
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

            getKWnum();
        }

        //���� �� ������ ��ε�
        public void LoadProfile() 
        {
            Debug.Log("����� ���ټҰ�: " + txtIntro_edit.text);
            txtIntro.text = txtIntro_edit.text;
        }
        

        //���������� �ε� �� ���� �ҷ�����
        public void Edit_Load_Intro()
        {
            txtPlaceholder.text = txtIntro.text;
            txtIntro_edit.text = txtIntro.text;
            Intro_Input.text = txtIntro.text;
        }

        public async void Edit_Save_Intro()
        {
            DocumentReference userRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN);
            
            await userRef.UpdateAsync("Introduction", txtIntro_edit.text);
            Debug.Log(txtIntro_edit.text + "����Ϸ�!");
            //EDIT_INTRO = txtIntro_edit.text;
            //Debug.Log("edit �� == " + EDIT_INTRO);
        }

        public async Task Edit_SaveKW()
        {
            var awaiter = delete_duplicatedKW().GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                //int kw = 0;
                FirebaseManager.KWList.Clear();
                for (int i = 0; i < getKeywordList.saveKWlist.Count; i++)
                {
                    FirebaseManager.KWList.Add(getKeywordList.saveKWlist[i]);
                    Debug.Log("SAVE ���� ������ Ű����: " + FirebaseManager.KWList[i]);
                    
                }
                //return kw;
                FirebaseManager.db.Collection("userInfo").Document(GCN).UpdateAsync("Keyword", FirebaseManager.KWList);
                Debug.LogError("db�� Ű���� ���� �Ϸ�!");
            });

        }

        public async Task delete_duplicatedKW()
        {
            await FirebaseManager.db.Collection("userInfo").Document(GCN).UpdateAsync("Keyword", FieldValue.Delete);
            Debug.LogError("DBŰ���� ���� �Ϸ�!");
        }

        public async void KWedit()
        {
            Debug.LogError("KWedit ����");
            var awaiter = Edit_SaveKW().GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                //setUserKW();
                Invoke("setUserKW", 0.5f);
                
            });
        }

        public void KW_ToggleOn()
        {
            //���ɻ�
            for (int k = 1; k < 6; k++)
            { 
                GameObject Group = KWarea.transform.GetChild(k).gameObject;
                
                for (int i = 0; i < Group.transform.childCount; i++)
                {
                    string Text = Group.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text;
                    Group.transform.GetChild(i).GetComponent<Toggle>().isOn = false;

                    for (int w = 0; w < FirebaseManager.KWList.Count; w++)
                    {
                        if (Text == FirebaseManager.KWList[w].ToString())
                        {
                            Group.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
                        }
                    }
                }
            }
            //����
            for (int k = 7; k < 14; k++)
            {
                GameObject Group = KWarea.transform.GetChild(k).gameObject;
                
                for (int i = 0; i < Group.transform.childCount; i++)
                {
                    string Text = Group.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text;
                    Group.transform.GetChild(i).GetComponent<Toggle>().isOn = false;

                    for (int w = 0; w < FirebaseManager.KWList.Count; w++)
                    {
                        if (Text == FirebaseManager.KWList[w].ToString())
                        {
                            Group.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
                        }
                    }
                }
            }
            Debug.LogError("KW_ToggleOn ����");
        }
        
        //Ű���� Ĩ�� �����ϴ� �Լ�
        async Task setUserKW()
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
            float sum0 = -32;
            float sum1 = -32;
            float sum2 = -32;

            List<float> KW_Wid = new List<float>();

            //Debug.Log("KWList �޾ƿ�");
            GameObject ListContent;

            if (FirebaseManager.KWList.Count == 0) //Ű���尡 ������ UI
            {
                KWParents.transform.GetChild(0).gameObject.SetActive(true);
                KWContents0.SetActive(false);
                KWContents1.SetActive(false);
                KWContents2.SetActive(false);
                KWContents3.SetActive(false);

                //canvas_ED.SetActive(false);
                //canvas_ED.SetActive(true);

                KWParents.SetActive(false);
                KWParents.SetActive(true);
            }
            else
            {
                KWParents.transform.GetChild(0).gameObject.SetActive(false);
                KWContents0.SetActive(true);
                KWContents1.SetActive(true);
                KWContents2.SetActive(true);
                KWContents3.SetActive(true);

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
                    }
                    else if (sum1 + KW_Wid[n] <= 1312)
                    {
                        sum1 = sum1 + KW_Wid[n];
                        ListContent.transform.SetParent(KWContents1.transform, false);
                    }
                    else if (sum2 + KW_Wid[n] <= 1312)
                    {
                        sum2 = sum2 + KW_Wid[n];
                        ListContent.transform.SetParent(KWContents2.transform, false);
                    }
                    else
                    {
                        ListContent.transform.SetParent(KWContents3.transform, false);
                    }

                }
                canvas_Pr.SetActive(false);
                canvas_Pr.SetActive(true);

                KWParents.SetActive(false);
                KWParents.SetActive(true);
                KW_ToggleOn();

                Debug.LogError("setUserKW ����");
            }
            
        }

        //������ Ű������ ���ڸ� �޾ƿ� UI�� ����ִ� �ڵ�
        public void getKWnum()
        {
            IndicatorNum.text = getKeywordList.KWcheckCount.ToString();
        }
    }
}

