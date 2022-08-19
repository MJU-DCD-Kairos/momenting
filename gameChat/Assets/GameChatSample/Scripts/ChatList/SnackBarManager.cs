using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnackBarManager : MonoBehaviour
{
    public float DestroyTime = 6f;
    public GameObject Canvas;

    public void Sb_DeleteList()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "채팅방이 삭제됐습니다.";
        Destroy(snackbar, DestroyTime);
    }
    public void Sb_Report()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "신고가 접수됐습니다.";
        Destroy(snackbar, DestroyTime);
    }

    public void KW_Sb_DeleteList()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "키워드는 최대 5개까지 선택 가능합니다.";
        Destroy(snackbar, DestroyTime);
    }

    public void Sb_RQ_Accept()
    {
        Canvas = GameObject.Find("ChatList").transform.Find("ChatList_Main").gameObject;
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "수락 완료되었습니다." + "\n" + "1:1 채팅을 시작해보세요!";
        Destroy(snackbar, DestroyTime);
    }
    public void Sb_RQ_Decline()
    {
        Canvas = GameObject.Find("ChatList").transform.Find("ChatList_Main").gameObject;
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "거절 완료되었습니다.";
        Destroy(snackbar, DestroyTime);
    }
}
