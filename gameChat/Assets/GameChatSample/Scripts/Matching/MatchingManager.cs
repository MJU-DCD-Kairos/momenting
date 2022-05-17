using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//파이어베이스 리얼타임데이터베이스 사용을 위한 using선언
using Firebase;
using Firebase.Database; //실시간데이터베이스
using Firebase.Firestore; //파이어스토어
using Firebase.Extensions;



public class MatchingManager : MonoBehaviour
{

    //파이어스토어에서 불러올 유저정보로 참조 선언
    //FirebaseManager FStore;
    public static FirebaseFirestore db;

    //필요변수선언
    int Mnum = 0;//매칭에서 남자 수
    int Wnum = 0;//매칭에서 여자 수
    List<string> MUserList = new List<string>();
    List<string> DUserList = new List<string>();
    List<string> DUserListStack = new List<string>();

    private static MatchingManager Instance;
    public DatabaseReference reference;

    public class UserInfo //리얼타임데이터베이스에 추가할 유저 정보를 클래스로 생성
    {
        public string uid = ""; //유저고유아이디
        public string sex = ""; //성별
        public bool isActive = false; //매칭가능한지 여부
        

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
        public List<string> RuidList = new List<string>(); //신고한유저
    }

    //public string uid = PlayerPrefs.GetString("uid"); //유저고유아이디
    //public string sex = ""; //성별
    //public bool checkM = false; //매칭중인지 체크
    //public List<string> RuidList = new List<string>(); //신고한유저

    public void Start()
    {
        //FStore = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();


        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://momenting-a1670-default-rtdb.firebaseio.com/");
        
        // 파이어베이스의 메인 참조 얻기
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        
        //db = FirebaseFirestore.DefaultInstance;

        string id = PlayerPrefs.GetString("uid");
        Debug.Log("유저아이디: " + id);
    }

    public void Onclick_Matching()
    {
        AddUserInfo();
        writeNewUser(); //리얼타임데이터베이스에 유저정보 저장

    }


    public string id;

    UserInfo user = new UserInfo(); //유저 정보 클래스 생성
    UserReport userR = new UserReport(); //유저 신고 클래스 생성

    public async void AddUserInfo()
    {
        db = FirebaseFirestore.DefaultInstance;
        id = PlayerPrefs.GetString("uid");
        user.uid = id; //유저아이디 추가
        
        //성별 불러오기
        CollectionReference userRef = db.Collection("Users").Document(id).Collection("profile"); //profile컬렉션 참조
        QuerySnapshot snapshot = await userRef.GetSnapshotAsync();
        
        foreach (DocumentSnapshot doc in snapshot.Documents)
        {
            Dictionary<string, object> docDictionary = doc.ToDictionary();

            user.sex = docDictionary["sex"] as string; //성별 추가
        }

        //신고한유저리스트 불러오기
        CollectionReference ReportRef = db.Collection("Users").Document(id).Collection("report"); //profile컬렉션 참조
        QuerySnapshot ReportSnapshot = await ReportRef.GetSnapshotAsync();

        foreach (DocumentSnapshot doc in ReportSnapshot.Documents)
        {
            Dictionary<string, object> docDictionary = doc.ToDictionary();

            userR.RuidList.Add(docDictionary["uid"] as string); //신고한유저리스트에 추가
        }

        user.isActive = true; //매칭가능 상태로 전환

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
