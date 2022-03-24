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
    //txt파일을 인스펙터에서 참조하도록 생성
    public TextAsset UserDatabase;

    //유저 정보가 들어갈 리스트 배열 생성
    public List<List> PersonalList;

    //선택된 리스트 담을 리스트 배열 생성
    public List<OnCheckedList> CheckedList = new List<OnCheckedList>();

    [System.Serializable]
    public class OnCheckedList
    {
        public OnCheckedList(string _Index) { idx = _Index; }

        public string idx;
    }
    

    //리스트를 생성할 부모 오브젝트를 참조
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
        //리스트 불러오기
        line = UserDatabase.text.Substring(0, UserDatabase.text.Length - 1).Split('\n');
        for (i = 0; i < line.Length; i++)
        {
            row = line[i].Split('\t');

            PersonalList.Add(new List(row[0], row[1], row[2], row[3] == "TRUE", row[4] == "TRUE"));
            //CheckedList.Add(new ToggleList (row[4] == "TRUE"));

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

            }
            
            //닉네임을 UserName 오브젝트의 텍스트 컴포넌트에 할당
            GameObject[] UserNameList = GameObject.FindGameObjectsWithTag("UserName");
            UserNameList[i].GetComponent<Text>().text = row[0];

            //메시지 시간을 Recent 오브젝트의 텍스트 컴포넌트에 할당
            GameObject[] RecentList = GameObject.FindGameObjectsWithTag("Recent");
            RecentList[i].GetComponent<Text>().text = row[1];

            //메시지미리보기를 ChatPreview 오브젝트의 텍스트 컴포넌트에 할당
            GameObject[] ChatPreviewList = GameObject.FindGameObjectsWithTag("ChatPreview");
            ChatPreviewList[i].GetComponent<Text>().text = row[2];

            //값이 1인 경우에만 매너평가 배지 생성
            //0과 1로 구분 (0 = 24시간 지나지 않았거나 매너평가를 한 경우 / 1 = 24시간 지났고, 매너평가 하지 않은 경우)
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


    //토글 버튼이 On 되면 리스트 txt파일의 Checkbox 값이 TRUE로 바뀜
    //row[4]에서 TRUE 값이 하나라도 있으면 삭제 버튼 Enabled 활성화
    //삭제 버튼 Enabled 누르면 Checkbox 값이 TRUE인 line(인덱스 번호로 찾기?) 전부 찾아서 리스트에서 line 삭제.

    /*
    void FindChecked() //리스트에서 Checkbox 값이 true인 인덱스 찾는 함수 
    {
        int idx = PersonalList.FindIndex(x => x.UserName == "진라면매운맛");
        Debug.Log(idx);
        
    }
    */


    void Save() //json 파일로 저장
    {
        string jdata = JsonConvert.SerializeObject(PersonalList);
        File.WriteAllText(Application.dataPath + "/GameChatSample/Resources/MyPersonalList.txt", jdata);

        //OnToggle(Check);
    }

    void Load() //json 파일 가져오기
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
