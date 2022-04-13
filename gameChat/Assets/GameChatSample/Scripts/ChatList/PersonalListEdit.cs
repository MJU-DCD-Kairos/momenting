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
        public List(string _UserName, string _Recent, string _Chat, string _Badge_MannerEvaluation)
        { UserName = _UserName; Recent = _Recent; Chat = _Chat; Badge_MannerEvaluation = _Badge_MannerEvaluation; }


        public Image ProfileImage;
        public string UserName, Recent, Chat;
        public string Badge_MannerEvaluation;

    }
   

    public List<List> PersonalList; //���� ������ �� ����Ʈ �迭 ����
    public List<Toggle> CheckedToggles; //üũ�� ����� �����ϴ� ����Ʈ
    public List<Toggle> Toggles; //����Ʈ�� ��� ����� �����ϴ� ����Ʈ
    public List<GameObject> ListContents; //����Ʈ Ŭ�� ������Ʈ�� �����ϴ� ����Ʈ
    public Toggle checkbox;

    public GameObject Content_ChatList; //����Ʈ�� ������ �θ� ������Ʈ�� ����
    public GameObject Content_Personal; //����Ʈ�� ������ �θ� ������Ʈ�� ����
    public GameObject Content_PersonalEdit; //����Ʈ�� ������ �θ� ������Ʈ�� ����
    
    public int i; //����Ʈ �ε���
    
    public GameObject ChatList; //ĵ���� ����
    public GameObject Personal_Edit; //ĵ���� ����
    public GameObject Personal_SeeMore; //ĵ���� ����
    void Start()
    {
        MyList();
    }

    StreamWriter writer;
    //StreamReader reader;
    //public string textValue;
    public void MyList()
    {
        //����Ʈ�� �ʱ�ȭ
        Toggles.Clear();
        ListContents.Clear();
        CheckedToggles.Clear();
        PersonalList.Clear();

        //����Ʈ �ҷ�����
        //reader = new StreamReader(Application.dataPath + "/GameChatSample/Resources/text/WriteTXT.txt");
        //string texValue = reader.ReadToEnd();
        //reader.Close();
        string textValue = File.ReadAllText(Application.dataPath + "/GameChatSample/Resources/text/WriteTXT.txt");
        string[] line = textValue.Substring(0, textValue.Length - 1).Split('\n');
        for (i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            //����Ʈ �������� Content �θ� �ؿ� �ڽ����� ����
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

            //�г����� UserName ������Ʈ�� �ؽ�Ʈ ������Ʈ�� �Ҵ�
            GameObject[] UserNameList = GameObject.FindGameObjectsWithTag("UserName");
            UserNameList[i].GetComponent<Text>().text = row[0];

            //�޽��� �ð��� Recent ������Ʈ�� �ؽ�Ʈ ������Ʈ�� �Ҵ�
            GameObject[] RecentList = GameObject.FindGameObjectsWithTag("Recent");
            RecentList[i].GetComponent<Text>().text = row[1];

            //�޽����̸����⸦ ChatPreview ������Ʈ�� �ؽ�Ʈ ������Ʈ�� �Ҵ�
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
    public Text Label; //����Ʈ ���� ���̺� ����

    public void changeLabel() //���� ���̺� ����
    {
        int count = CheckedToggles.Count; //���õ� ����Ʈ ����
        int countAll = PersonalList.Count; //����Ʈ ��ü ����
        Label.text = "��ü���� (" + "<color=#f56537>" + count + "</color>" + "/" + countAll + ")";
    }

    public void DestroyList() 
    {
        LoadDialog(); //�����ϱ� ��ư ��ġ �� ���̾�α� ����
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

    public void ConfirmDialog() //���̾�α� �����ϱ� ��ư ��ġ �� ������ ����Ʈ ����
    {
        for (int i = PersonalList.Count - 1; i >= 0; i--)
        {
            Toggle currentToggle = Toggles[i].GetComponent<Toggle>();

            if (currentToggle.isOn)
            {
                Destroy(ListContents[i]); //����Ʈ ������ ���� ������Ʈ ����
                PersonalList.Remove(PersonalList[i]); //����Ʈ ����
                //Destroy(Toggles[i]);
            }

        }
        writeTXT();
        //Personal_Edit.SetActive(false);
        //Personal_SeeMore.SetActive(true);
        EnabledBtn.SetActive(false);
        GameObject.FindGameObjectWithTag("SnackbarManager").GetComponent<SnackBarManager>().Sb_DeleteList();
        MyList(); //����Ʈ ����
    }
    
    public void writeTXT() //����Ʈ �����
    {
        writer = new StreamWriter(Application.dataPath + "/GameChatSample/Resources/text/WriteTXT.txt", false); //���� �̸��� ���� ���Ϸ� ���
        
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
    
    public GameObject AllSelcetBtn; //��ü���� ��� ����
    public void Allcheck(bool select) //��ü ���� ���
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
        if (CheckedToggles.Count == 0) //üũ�� ��� ����Ʈ�� ����ִٸ�
        {
            EnabledBtn.SetActive(false); //�����ϱ� Ȱ��ȭ ��ư�� ��Ȱ��ȭ��Ŵ
        }

        else //üũ�� ��� ����Ʈ�� ������� �ʴٸ�
        {
            EnabledBtn.SetActive(true); //�����ϱ� Ȱ��ȭ ��ư�� Ȱ��ȭ��Ŵ
        }
    }
    private void Awake()
    {
        if (Personal_Edit == true) { AllCheckBtn.onValueChanged.AddListener(Allcheck); }
    }

}
