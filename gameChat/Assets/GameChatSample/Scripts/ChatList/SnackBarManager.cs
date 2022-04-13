using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnackBarManager : MonoBehaviour
{
    public float DestroyTime = 2.5f;
    public GameObject Canvas_PersonalSeeMore;

    private void Start()
    {
        
    }
    public void Sb_DeleteList()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas_PersonalSeeMore.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "채팅방이 삭제됐습니다.";
        Destroy(snackbar, DestroyTime);
    }
}
