using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;



public class PersonalListEdit : MonoBehaviour
{

    [System.Serializable]

    public class List //각 값을 보유할 클래스 생성
    {
        public List(string _UserName, string _Recent, string _Chat, bool _Badge_MannerEvaluation, Toggle _Toggle)
        { UserName = _UserName; Recent = _Recent; Chat = _Chat; Badge_MannerEvaluation = _Badge_MannerEvaluation; toggle = _Toggle; }


        public Image ProfileImage;
        public string UserName, Recent, Chat;
        public Toggle toggle;

        public bool Badge_MannerEvaluation;


    }

    //txt파일을 인스펙터에서 참조하도록 생성
    public TextAsset UserDatabase;

    //유저 정보가 들어갈 리스트 배열 생성
    public List<List> PersonalList;

    public Toggle checkbox;


    //리스트를 생성할 부모 오브젝트를 참조
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
        //리스트 불러오기
        string[] line = UserDatabase.text.Substring(0, UserDatabase.text.Length - 1).Split('\n');
        for (i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            GameObject Canvas_ChatList = GameObject.Find("ChatList").transform.Find("ChatList_Main").gameObject;
            GameObject Canvas_Personal_SeeMore = GameObject.Find("ChatList").transform.Find("Personal_SeeMore").gameObject;
            GameObject Canvas_Personal_Edit = GameObject.Find("ChatList").transform.Find("Personal_Edit").gameObject;


            //리스트 프리팹을 Content 부모 밑에 자식으로 생성
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

            //닉네임을 UserName 오브젝트의 텍스트 컴포넌트에 할당
            GameObject[] UserNameList = GameObject.FindGameObjectsWithTag("UserName");
            UserNameList[i].GetComponent<Text>().text = row[0];

            //메시지 시간을 Recent 오브젝트의 텍스트 컴포넌트에 할당
            GameObject[] RecentList = GameObject.FindGameObjectsWithTag("Recent");
            RecentList[i].GetComponent<Text>().text = row[1];

            //메시지미리보기를 ChatPreview 오브젝트의 텍스트 컴포넌트에 할당
            GameObject[] ChatPreviewList = GameObject.FindGameObjectsWithTag("ChatPreview");
            ChatPreviewList[i].GetComponent<Text>().text = row[2];

            //값이 true인 경우에만 매너평가 배지 생성 (false = 24시간 지나지 않았거나 매너평가를 한 경우 / true = 24시간 지났고, 매너평가 하지 않은 경우)
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


    void Save() //json 파일로 저장
    {
        string jdata = JsonConvert.SerializeObject(PersonalList);
        File.WriteAllText(Application.dataPath + "/GameChatSample/Resources/MyPersonalList.txt", jdata);

        //OnToggle(Check);
    }

    void Load() //json 파일 가져오기
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
