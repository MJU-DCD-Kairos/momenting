using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using FireStoreScript;
using Firebase.Extensions;
using SelectMgr;

public class SendManager : MonoBehaviour
{
    public string Cuser;
    public void sendRQ()
    {
        //Debug.Log(ChoMgr.instance.currentChoice);
        Cuser = SelectPer.ChoName; //��� �����г��� ������ ����
        Debug.Log("���������� ����: " + Cuser);
        //Cuser = "����"; //�׽�Ʈ��
        RQdb();
    }


    public string RQCheck;
    
    async void RQdb() //������û ��� ����
    {
        CollectionReference userDB = FirebaseManager.db.Collection("userInfo").Document(Cuser).Collection("RQ"); //���������� db ����
        Dictionary<string, object> addRQ = new Dictionary<string, object> //RQ�� �߰��� ��������
                {
                    { "nickName" , FirebaseManager.GCN }, //�г���
                    { "sex" , FirebaseManager.sex }, //����
                    { "Info" , FirebaseManager.myintroduction }, //���ټҰ�
                    { "age", FirebaseManager.age }, //����
                    { "time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") }, //��û�� �ð�
                    { "state", "N" } //����(N/C/D/A)
                };

        await userDB.Document(FirebaseManager.GCN).SetAsync(addRQ);
        Debug.Log("����Ϸ�!");

        //await userDB.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        //{
        //    DocumentSnapshot snapshot = task.Result;
        //    if (snapshot.Exists)
        //    {
        //        Dictionary<string, object> docDictionary = snapshot.ToDictionary();

        //        Dictionary<string, object> addRQ = new Dictionary<string, object> //RQ�� �߰��� ��������
        //        {
        //            { "nickName" , FirebaseManager.GCN }, //�г���
        //            { "sex" , FirebaseManager.sex }, //����
        //            { "Info" , FirebaseManager.myintroduction }, //���ټҰ�
        //            { "age", FirebaseManager.age }, //����
        //            { "time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") }, //��û�� �ð�
        //            { "state", "N" } //����(N/C/D/A)
        //        };

        //        userDB.Document(FirebaseManager.GCN).SetAsync(addRQ);
        //        Debug.Log("����Ϸ�!");
        //    }
        //    else
        //    {
        //        Debug.Log("�������� ����");
        //    }
        //});
        

    }
}
