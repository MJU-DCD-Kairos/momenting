using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RQProfile;
using FireStoreScript;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.UI;

public class RQmanager : MonoBehaviour
{
    //public string Username = SaveName.RQ_nickname;
    //public string STATE;
    public async void Onclick_Decline() //거절 버튼
    {
        DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ").Document(SaveName.RQ_nickname);
        await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            RQRef.UpdateAsync("state", "D"); //디비 state를 D로 변경

            active_canvas();
            destroyPrefab();
        });
    }

    public async void Onclick_Accept() //수락 버튼
    {
        DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ").Document(SaveName.RQ_nickname);
        await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            RQRef.UpdateAsync("state", "A"); //디비 state를 A로 변경

            active_canvas();
            destroyPrefab();
        });
    }

    public async void Onclick_Check() //프리팹 최초 클릭 시
    {
        DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ").Document(SaveName.RQ_nickname);
        await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            RQRef.UpdateAsync("state", "C"); //디비 state를 C로 변경

            GameObject badge = this.gameObject.transform.GetChild(0).gameObject; //뱃지 찾기
            if (badge.activeInHierarchy) { badge.SetActive(false); } //뱃지가 활성화되어 있으면 비활성화 시킴
        });
    }

    void destroyPrefab()
    {
        //챗리스트 메인페이지 받은신청 프리팹 삭제
        for (int n = 0; n < (GameObject.Find("Group_Received").transform.childCount); n++)
        {
            if((GameObject.Find("Group_Received").transform.GetChild(n).transform.Find("Text_name").GetComponent<Text>().text) == SaveName.RQ_nickname)
            {
                GameObject.Destroy(GameObject.Find("Group_Received").transform.GetChild(n).gameObject);
            }
        }

        //챗리스트 더보기페이지 받은신청 프리팹 삭제
        if (GameObject.Find("Received_SeeMore") != null)
        {
            for (int n = 0; n < (GameObject.Find("Content_SeeMore").transform.childCount); n++)
            {
                if ((GameObject.Find("Content_SeeMore").transform.GetChild(n).transform.Find("Information").transform.Find("UserName").GetComponent<Text>().text) == SaveName.RQ_nickname)
                {
                    GameObject.Destroy(GameObject.Find("Content_SeeMore").transform.GetChild(n).gameObject);
                }
            }
            
        }
    }

    void active_canvas()
    {
        GameObject.Find("ChatList").transform.Find("ChatList_Main").gameObject.SetActive(true); //메인캔버스 활성화
        GameObject.Find("Profile_Received").SetActive(false); //프로필 캔버스 끄기
    }
}
