using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class DemoTxtFile : MonoBehaviour
{
    public Text sender;
    public Text title;

    StreamWriter writer;
    public void start()
    {
        writeTXT();
    }
    public void writeTXT() //리스트 덮어쓰기
    {
        writer = new StreamWriter(Application.dataPath + "/GameChatSample/Resources/text/ChatInfo.txt", true); //같은 이름을 가진 파일로 덮어씀
        writer.WriteLine("title","sender");
        writer.Flush();
        writer.Close();

    }


}