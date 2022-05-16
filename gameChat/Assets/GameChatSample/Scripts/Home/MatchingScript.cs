using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//���̾�̽� ����Ÿ�ӵ����ͺ��̽� ����� ���� using����
using Firebase;
using Firebase.Firestore;//���� ��ť��Ʈ ȣ���� ���� ����
using Firebase.Extensions;
using Firebase.Database;

public class MatchingScript : MonoBehaviour
{

    //���̾���� �ҷ��� ���������� ���� ����
    FirebaseManager FStore;


    //�ʿ亯������
    int Mnum = 0;//��Ī���� ���� ��
    int Wnum = 0;//��Ī���� ���� ��
    List<string> MUserList = new List<string>();
    List<string> DUserList = new List<string>();
    List<string> DUserListStack = new List<string>();

    private DatabaseReference reference = null;

    public class UserInfo //����Ÿ�ӵ����ͺ��̽��� �߰��� ���� ������ Ŭ������ ����
    {
        public string uid = "";
        public string sex = "";
        public bool checkM = false; //��Ī������ üũ
        public List<string> RuidList = new List<string>();

    }

    private void Start()
    {
        FStore = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();


        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://momenting-a1670-default-rtdb.firebaseio.com/");
        // ���̾�̽��� ���� ���� ���
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
        //uid�ϳ� ȣ��
        //if(uid.ch)
        //uid�� "checking":"true"�� �߰�, ��Ī ������� ������ ǥ��




    }



    public async void AddUserInfo()
    {
        UserInfo user = new UserInfo();//�������� Ŭ���� ����

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
                //�Ű� ���������� ��� �������� �����ߵ�
                //Debug.Log(string.Format(��report {0}��, documentDictionary[��report��]));
            }
            
        });
        //�Ű���������� ����Ʈ Ruid�� �߰�, uid�� stringŸ��*/
    }
    //����Ÿ�ӵ����ͺ��̽��� UserInfo�� �߰��ؾ���.

}


