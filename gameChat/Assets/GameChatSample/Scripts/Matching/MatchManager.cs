using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class MatchManager : MonoBehaviour
{
    /*
    public DatabaseReference Mref;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://momenting-a1670-default-rtdb.firebaseio.com/");

        // 파이어베이스의 메인 참조 얻기
        Mref = FirebaseDatabase.DefaultInstance.RootReference;

        name = PlayerPrefs.GetString("name");
        OnclickMatching();
        AddUser();
    }
    
    class User //유저 정보를 담는 클래스
    {
        public string name;
        public string sex;
        public bool isActive;


        public User(string name, string sex, bool isActive) //초기화하기 쉽게 생성자 사용
        {
            this.name = name;
            this.sex = sex;
            this.isActive = isActive;
        }
    }
    
    public void AddUser()
    {
        string myname = PlayerPrefs.GetString("name");
        User user = new User(myname, PlayerPrefs.GetString("sex"), true); //닉네임, 성별, 매칭가능여부를 클래스에 추가
        string json = JsonUtility.ToJson(user); //데이터를 json 형태로 변환

        //string key = Mref.Child("Users").Push().Key;
        Mref.Child("Users").Child(myname).SetRawJsonValueAsync(json); //"Users"DB에 생성
    }
   
    public void OnclickMatching()
    {
        //bool matchingDB = Convert.ToBoolean(PlayerPrefs.GetString("isInMatchingDB"));
        string myname = PlayerPrefs.GetString("name");
        if (matchingDB == false) //매칭데이터베이스에 정보가 추가되어있지 않다면
        {
            //AddUser(); //매칭 데이터베이스에 유저정보(닉네임,성별,매칭가능여부) 생성
        }
        else
        {
            Mref.Child("Users").Child(myname).SetValueAsync(true); //DB에 isActive 상태 변경
            Debug.Log(myname);
        }


        //matching();

    }
    */
    public void matching() //리얼타임데이터베이스의 Rooms DB에 들어감
    {

        var roomRef = FirebaseDatabase.DefaultInstance.GetReference("Rooms");
        roomRef.ValueChanged += HandleValueChanged;

        void HandleValueChanged (object sender, ValueChangedEventArgs args)
        {
            if(args.DatabaseError != null) //에러가 있다면
            {
                Debug.LogError(args.DatabaseError.Message); //에러메시지
                return;
            }
        }

    }
    
    
    public void LoadData()
    {
        
        FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("데이터 불러오기 오류!");
            }

            else if (task.IsCompleted)
            {
                Debug.Log("데이터 불러오기 성공");
                DataSnapshot snapshot = task.Result;
                foreach(DataSnapshot data in snapshot.Children)
                {

                    IDictionary username = (IDictionary)data.Value;
                    //RTname = username["name"].ToString();
                    IDictionary usersex = (IDictionary)data.Value;
                    //Debug.Log("Name: " + userinfo["name"]);
                    
                }
            }
        });
        
    }
    

}
