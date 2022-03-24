using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;



[System.Serializable]

public class List
{
    public List(string _UserName, string _Recent, string _Chat, bool _Badge_MannerEvaluation, bool _Checkbox)
    { UserName = _UserName; Recent = _Recent; Chat = _Chat; Badge_MannerEvaluation = _Badge_MannerEvaluation; Checkbox = _Checkbox; }


    public Image ProfileImage;
    public string UserName, Recent, Chat;
    public bool Badge_MannerEvaluation, Checkbox;


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

    //���� ������ �� ����Ʈ �迭 ����
    public List<List> PersonalList;

    //���õ� ����Ʈ ���� ����Ʈ �迭 ����
    public List<OnCheckedList> CheckedList = new List<OnCheckedList>();

    [System.Serializable]
    public class OnCheckedList
    {
        public OnCheckedList(string _Index) { idx = _Index; }

        public string idx;
    }
    

    //����Ʈ�� ������ �θ� ������Ʈ�� ����
    public GameObject Content;
    //public GameObject CheckboxGroup;
    public string[] line;
    public string[] row;

    public int i;
    //public string idx;

    public GameObject[] ToggleList;
    void Start()
    {
        
        MyList();
        //AllSelect();
        //ToggleList();
        //FindChecked();
    }

    public void MyList()
    {
        //����Ʈ �ҷ�����
        line = UserDatabase.text.Substring(0, UserDatabase.text.Length - 1).Split('\n');
        for (i = 0; i < line.Length; i++)
        {
            row = line[i].Split('\t');

            PersonalList.Add(new List(row[0], row[1], row[2], row[3] == "TRUE", row[4] == "TRUE"));
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
            if (row[3] == "TRUE")
            {
                GameObject[] Badge = GameObject.FindGameObjectsWithTag("Information");
                Badge[i].transform.GetChild(0).gameObject.SetActive(true);
            }

            

        }

        Save();
        Load();

    }

    private void Update()
    {
        //Check();
    }

    /*
    public void ToggleList()
    {
        PersonalList.Badge_MannerEvaluation
    }
    */


    //��� ��ư�� On �Ǹ� ����Ʈ txt������ Checkbox ���� TRUE�� �ٲ�
    //row[4]���� TRUE ���� �ϳ��� ������ ���� ��ư Enabled Ȱ��ȭ
    //���� ��ư Enabled ������ Checkbox ���� TRUE�� line(�ε��� ��ȣ�� ã��?) ���� ã�Ƽ� ����Ʈ���� line ����.

    /*
    void FindChecked() //����Ʈ���� Checkbox ���� true�� �ε��� ã�� �Լ� 
    {
        int idx = PersonalList.FindIndex(x => x.UserName == "�����ſ��");
        Debug.Log(idx);
        
    }
    */


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

    /*
    void Check()
    {
        if (ToggleList[i].GetComponent<Toggle>().isOn == true)
        {
            string index = i.ToString();
            CheckedList.Add(new OnCheckedList(index));
        }

        else if (ToggleList[i].GetComponent<Toggle>().isOn == false)
        {
            string index = i.ToString();
            CheckedList.Remove(new OnCheckedList(index));
        }

    }
    */

    public Toggle allCheck;

    /*
    public void AllSelect()
    {
        //GameObject[] ToggleList = GameObject.FindGameObjectsWithTag("Toggle");
        //ToggleList[i].GetComponent<Toggle>().isOn = true;
        if (allCheck.isOn)
        {
            for (int n = 0; n < 35; n++)
            {
                GameObject[] ToggleList = GameObject.FindGameObjectsWithTag("Toggle");
                ToggleList[n].GetComponent<Toggle>().isOn = true;

            }
        }
        else
        {
            for (int n = 0; n < 35; n++)
            {
                GameObject[] ToggleList = GameObject.FindGameObjectsWithTag("Toggle");
                ToggleList[n].GetComponent<Toggle>().isOn = false;

            }
        }

    }
    */
}
