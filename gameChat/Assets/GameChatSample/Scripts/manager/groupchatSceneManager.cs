using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameChatUnity;
using AS;

public class groupchatSceneManager : MonoBehaviour
{
    //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
    gameSceneManager gSM;
    public Button backToChatList;

    public Text ThisCRoomNameTitle;
    public RectTransform ContentRect;


    //�ҷ������� �޽����� ������ �޽����� ������ �Ǻ��ϱ� ���� �������� ����
    public Message LastMSG;

    //�� ��ǳ�� ��� ��ǳ�� ������ ���� ���� ����
    AreaScript LastArea;
    public GameObject MyArea, ElseArea;
    

    // Start is called before the first frame update
    void Start()
    {
        //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

        //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
        backToChatList.onClick.AddListener(gSM.LoadScene_ChatList);


        //�޽����� �ε��ϸ� �ʿ��� ����(ä�ù� �̸�, ��ũ�Ѻ� �θ� ��ü ã�ƿ�)
        ThisCRoomNameTitle.text = gameSceneManager.chatRname;

        StartCoroutine("TestMSG", gameSceneManager.chatRID);
    }
    void update()
    {


        // Update is called once per frame
        if (Application.platform == RuntimePlatform.Android)  // �÷��� ���� .
        {
            if (Input.GetKey(KeyCode.Escape)) // Ű ���� �ڵ� ��ȣ�� �޾ƿ��°�.
            {
                PlayerPrefs.SetString("LastMSGID", GameObject.Find("CRCode").name);
                SceneManager.LoadScene("ChatList"); // ������ �̵� .
                //Application.Quit(); // �� ���� .(������)            �������� �̵��̳� ������ �����ϳ� ���Ͻô°��� ����Ͻø� �˴ϴ�.
            }
        }
    }



    //���� �޽����� �������� �Լ�
    public IEnumerator TestMSG(string id)
    {
        //������ ä���� �޾ƿ�
        GameChat.getMessages(id, 0, 1, "", "", "", (List<Message> Messages, GameChatException Exception) =>
        {

            if (Exception != null)
            {
                // Error �ڵ鸵
                return;
            }


            foreach (Message elem in Messages)
            {
                LastMSG = elem;
                Debug.Log(LastMSG.ToString());
            }
        });


        GameChat.getMessages(id, 0, 200, "", "", "asc", (List<Message> Messages, GameChatException Exception) =>
        {

            if (Exception != null)
            {
                // Error �ڵ鸵
                return;
            }

            foreach (Message elem in Messages)
            {
                if (LastMSG.message_id != elem.message_id)
                {
                    Debug.LogError("@###@#@#@#@#@" + elem.content.ToString());
                    if (GameChatSample.SampleGlobalData.G_User.id == elem.sender.id)
                    {
                        AreaScript Area = Instantiate(MyArea).GetComponent<AreaScript>();
                        Area.transform.SetParent(ContentRect.transform, false);
                        Area.BoxRect.sizeDelta = new Vector2(1000, Area.BoxRect.sizeDelta.y);
                        Area.TextRect.GetComponent<Text>().text = elem.content;
                        Debug.Log(elem.content);
                        Area.TimeText.text = elem.created_at;

                    }
                    else
                    {
                        AreaScript Area2 = Instantiate(ElseArea).GetComponent<AreaScript>();
                        Area2.transform.SetParent(ContentRect.transform, false);
                        Area2.BoxRect.sizeDelta = new Vector2(1000, Area2.BoxRect.sizeDelta.y);
                        Area2.TextRect.GetComponent<Text>().text = elem.content;
                        Area2.UserText.text = elem.sender.name;
                        Area2.TimeText.text = elem.created_at;

                    }

                }
                else
                {
                    break;
                }
            }
        });
        yield return null;
    }


}
