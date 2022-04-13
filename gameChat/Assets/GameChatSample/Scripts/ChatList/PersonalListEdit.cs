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
        public List(string _UserName, string _Recent, string _Chat, string _Badge_MannerEvaluation)
        { UserName = _UserName; Recent = _Recent; Chat = _Chat; Badge_MannerEvaluation = _Badge_MannerEvaluation; }


        public Image ProfileImage;
        public string UserName, Recent, Chat;
        public string Badge_MannerEvaluation;

    }
   

    public List<List> PersonalList; //유저 정보가 들어갈 리스트 배열 생성
    public List<Toggle> CheckedToggles; //체크된 토글을 저장하는 리스트
    public List<Toggle> Toggles; //리스트의 모든 토글을 저장하는 리스트
    public List<GameObject> ListContents; //리스트 클론 오브젝트를 저장하는 리스트
    public Toggle checkbox;

    public GameObject Content_ChatList; //리스트를 생성할 부모 오브젝트를 참조
    public GameObject Content_Personal; //리스트를 생성할 부모 오브젝트를 참조
    public GameObject Content_PersonalEdit; //리스트를 생성할 부모 오브젝트를 참조
    
    public int i; //리스트 인덱스
    
    public GameObject ChatList; //캔버스 참조
    public GameObject Personal_Edit; //캔버스 참조
    public GameObject Personal_SeeMore; //캔버스 참조
    void Start()
    {
        MyList();
    }

    StreamWriter writer;
    //StreamReader reader;
    //public string textValue;
    public void MyList()
    {
        //리스트들 초기화
        Toggles.Clear();
        ListContents.Clear();
        CheckedToggles.Clear();
        PersonalList.Clear();

        //리스트 불러오기
        //reader = new StreamReader(Application.dataPath + "/GameChatSample/Resources/text/WriteTXT.txt");
        //string texValue = reader.ReadToEnd();
        //reader.Close();
        string textValue = File.ReadAllText(Application.dataPath + "/GameChatSample/Resources/text/WriteTXT.txt");
        string[] line = textValue.Substring(0, textValue.Length - 1).Split('\n');
        for (i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            //리스트 프리팹을 Content 부모 밑에 자식으로 생성
            if (Personal_Edit.activeInHierarchy == true)
            {
                GameObject ListContent = Instantiate(Resources.Load("Prefabs/List_Personal_Edit")) as GameObject;
                ListContent.transform.SetParent(Content_PersonalEdit.transform, false);
                checkbox = ListContent.GetComponentInChildren<Toggle>();
                Toggles.Add(checkbox);
                ListContents.Add(ListContent);
            }

            else if (Personal_SeeMore.activeInHierarchy == true)
            {
                GameObject ListContent = Instantiate(Resources.Load("Prefabs/List_PersonalChat")) as GameObject;
                ListContent.transform.SetParent(Content_Personal.transform, false);
            }

            else if(ChatList.activeInHierarchy == true)
            {
                GameObject ListContent = Instantiate(Resources.Load("Prefabs/List_PersonalChat")) as GameObject;
                ListContent.transform.SetParent(Content_ChatList.transform, false);
            }
            PersonalList.Add(new List(row[0], row[1], row[2], row[3]));

            //닉네임을 UserName 오브젝트의 텍스트 컴포넌트에 할당
            GameObject[] UserNameList = GameObject.FindGameObjectsWithTag("UserName");
            UserNameList[i].GetComponent<Text>().text = row[0];

            //메시지 시간을 Recent 오브젝트의 텍스트 컴포넌트에 할당
            GameObject[] RecentList = GameObject.FindGameObjectsWithTag("Recent");
            RecentList[i].GetComponent<Text>().text = row[1];

            //메시지미리보기를 ChatPreview 오브젝트의 텍스트 컴포넌트에 할당
            GameObject[] ChatPreviewList = GameObject.FindGameObjectsWithTag("ChatPreview");
            ChatPreviewList[i].GetComponent<Text>().text = row[2];

            if(PersonalList[i].Badge_MannerEvaluation == "TRUE")
            {
                GameObject[] Badge = GameObject.FindGameObjectsWithTag("Information");
                Badge[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            
        }
        changeLabel();
    }


    
    public GameObject DisabledBtn;
    public GameObject EnabledBtn;
    public Toggle AllCheckBtn;
    public Text Label; //리스트 개수 레이블 참조

    public void changeLabel() //개수 레이블 변경
    {
        int count = CheckedToggles.Count; //선택된 리스트 개수
        int countAll = PersonalList.Count; //리스트 전체 개수
        Label.text = "전체선택 (" + "<color=#f56537>" + count + "</color>" + "/" + countAll + ")";
    }

    public void DestroyList() 
    {
        LoadDialog(); //삭제하기 버튼 터치 시 다이얼로그 띄우기
    }

    public GameObject Dialogs;
    private void LoadDialog()
    {
        Dialogs.SetActive(true);
    }

    public void DismissDialog()
    {
        Dialogs.SetActive(false);
    }

    public void ConfirmDialog() //다이얼로그 삭제하기 버튼 터치 시 선택한 리스트 삭제
    {
        for (int i = PersonalList.Count - 1; i >= 0; i--)
        {
            Toggle currentToggle = Toggles[i].GetComponent<Toggle>();

            if (currentToggle.isOn)
            {
                Destroy(ListContents[i]); //리스트 프리팹 복제 오브젝트 삭제
                PersonalList.Remove(PersonalList[i]); //리스트 삭제
                //Destroy(Toggles[i]);
            }

        }
        writeTXT();
        //Personal_Edit.SetActive(false);
        //Personal_SeeMore.SetActive(true);
        EnabledBtn.SetActive(false);
        GameObject.FindGameObjectWithTag("SnackbarManager").GetComponent<SnackBarManager>().Sb_DeleteList();
        MyList(); //리스트 갱신
    }
    
    public void writeTXT() //리스트 덮어쓰기
    {
        writer = new StreamWriter(Application.dataPath + "/GameChatSample/Resources/text/WriteTXT.txt", false); //같은 이름을 가진 파일로 덮어씀
        
        for (int j = 0; j < PersonalList.Count; j++) 
        {
            string name = PersonalList[j].UserName;
            string recent = PersonalList[j].Recent;
            string chat = PersonalList[j].Chat;
            string mannerBadge = PersonalList[j].Badge_MannerEvaluation;
            writer.WriteLine(name + "\t" + recent + "\t" + chat + "\t" + mannerBadge);
        }
        writer.Close();

    }
    
    public GameObject AllSelcetBtn; //전체선택 토글 참조
    public void Allcheck(bool select) //전체 선택 토글
    {
        if (AllCheckBtn.isOn == true)
        {
            for (int i = PersonalList.Count - 1; i >= 0; i--)
            {
                Toggles[i].GetComponent<Toggle>().isOn = true;
                AllSelcetBtn.SetActive(true);
            }
            
        }
        else
        {
            for (int i = PersonalList.Count - 1; i >= 0; i--)
            {
                Toggles[i].GetComponent<Toggle>().isOn = false;
            }
            AllSelcetBtn.SetActive(false);
        }
       
    }

    public void OnEnableBtn()
    {
        if (CheckedToggles.Count == 0) //체크된 토글 리스트가 비어있다면
        {
            EnabledBtn.SetActive(false); //삭제하기 활성화 버튼을 비활성화시킴
        }

        else //체크된 토글 리스트가 비어있지 않다면
        {
            EnabledBtn.SetActive(true); //삭제하기 활성화 버튼을 활성화시킴
        }
    }
    private void Awake()
    {
        if (Personal_Edit == true) { AllCheckBtn.onValueChanged.AddListener(Allcheck); }
    }

}
