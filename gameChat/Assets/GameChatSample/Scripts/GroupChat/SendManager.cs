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
        Cuser = SelectPer.ChoName; //상대 유저닉네임 변수에 저장
        Debug.Log("최종선택한 유저: " + Cuser);
        //Cuser = "현진"; //테스트용
        RQdb();
    }


    public string RQCheck;
    
    async void RQdb() //받은신청 디비에 저장
    {
        CollectionReference userDB = FirebaseManager.db.Collection("userInfo").Document(Cuser).Collection("RQ"); //선택한유저 db 참조
        Dictionary<string, object> addRQ = new Dictionary<string, object> //RQ에 추가할 유저정보
                {
                    { "nickName" , FirebaseManager.GCN }, //닉네임
                    { "sex" , FirebaseManager.sex }, //성별
                    { "Info" , FirebaseManager.myintroduction }, //한줄소개
                    { "age", FirebaseManager.age }, //나이
                    { "time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") }, //신청한 시간
                    { "state", "N" } //상태(N/C/D/A)
                };

        await userDB.Document(FirebaseManager.GCN).SetAsync(addRQ);
        Debug.Log("저장완료!");

        //await userDB.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        //{
        //    DocumentSnapshot snapshot = task.Result;
        //    if (snapshot.Exists)
        //    {
        //        Dictionary<string, object> docDictionary = snapshot.ToDictionary();

        //        Dictionary<string, object> addRQ = new Dictionary<string, object> //RQ에 추가할 유저정보
        //        {
        //            { "nickName" , FirebaseManager.GCN }, //닉네임
        //            { "sex" , FirebaseManager.sex }, //성별
        //            { "Info" , FirebaseManager.myintroduction }, //한줄소개
        //            { "age", FirebaseManager.age }, //나이
        //            { "time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") }, //신청한 시간
        //            { "state", "N" } //상태(N/C/D/A)
        //        };

        //        userDB.Document(FirebaseManager.GCN).SetAsync(addRQ);
        //        Debug.Log("저장완료!");
        //    }
        //    else
        //    {
        //        Debug.Log("유저정보 없음");
        //    }
        //});
        

    }
}
