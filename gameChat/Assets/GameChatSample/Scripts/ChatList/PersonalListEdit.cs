using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;



public class PersonalListEdit : MonoBehaviour
{

    [System.Serializable]

    public class List //�� ���� ������ Ŭ���� ����
    {
        public List(string _UserName, string _Recent, string _Chat, bool _Badge_MannerEvaluation, Toggle _Toggle)
        { UserName = _UserName; Recent = _Recent; Chat = _Chat; Badge_MannerEvaluation = _Badge_MannerEvaluation; toggle = _Toggle; }


        public Image ProfileImage;
        public string UserName, Recent, Chat;
        public Toggle toggle;

        public bool Badge_MannerEvaluation;


    }

    //txt������ �ν����Ϳ��� �����ϵ��� ����
    public TextAsset UserDatabase;

    //���� ������ �� ����Ʈ �迭 ����
    public List<List> PersonalList;

    public Toggle checkbox;


    //����Ʈ�� ������ �θ� ������Ʈ�� ����
    public GameObject Content;

    public int i;
    public GameObject ListContent;

    void Start()
    {

        MyList();
    }
    private void Update()
    {
        //OnChecked();
    }

    public void MyList()
    {
        //����Ʈ �ҷ�����
        string[] line = UserDatabase.text.Substring(0, UserDatabase.text.Length - 1).Split('\n');
        for (i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

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
                checkbox = ListContent.GetComponentInChildren<Toggle>();
            }

            PersonalList.Add(new List(row[0], row[1], row[2], row[3] == "TRUE", checkbox));

            //�г����� UserName ������Ʈ�� �ؽ�Ʈ ������Ʈ�� �Ҵ�
            GameObject[] UserNameList = GameObject.FindGameObjectsWithTag("UserName");
            UserNameList[i].GetComponent<Text>().text = row[0];

            //�޽��� �ð��� Recent ������Ʈ�� �ؽ�Ʈ ������Ʈ�� �Ҵ�
            GameObject[] RecentList = GameObject.FindGameObjectsWithTag("Recent");
            RecentList[i].GetComponent<Text>().text = row[1];

            //�޽����̸����⸦ ChatPreview ������Ʈ�� �ؽ�Ʈ ������Ʈ�� �Ҵ�
            GameObject[] ChatPreviewList = GameObject.FindGameObjectsWithTag("ChatPreview");
            ChatPreviewList[i].GetComponent<Text>().text = row[2];

            //���� true�� ��쿡�� �ų��� ���� ���� (false = 24�ð� ������ �ʾҰų� �ų��򰡸� �� ��� / true = 24�ð� ������, �ų��� ���� ���� ���)
            if (row[3] == "TRUE")
            {
                GameObject[] Badge = GameObject.FindGameObjectsWithTag("Information");
                Badge[i].transform.GetChild(0).gameObject.SetActive(true);
            }

            //GameObject[] ToggleList = GameObject.FindGameObjectsWithTag("Toggle");
            //checkbox = ToggleList[i].GetComponent<Toggle>();



        }
        //Save();
        //Load();

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
        //PersonalList = JsonConvert.DeserializeObject<List<List>>(jdata);

        //OnToggle(Check);
    }
    //public List<string> currentSelected;
    public GameObject DisabledBtn;

    public void CheckSelected()
    {
        for (int i = PersonalList.Count - 1; i >= 0; i--)
        {
            if (checkbox.isOn)
            {
                DisabledBtn.SetActive(false);
                
                //PersonalList.Remove(PersonalList[i]);
            }
            //PersonalList.Remove(PersonalList.FindAll(x => x.toggle.isOn));
           
        }
    }
    
    public void DestroyList()
    {
        for (int i = PersonalList.Count - 1; i >= 0; i--)
        {
            if (checkbox.isOn)
            {
                Debug.Log(i);
                //PersonalList.Remove(PersonalList[i]);
            }
            //PersonalList.Remove(PersonalList.FindAll(x => x.toggle.isOn));

        }
    }

    //List<List> currentChecked = PersonalList.FindAll(x => x.toggle.isOn = true);
    
}
