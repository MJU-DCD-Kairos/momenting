using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Extensions;
using System;

public class UserToken : MonoBehaviour
{
    public static FirebaseFirestore db;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void signupComplete()
    {
        //配奴 db 积己
        
        Dictionary<string, object> newTokenData = new Dictionary<string, object> //货肺款 雕寂呈府 积己
        {
            {"name", PlayerPrefs.GetString("name") },
            {"token" , PlayerPrefs.GetString("token") }
        };
        db.Collection("userToken").AddAsync(newTokenData).ContinueWithOnMainThread(task => {
            DocumentReference addedDocRef = task.Result;
            Debug.Log(String.Format("Added document with ID: {0}.", addedDocRef.Id));
        }); 
    }
}
