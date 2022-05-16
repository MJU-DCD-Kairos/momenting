using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//파이어베이스 리얼타임데이터베이스 사용을 위한 using선언
using Firebase;
using Firebase.Firestore;//유저 도큐먼트 호출을 위해 선언
using Firebase.Extensions;
using Firebase.Database;

public class MatchingScript : MonoBehaviour
{

    //파이어스토어에서 불러올 유저정보로 참조 선언
    FirebaseManager FStore;


    //필요변수선언
    int Mnum = 0;//매칭에서 남자 수
    int Wnum = 0;//매칭에서 여자 수
    List<string> MUserList = new List<string>();
    List<string> DUserList = new List<string>();
    List<string> DUserListStack = new List<string>();

    private DatabaseReference reference = null;

    public class UserInfo //리얼타임데이터베이스에 추가할 유저 정보를 클래스로 생성
    {
        public string uid = "";
        public string sex = "";
        public bool checkM = false; //매칭중인지 체크
        public List<string> RuidList = new List<string>();

    }

    private void Start()
    {
        FStore = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();


        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://momenting-a1670-default-rtdb.firebaseio.com/");
        // 파이어베이스의 메인 참조 얻기
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //LoadData();
        //AddUserInfo();
    }



    // Update is called once per frame
    void Update()
    {

    }


    void Matching()
    {
        //uid하나 호출
        //if(uid.ch)
        //uid에 "checking":"true"값 추가, 매칭 대기중인 유저를 표시




    }



    public async void AddUserInfo()
    {
        UserInfo user = new UserInfo();//유저정보 클래스 생성

        CollectionReference userRef = FStore.db.Collection("Users");
        QuerySnapshot snapshot = await userRef.GetSnapshotAsync();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Console.WriteLine("User: {0}", document.Id);
            Dictionary<string, object> documentDictionary = document.ToDictionary();
            Console.WriteLine("First: {0}", documentDictionary["First"]);
            if (documentDictionary.ContainsKey("Middle"))
            {
                Console.WriteLine("Middle: {0}", documentDictionary["Middle"]);
            }
            Console.WriteLine("Last: {0}", documentDictionary["Last"]);
            Console.WriteLine("Born: {0}", documentDictionary["Born"]);
            Console.WriteLine();
        }





        /*





        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {

            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                user.uid = documentDictionary["uid"].ToString();
                user.sex = documentDictionary["sex"].ToString();
                user.RuidList.Add(documentDictionary["report"].ToString());
                Debug.Log(user.uid);
                Debug.Log(user.sex);
                //신고 계정정보가 어떻게 들어오는지 찍어봐야됨
                //Debug.Log(string.Format(“report {0}”, documentDictionary[“report”]));
            }
            
        });
        //신고계정정보를 리스트 Ruid에 추가, uid는 string타입*/
    }
    //리얼타임데이터베이스에 UserInfo를 추가해야함.

}


