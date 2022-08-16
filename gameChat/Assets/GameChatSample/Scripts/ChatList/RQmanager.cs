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
    public async void Onclick_Decline() //���� ��ư
    {
        DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ").Document(SaveName.RQ_nickname);
        await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            RQRef.UpdateAsync("state", "D"); //��� state�� D�� ����

            active_canvas();
            destroyPrefab();
        });
    }

    public async void Onclick_Accept() //���� ��ư
    {
        DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ").Document(SaveName.RQ_nickname);
        await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            RQRef.UpdateAsync("state", "A"); //��� state�� A�� ����

            active_canvas();
            destroyPrefab();
        });
    }

    public async void Onclick_Check() //������ ���� Ŭ�� ��
    {
        DocumentReference RQRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN).Collection("RQ").Document(SaveName.RQ_nickname);
        await RQRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            RQRef.UpdateAsync("state", "C"); //��� state�� C�� ����

            GameObject badge = this.gameObject.transform.GetChild(0).gameObject; //���� ã��
            if (badge.activeInHierarchy) { badge.SetActive(false); } //������ Ȱ��ȭ�Ǿ� ������ ��Ȱ��ȭ ��Ŵ
        });
    }

    void destroyPrefab()
    {
        //ê����Ʈ ���������� ������û ������ ����
        for (int n = 0; n < (GameObject.Find("Group_Received").transform.childCount); n++)
        {
            if((GameObject.Find("Group_Received").transform.GetChild(n).transform.Find("Text_name").GetComponent<Text>().text) == SaveName.RQ_nickname)
            {
                GameObject.Destroy(GameObject.Find("Group_Received").transform.GetChild(n).gameObject);
            }
        }

        //ê����Ʈ ������������ ������û ������ ����
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
        GameObject.Find("ChatList").transform.Find("ChatList_Main").gameObject.SetActive(true); //����ĵ���� Ȱ��ȭ
        GameObject.Find("Profile_Received").SetActive(false); //������ ĵ���� ����
    }
}
