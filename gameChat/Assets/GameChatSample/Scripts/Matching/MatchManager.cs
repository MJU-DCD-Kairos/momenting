using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class MatchManager : MonoBehaviour
{
    public DatabaseReference Mref;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://momenting-a1670-default-rtdb.firebaseio.com/");

        // ���̾�̽��� ���� ���� ���
        Mref = FirebaseDatabase.DefaultInstance.RootReference;

        name = PlayerPrefs.GetString("name");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    class User //���� ������ ��� Ŭ����
    {
        public string name;
        public string sex;
        public bool isActive;


        public User(string name, string sex, bool isActive) //�ʱ�ȭ�ϱ� ���� ������ ���
        {
            this.name = name;
            this.sex = sex;
            this.isActive = isActive;
        }
    }

    public void AddUser()
    {
        string myname = PlayerPrefs.GetString("name");
        User user = new User(myname, PlayerPrefs.GetString("sex"), true);
        string json = JsonUtility.ToJson(user); //�����͸� json ���·� ��ȯ

        //string key = Mref.Child("Users").Push().Key;
        Mref.Child("Users").Child(myname).SetRawJsonValueAsync(json);
    }

    public void matching() //����Ÿ�ӵ����ͺ��̽��� Rooms�� 
    {
        var roomRef = FirebaseDatabase.DefaultInstance.GetReference("Rooms");
        roomRef.ValueChanged += HandleValueChanged;

        void HandleValueChanged (object sender, ValueChangedEventArgs args)
        {
            if(args.DatabaseError != null) //������ �ִٸ�
            {
                Debug.LogError(args.DatabaseError.Message); //�����޽���
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
                Debug.Log("������ �ҷ����� ����!");
            }

            else if (task.IsCompleted)
            {
                Debug.Log("������ �ҷ����� ����");
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

    public void OnclickMatching()
    {
        AddUser();
        //matching();
        
    }
}
