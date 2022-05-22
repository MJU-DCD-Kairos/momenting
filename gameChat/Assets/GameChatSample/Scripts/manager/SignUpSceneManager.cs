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
        public static string[] Tendency = new string[5];
        public static string[] Interests = new string[5];
        public static string[] Lifestyle = new string[5];
        public static int sum;
        //public GameObject[] Interests;
        //public GameObject[] Lifestyle;



        public Text aa;
        // Start is called before the first frame update
        void Start()
        {
            //don't destroy로 살려서 넘어온 게임씬매니저의 스크립트를 변수에 담음
            gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();
            GSD = GameObject.Find("GoogleSIgnInManager").GetComponent<GoogleSignInDemo>();


            //버튼에 gSM의 로드씬 함수 리스너를 추가함


            GoToHome.onClick.AddListener(GSD.save);
            GoToHome.onClick.AddListener(setstring);
            GoToHome.onClick.AddListener(gSM.LoadScene_Home);
        }

        public void setstring()
        {
            aa.text = PlayerPrefs.GetString("GoogleID");
        }
        public int a = 0;
        public void AddKeytoList()
        {

            if (a < 5)
            {
                Tendency[a] = GameObject.Find("Btn_Keyword").GetComponent<KeywordHive>().TenText.text;


                Debug.Log(GameObject.Find("Btn_Keyword").GetComponent<KeywordHive>().TenText.text);

                a += 1;

            }
            else
            {
                Debug.Log("키워드 선택 개수를 초과하였음");
            }
        }
        public void AddKeytoList2()
        {
            int a = 0;
            if (a < 5)
            {

                Interests[a] = GetComponent<KeywordHive2>().IntText.text;



                Debug.Log(GetComponent<KeywordHive2>().IntText.text);

                a += 1;

            }
            else
            {
                Debug.Log("키워드 선택 개수를 초과하였음");
            }
        }
        public void AddKeytoList3()
        {
            int a = 0;
            if (a < 5)
            {

                Lifestyle[a] = GetComponent<KeywordHive3>().LifeText.text;


                Debug.Log(GetComponent<KeywordHive3>().LifeText.text);
                a += 1;

            }
            else
            {
                Debug.Log("키워드 선택 개수를 초과하였음");
            }
        }

        public void CheckTenList(bool ischecked)
        {
            if (ischecked == true)
            {

            }
        }



    }
}