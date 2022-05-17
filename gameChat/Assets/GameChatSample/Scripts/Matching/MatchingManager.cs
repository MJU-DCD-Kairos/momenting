using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���̾�̽� ����Ÿ�ӵ����ͺ��̽� ����� ���� using����
using Firebase;
using Firebase.Database; //�ǽð������ͺ��̽�
using Firebase.Firestore; //���̾���
using Firebase.Extensions;



public class MatchingManager : MonoBehaviour
{

    //���̾���� �ҷ��� ���������� ���� ����
    //FirebaseManager FStore;
    public static FirebaseFirestore db;

    //�ʿ亯������
    int Mnum = 0;//��Ī���� ���� ��
    int Wnum = 0;//��Ī���� ���� ��
    List<string> MUserList = new List<string>();
    List<string> DUserList = new List<string>();
    List<string> DUserListStack = new List<string>();

    private static MatchingManager Instance;
    public DatabaseReference reference;

    public class UserInfo //����Ÿ�ӵ����ͺ��̽��� �߰��� ���� ������ Ŭ������ ����
    {
        public string uid = ""; //�����������̵�
        public string sex = ""; //����
        public bool isActive = false; //��Ī�������� ����
        

        /*
        public UserInfo(string uid, string sex, bool checkM, List<string> RuidList)
        {
            this.uid = uid;
            this.sex = sex;
            this.checkM = checkM;
            this.RuidList = RuidList;
        }
        */
    }

    public class UserReport
    {
        public List<string> RuidList = new List<string>(); //�Ű�������
    }

    //public string uid = PlayerPrefs.GetString("uid"); //�����������̵�
    //public string sex = ""; //����
    //public bool checkM = false; //��Ī������ üũ
    //public List<string> RuidList = new List<string>(); //�Ű�������

    public void Start()
    {
        //FStore = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();


        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://momenting-a1670-default-rtdb.firebaseio.com/");
        
        // ���̾�̽��� ���� ���� ���
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        
        //db = FirebaseFirestore.DefaultInstance;

        string id = PlayerPrefs.GetString("uid");
        Debug.Log("�������̵�: " + id);
    }

    public void Onclick_Matching()
    {
        AddUserInfo();
        writeNewUser(); //����Ÿ�ӵ����ͺ��̽��� �������� ����

    }


    public string id;

    UserInfo user = new UserInfo(); //���� ���� Ŭ���� ����
    UserReport userR = new UserReport(); //���� �Ű� Ŭ���� ����

    public async void AddUserInfo()
    {
        db = FirebaseFirestore.DefaultInstance;
        id = PlayerPrefs.GetString("uid");
        user.uid = id; //�������̵� �߰�
        
        //���� �ҷ�����
        CollectionReference userRef = db.Collection("Users").Document(id).Collection("profile"); //profile�÷��� ����
        QuerySnapshot snapshot = await userRef.GetSnapshotAsync();
        
        foreach (DocumentSnapshot doc in snapshot.Documents)
        {
            Dictionary<string, object> docDictionary = doc.ToDictionary();

            user.sex = docDictionary["sex"] as string; //���� �߰�
        }

        //�Ű�����������Ʈ �ҷ�����
        CollectionReference ReportRef = db.Collection("Users").Document(id).Collection("report"); //profile�÷��� ����
        QuerySnapshot ReportSnapshot = await ReportRef.GetSnapshotAsync();

        foreach (DocumentSnapshot doc in ReportSnapshot.Documents)
        {
            Dictionary<string, object> docDictionary = doc.ToDictionary();

            userR.RuidList.Add(docDictionary["uid"] as string); //�Ű�����������Ʈ�� �߰�
        }

        user.isActive = true; //��Ī���� ���·� ��ȯ

        Debug.Log(user.uid);
        Debug.Log(user.sex);
        Debug.Log(user.isActive);

        for(int i =0; i<userR.RuidList.Count; i++)
        {
            Debug.Log(userR.RuidList[i]);
        }


    }

    private void writeNewUser()
    {
        string userInfoData = JsonUtility.ToJson(user);
        string userReportData = JsonUtility.ToJson(userR);

        reference.Child("Users").Child(id).SetValueAsync(userInfoData);
        reference.Child("Users").Child(id).Child("report").UpdateChildrenAsync(userReportData);
    }
}
