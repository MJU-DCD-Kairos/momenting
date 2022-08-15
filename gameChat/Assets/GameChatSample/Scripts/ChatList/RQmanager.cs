using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RQProfile;
using FireStoreScript;
using Firebase.Firestore;
using Firebase.Extensions;

public class RQmanager : MonoBehaviour
{
    //public string Username = SaveName.RQ_nickname;
    public string STATE;
    public async void Onclick_Decline() //거절 버튼
    {
        DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN);
        await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                //Dictionary<string, object> doc = snapshot.ToDictionary();
                //List<object> RQL = (List<object>)doc["RQ"];
                RQRef.UpdateAsync("RQ", FieldValue.Equals("state" , "D"));
            }
        });
    }

    public async void Onclick_Accept() //수락 버튼
    {
        DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ").Document(SaveName.RQ_nickname);
        await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            //Dictionary<string, object> updates = new Dictionary<string, object>
            //        {
            //            { "state", "A" }
            //        };
            //RQRef.UpdateAsync(field: "RQ/nickName", FieldValue.ArrayUnion(FieldValue.Equals("nickName", SaveName.RQ_nickname)));
            //RQRef.UpdateAsync("RQ", FieldValue.ArrayUnion(FieldValue.Equals("nickName",SaveName.RQ_nickname),updates));
            //RQRef.Equals("nickName");
            RQRef.UpdateAsync("state", "A");

            //if (snapshot.)
            //{
            //    Dictionary<string, object> updates = new Dictionary<string, object>
            //    {
            //        { "state", "A" }
            //    };
            //    //RQRef.UpdateAsync(field: "RQ/nickName", FieldValue.ArrayUnion(FieldValue.Equals("nickName", SaveName.RQ_nickname)));
            //    //RQRef.UpdateAsync("RQ", FieldValue.ArrayUnion(FieldValue.Equals("nickName",SaveName.RQ_nickname),updates));
            //    //RQRef.Equals("nickName");
            //    RQRef.UpdateAsync("nickName", FieldValue.ArrayUnion(updates));
            //}
        });
    }
}
