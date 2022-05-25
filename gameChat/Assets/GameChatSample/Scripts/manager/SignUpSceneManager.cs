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
using KH;
using KH2;
using KH3;


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

            PreAge.text = Mage.options[Mage.value].text;
            PreName.text = Mname.text;
            PreSex.text = age;
            PreIntro.text = Mintro.text;


        }

        public void setstring()
        {
            aa.text = PlayerPrefs.GetString("GoogleID");
        }

        //������ Ű������ ���ڸ� �޾ƿ� UI�� ����ִ� �ڵ�
        public void getKWnum()
        {
            IndicatorNum.text = getKeywordList.KWcheckCount + "/" + "5";
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
        
    }
}