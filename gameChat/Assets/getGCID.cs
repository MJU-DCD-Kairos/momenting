using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameChatSample;

namespace Prefebs
{
    public class getGCID : MonoBehaviour
    {
        //채팅방 이름을 저장할 전역변수 선언
        public static string thisRoomName = "";
        public static bool clickCard;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //버튼을 누르면 해당 카드의 채팅방 이름을 반환하는 함수
        public void onclickNameReturn()
        {

            thisRoomName = this.transform.GetChild(2).gameObject.GetComponent<Text>().text;
            Debug.Log("##########thisRoomName" + thisRoomName);
            clickCard = true;
        }


    }
}
