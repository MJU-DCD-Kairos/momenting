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

        //ä�ù� ���� id ���� ���� ("#All")�κ��� �����Ͽ� ���� Ű���� ������ �� ������ �̰ɷ� �ش� ä�ù濡 ���ٵ� ����
        creatChatCode = "chatchat";
        form.AddField("projectId", creatChatCode);


        //�� url ��û�� POST ��û
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        www.SetRequestHeader("x-api-key", APIKey);
        //www.SetRequestHeader("content-type", "application/json");

        //��û�� ���� ������ ��ٸ�
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("����");
            Debug.Log(www.downloadHandler.text);
            //DB�� ���� �����Ϳ� �ش� ä�ù� �ڵ�� �����ϴ� ���� �߰� �ʿ�
            

        }
    }


}
