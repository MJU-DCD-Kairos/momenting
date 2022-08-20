using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FireStoreScript;
using Firebase.Firestore;

namespace editprofile
{
    public class MyProfileEditManager : MonoBehaviour
    {
        public Text txtIntro_edit;
        public Text txtPlaceholder;
        public static string EDIT_INTRO;
        
        //페이지 최초 로드 시 정보 불러오기
        public void Load_introduction()
        {
            txtPlaceholder.text = FirebaseManager.myintroduction;
            txtIntro_edit.text = FirebaseManager.myintroduction;
        }

        //프로필 편집
        public async void Edit_Save()
        {
            DocumentReference userRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN);
            //Dictionary<string, object> addRQ = new Dictionary<string, object> 
            //        {
            //            { "Info" ,  }, //한줄소개
            //        };

            await userRef.UpdateAsync("Introduction", txtIntro_edit.text);
            Debug.Log("저장완료!");
            EDIT_INTRO = txtIntro_edit.text;
            Debug.Log("edit 씬 == " + EDIT_INTRO);
        }

        //public void Edit_Change()
        //{
        //    EDIT_INTRO = txtIntro.text;
        //}
    }
}
