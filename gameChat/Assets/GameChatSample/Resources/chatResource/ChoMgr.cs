using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using groupchatManager;

public enum People
{
    noone,notwo,nothree,nofour,nofive
        //보이는 순서에 따라 번호 부여
}
public class ChoMgr : MonoBehaviour
{
    public static ChoMgr instance;

    public Text name0;
    public Text name1;
    public Text name2;
    public Text name3;
    public Text name4;

    private void Awake()
    {
        if (instance == null) instance = this; 
        else if (instance != null) return;
        DontDestroyOnLoad(gameObject);
        //씬 전환이 돼도 사라지지 않도록
        Debug.Log(currentChoice + "ChoMgr");


    }
    public People currentChoice;

    public void setNameText()
    {
        for (int i = 0; i < groupchatSceneManager.chatRoom.Count; i++)
        {
            if (i == 0) { name0.text = groupchatSceneManager.chatRoom[i]; }
            else if (i == 1) { name1.text = groupchatSceneManager.chatRoom[i]; }
            else if (i == 2) { name2.text = groupchatSceneManager.chatRoom[i]; }
            else if (i == 3) { name3.text = groupchatSceneManager.chatRoom[i]; }
            else if (i == 4) { name4.text = groupchatSceneManager.chatRoom[i]; }
            Debug.Log("chomgr" + groupchatSceneManager.chatRoom[i]);
        }
    }
}
