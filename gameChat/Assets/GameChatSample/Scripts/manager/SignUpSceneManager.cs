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
public class SignUpSceneManager : MonoBehaviour
{
    gameSceneManager gSM;
    GoogleSignInDemo GSD;
    public Button GoToHome;
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

    

   
}
