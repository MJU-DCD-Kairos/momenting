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
    public void writeTXT() //����Ʈ �����
    {
        writer = new StreamWriter(Application.dataPath + "/GameChatSample/Resources/text/ChatInfo.txt", true); //���� �̸��� ���� ���Ϸ� ���
        writer.WriteLine("title","sender");
        writer.Flush();
        writer.Close();

    }


}