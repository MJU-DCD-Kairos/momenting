using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class personalchatSceneManager : MonoBehaviour
{
    //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
    gameSceneManager gSM;
    public Button backToChatList;

    // Start is called before the first frame update
    void Start()
    {
        //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

        //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
        backToChatList.onClick.AddListener(gSM.LoadScene_ChatList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
