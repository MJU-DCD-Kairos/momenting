using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System.Text;

namespace SUCM
{
    public class SignUpSceneManager : MonoBehaviour
    {

        gameSceneManager gSM;
        GoogleSignInDemo GSD;
        public Button GoToHome;
        public GameObject KWpassBtn;
        public Text PreName;
        public Text PreAge;
        public Text PreSex;
        public Text PreIntro;
        public InputField Mname;
        public Dropdown Mage;
        public Dropdown MMon;
        public Dropdown Mday;
        public Dropdown Msex;
        public InputField Mintro;


        //public GameObject[] Interests;
        //public GameObject[] Lifestyle;


        //���� ���̵� �޾ƿ�
        public Text aa;

        //���õ� Ű���� ���� ����
        public Text IndicatorNum;

        public GameObject KWGroup;
        public GameObject KW0;
        public GameObject KW1;
        public GameObject KW2;
        public GameObject KW3;
        public GameObject canvas_Pre;

        // Start is called before the first frame update
        void Start()
        {
            //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
            gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();
            GSD = GameObject.Find("GoogleSIgnInManager").GetComponent<GoogleSignInDemo>();

            //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
            GoToHome.onClick.AddListener(GSD.save);
            GoToHome.onClick.AddListener(setstring);
            GoToHome.onClick.AddListener(gSM.LoadScene_Home);
        }
        public string age;
        public string newAge;
        private void Update()
        {
            getKWnum();
            setKWBtn();

            string age2 = Mage.options[Mage.value].text;
            string mon = MMon.options[MMon.value].text;
            string day = Mday.options[Mday.value].text;

            age = age2;
            //token = Token.text;

            //���� ���ϱ�
            newAge = DateTime.Now.ToString("yyyy");
            Debug.Log(DateTime.Now.ToString("yyyy"));

            newAge = DateTime.Now.ToString("yyyy");
            Debug.Log(DateTime.Now.ToString("yyyy"));

            newAge = (int.Parse(newAge.ToString()) - int.Parse(age2)).ToString();
            Debug.Log("newAge1 :" + newAge);
            if (int.Parse(DateTime.Now.ToString("MM")) == int.Parse(mon))
            {
                if (int.Parse(DateTime.Now.ToString("dd")) < int.Parse(day))
                    age = newAge = (int.Parse(newAge) - 1).ToString();

            }
            else if (int.Parse(DateTime.Now.ToString("MM")) < int.Parse(mon))
            {
                age = newAge = (int.Parse(newAge) - 1).ToString();
            }
            else
            {
                age = newAge;
            }

            PreAge.text = newAge;
            PreName.text = Mname.text;
            PreSex.text = Msex.options[Msex.value].text;
            
            if(Mintro.text != null) 
            {
                PreIntro.text = "�� �� �Ұ��� �ۼ����� �ʾҾ��.";
            }
            else
            {
                PreIntro.text = Mintro.text;
            }
            
        }

        public void setstring()
        {
            aa.text = PlayerPrefs.GetString("GoogleID");
        }

        //������ Ű������ ���ڸ� �޾ƿ� UI�� ����ִ� �ڵ�
        public void getKWnum()
        {
            IndicatorNum.text = getKeywordList.KWcheckCount + "/" + "8";
        }

        public void setKWBtn()
        {
            if (getKeywordList.KWcheckCount >= 1)
            {
                // ������ Ű���� ������ 1�� �̻��̸� ��ӹ�ư Ȱ��ȭ
                KWpassBtn.SetActive(true);
            }
            else
            {
                // ������ Ű���� ������ 1�� �̸��̸� ��ӹ�ư ��Ȱ��ȭ
                KWpassBtn.SetActive(false);
            }
                
        }
        
        public void Pre_setKW()
        {
            //����Ʈ ������ ����
            for (int j = 0; j < 4; j++)
            {
                GameObject KWprefab = KWGroup.transform.GetChild(j).gameObject;
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

            GameObject ListContent;

            if (getKeywordList.saveKWlist.Count == 0) //Ű���尡 ������ UI
            {
                KWGroup.transform.GetChild(0).gameObject.SetActive(true);
                KW0.SetActive(false);
                KW1.SetActive(false);
                KW2.SetActive(false);
                KW3.SetActive(false);

                //canvas_ED.SetActive(false);
                //canvas_ED.SetActive(true);

                KWGroup.SetActive(false);
                KWGroup.SetActive(true);
            }
            else
            {
                KWGroup.transform.GetChild(0).gameObject.SetActive(false);
                KW0.SetActive(true);
                KW1.SetActive(true);
                KW2.SetActive(true);
                KW3.SetActive(true);

                for (int i = 0; i < getKeywordList.saveKWlist.Count; i++)
                {
                    if ((getKeywordList.saveKWlist[i].ToString().Length == 1) || getKeywordList.saveKWlist[i].ToString() == "IT") { KW_Wid.Add(36 + 187 + 32); }
                    else if (getKeywordList.saveKWlist[i].ToString().Length == 2) { KW_Wid.Add(36 + 246 + 32); }
                    else if (getKeywordList.saveKWlist[i].ToString().Length == 3)
                    {
                        if (getKeywordList.saveKWlist[i].ToString() == "SNS") { KW_Wid.Add(36 + 253 + 32); }
                        else { KW_Wid.Add(36 + 305 + 32); }
                    }
                    else if (getKeywordList.saveKWlist[i].ToString().Length == 4)
                    {
                        if (getKeywordList.saveKWlist[i].ToString() == "MBTI") { KW_Wid.Add(36 + 283 + 32); }
                        else { KW_Wid.Add(36 + 364 + 32); }
                    }
                    else if (getKeywordList.saveKWlist[i].ToString().Length == 5) { KW_Wid.Add(36 + 423 + 32); }
                    else { KW_Wid.Add(36 + 482 + 32); }
                }

                for (int n = 0; n < KW_Wid.Count; n++)
                {
                    ListContent = Instantiate(Resources.Load("Prefabs/KeywordPrefs/C_KW")) as GameObject;
                    //Ű���� ����
                    ListContent.transform.GetChild(0).GetComponent<Text>().text = "#" + getKeywordList.saveKWlist[n].ToString();

                    if (sum0 + KW_Wid[n] <= 1312)
                    {
                        sum0 = sum0 + KW_Wid[n];
                        ListContent.transform.SetParent(KW0.transform, false);
                    }
                    else if (sum1 + KW_Wid[n] <= 1312)
                    {
                        sum1 = sum1 + KW_Wid[n];
                        ListContent.transform.SetParent(KW1.transform, false);
                    }
                    else if (sum2 + KW_Wid[n] <= 1312)
                    {
                        sum2 = sum2 + KW_Wid[n];
                        ListContent.transform.SetParent(KW2.transform, false);
                    }
                    else
                    {
                        ListContent.transform.SetParent(KW3.transform, false);
                    }

                }
                canvas_Pre.SetActive(false);
                canvas_Pre.SetActive(true);

                KWGroup.SetActive(false);
                KWGroup.SetActive(true);
                //KW_ToggleOn();

                Debug.LogError("Pre_setKW ����");
            }
        }

        public void Load_TypeTest()
        {
            SceneManager.LoadScene("TypeTest");
        }
    }
}