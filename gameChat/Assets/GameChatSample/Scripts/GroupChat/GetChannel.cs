using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameChatUnity;

public class GetChannel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameChat.initialize("e3558324-2d64-47d0-bd7a-6fa362824bd7");
        //GameChat.sendMessage("d4aa3dfa-3b23-4a84-b4be-7ea4f19eed73", "재밌는 망고테스트");


        //GetChannelFunction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*
    void ServerConnect()
    {
        GameChat.connect(USER_ID, (Member User, GameChatException Exception) =>
        {

            if (Exception != null)
            {
                // Error 핸들링
                return;
            }
        });
    }
    */

    string GetChannelFunction()
    {
        string channelID = "c01f280f-428d-4dc7-a4d1-81b9a2667e7d";

        GameChat.getChannel(channelID, null, (Channel Channel, GameChatException Exception) =>
        {
            if (Exception != null)
            {
                // Error 핸들링
                Debug.Log("에러");
                return;
            }
            Debug.Log("get채널 사용 완료");
            //handling channelInfo instance
        });
        return "";
    }
}
