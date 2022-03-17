using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;


[System.Serializable]

public class List
{
    public List(string _UserName, string _Recent, string _Chat, string _Badge_MannerEvaluation, bool _Checkbox)
    { UserName = _UserName; Recent = _Recent; Chat = _Chat; Badge_MannerEvaluation = _Badge_MannerEvaluation; Checkbox = _Checkbox; }


    public Image ProfileImage;
    public string UserName, Recent, Chat, Badge_MannerEvaluation;
    public bool Checkbox;
}

/*
[System.Serializable]
public class ToggleList
{
    public ToggleList(bool _Checkbox) { Checkbox = _Checkbox; }

    public bool Checkbox;
}
*/

public class PersonalListEdit : MonoBehaviour
{
    //txt������ �ν����Ϳ��� �����ϵ��� ����
    public TextAsset UserDatabase;

    //����Ʈ �迭 ����
    public List<List> PersonalList;
    //public List<ToggleList> CheckedList;

    //����Ʈ�� ������ �θ� ������Ʈ�� ����
    public GameObject Content;
    public string[] line;
    public string[] row;

    void Start()
    {
        MyList();
        //ToggleList();
        FindChecked();

    }

    public void MyList()
    {
        //����Ʈ �ҷ�����
        line = UserDatabase.text.Substring(0, UserDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            row = line[i].Split('\t');

            PersonalList.Add(new List(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
            //CheckedList.Add(new ToggleList (row[4] == "TRUE"));

            GameObject Canvas_ChatList = GameObject.Find("ChatList").transform.Find("ChatList_Main").gameObject;
            GameObject Canvas_Personal_SeeMore = GameObject.Find("ChatList").transform.Find("Personal_SeeMore").gameObject;
            GameObject Canvas_Personal_Edit = GameObject.Find("ChatList").transform.Find("Personal_Edit").gameObject;

            //����Ʈ �������� Content �θ� �ؿ� �ڽ����� ����
            if (Canvas_Personal_Edit.activeInHierarchy == false)
            {
                GameObject ListContent = Instantiate(Resources.Load("Prefabs/List_PersonalChat")) as GameObject;
                ListContent.transform.SetParent(Content.transform, false);

            }
            
            else if (Canvas_Personal_SeeMore.activeInHierarchy || Canvas_ChatList.activeInHierarchy == false)
            {
                GameObject ListContent = Instantiate(Resources.Load("Prefabs/List_Personal_Edit")) as GameObject;
                ListContent.transform.SetParent(Content.transform, false);

            }
            
            //�г����� UserName ������Ʈ�� �ؽ�Ʈ ������Ʈ�� �Ҵ�
            GameObject[] UserNameList = GameObject.FindGameObjectsWithTag("UserName");
            UserNameList[i].GetComponent<Text>().text = row[0];

            //�޽��� �ð��� Recent ������Ʈ�� �ؽ�Ʈ ������Ʈ�� �Ҵ�
            GameObject[] RecentList = GameObject.FindGameObjectsWithTag("Recent");
            RecentList[i].GetComponent<Text>().text = row[1];

            //�޽����̸����⸦ ChatPreview ������Ʈ�� �ؽ�Ʈ ������Ʈ�� �Ҵ�
            GameObject[] ChatPreviewList = GameObject.FindGameObjectsWithTag("ChatPreview");
            ChatPreviewList[i].GetComponent<Text>().text = row[2];

            //���� 1�� ��쿡�� �ų��� ���� ����
            //0�� 1�� ���� (0 = 24�ð� ������ �ʾҰų� �ų��򰡸� �� ��� / 1 = 24�ð� ������, �ų��� ���� ���� ���)
            if (row[3] == "1")
            {
                GameObject[] Badge = GameObject.FindGameObjectsWithTag("Information");
                Badge[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            


        }

        Save();
        Load();

    }

    /*
    public void ToggleList()
    {
        line = UserDatabase.text.Substring(0, UserDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            row = line[i].Split('\t');

            CheckedList.Add(new List(row[4] == "TRUE"));

        }

        Debug.Log(CheckedList);
    }
    */

    //��� ��ư�� On �Ǹ� ����Ʈ txt������ Checkbox ���� TRUE�� �ٲ�
    //row[4]���� TRUE ���� �ϳ��� ������ ���� ��ư Enabled Ȱ��ȭ
    //���� ��ư Enabled ������ Checkbox ���� TRUE�� line(�ε��� ��ȣ�� ã��?) ���� ã�Ƽ� ����Ʈ���� line ����.

    
    void FindChecked() //����Ʈ���� Checkbox ���� true�� �ε��� ã�� �Լ� 
    {
        int idx = PersonalList.FindIndex(x => x.UserName == "�����ſ��");
        Debug.Log(idx);
        
    }
    


    void Save() //json ���Ϸ� ����
    {
        string jdata = JsonConvert.SerializeObject(PersonalList);
        File.WriteAllText(Application.dataPath + "/GameChatSample/Resources/MyPersonalList.txt", jdata);

        //OnToggle(Check);
    }

    void Load() //json ���� ��������
    {
        string jdata = File.ReadAllText(Application.dataPath + "/GameChatSample/Resources/MyPersonalList.txt");
        PersonalList = JsonConvert.DeserializeObject<List<List>>(jdata);

        //OnToggle(Check);
    }





    public void OnToggle(bool isOn)
    {

        //Check = isOn;
        //List CheckedList = Checkbox.Find(x => x.Checkbox == true);
        //CheckedList = PersonalList.FindAll(x => x == "TRUE");
        //GameObject Enabled = GameObject.Find("Btn_Edit_Enabled");
        //GameObject Disabled = GameObject.Find("Btn_Edit_Disabled");
        
        if (isOn)
        {
            FindChecked();
            //CheckedList.Checkbox = true;
            //Disabled.SetActive(false);
            //Enabled.SetActive(true);
        }

        else
        {
            //Disabled.SetActive(true);
            //Enabled.SetActive(false);
        }

        Save();
        
    }


    /*
    public void OnClickDestroy()
    {
        GameObject ForDestroy = GameObject.Find("List_Personal_Edit(Clone)");
        Destroy(ForDestroy);
    }
    */




}
