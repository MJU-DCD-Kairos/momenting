using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameChatSample;

namespace Prefebs
{
    public class getGCID : MonoBehaviour
    {
        //ä�ù� �̸��� ������ �������� ����
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

        //��ư�� ������ �ش� ī���� ä�ù� �̸��� ��ȯ�ϴ� �Լ�
        public void onclickNameReturn()
        {

            thisRoomName = this.transform.GetChild(2).gameObject.GetComponent<Text>().text;
            Debug.Log("##########thisRoomName" + thisRoomName);
            clickCard = true;
        }


    }
}
