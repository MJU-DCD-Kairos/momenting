using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBinder : MonoBehaviour
{

    public ChatManager chatManager;
    public InputField messageInput;

    public void SendChat()
    {
        chatManager.Chat(true, messageInput.text, "³ª", null);
        messageInput.text = "";
    }

    public void ReChat()
    {
        //chatManager.Chat();
    }
}
