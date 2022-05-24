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

        private void Update()
        {
            getKWnum();
            setKWBtn();
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