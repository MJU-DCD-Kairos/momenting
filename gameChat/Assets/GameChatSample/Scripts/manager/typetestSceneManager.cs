using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class typetestSceneManager : MonoBehaviour
{
    //스크립트 받아오기위한 타입 변수 선언
    gameSceneManager gSM;
    public Button backToProfile_normal;
    public Button backToProfile_result;

    // Start is called before the first frame update
    void Start()
    {
        //don't destroy로 살려서 넘어온 게임씬매니저의 스크립트를 변수에 담음
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

        //버튼에 gSM의 로드씬 함수 리스너를 추가함
        backToProfile_normal.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
        backToProfile_result.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
