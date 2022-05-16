using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class CreatChat : MonoBehaviour
{
    csvReaderChatRoomName crn;
    public string creatChatCode;

    // Start is called before the first frame update
    void Start()
    {
        crn = GameObject.Find("CreatMatcing").GetComponent<csvReaderChatRoomName>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallNewChatRoom()
    { 
        StartCoroutine("CreateChatRoom");
    }

    IEnumerator CreateChatRoom()
    {
        string url = "https://dashboard-api.gamechat.naverncp.com/v1/api/project/e3558324-2d64-47d0-bd7a-6fa362824bd7/channel";
        string APIKey = "ec31cc21b559da9eb19eaec2dadcd50ed786857a740a561d";

        WWWForm form = new WWWForm();
        name = crn.makeChatRoomName();
        form.AddField("name", name);

        //채팅방 고유 id 지정 가능 ("#All")부분을 변경하여 고유 키값을 생성할 수 있으며 이걸로 해당 채팅방에 접근도 가능
        creatChatCode = "chatchat";
        form.AddField("projectId", creatChatCode);


        //웹 url 요청함 POST 요청
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        www.SetRequestHeader("x-api-key", APIKey);
        //www.SetRequestHeader("content-type", "application/json");

        //요청에 대한 응답을 기다림
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("성공");
            Debug.Log(www.downloadHandler.text);
            //DB의 유저 데이터에 해당 채팅방 코드로 접속하는 로직 추가 필요
            

        }
    }


}
