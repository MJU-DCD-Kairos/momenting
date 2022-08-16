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
        snackbar.GetComponentInChildren<Text>().text = "ä�ù��� �����ƽ��ϴ�.";
        Destroy(snackbar, DestroyTime);
    }
    public void Sb_Report()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "�Ű� �����ƽ��ϴ�.";
        Destroy(snackbar, DestroyTime);
    }

    public void KW_Sb_DeleteList()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "Ű����� �ִ� 5������ ���� �����մϴ�.";
        Destroy(snackbar, DestroyTime);
    }

    public void Sb_RQ_Accept()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "���� �Ϸ�Ǿ����ϴ�." + "\n" + "1:1 ä���� �����غ�����!";
        Destroy(snackbar, DestroyTime);
    }
    public void Sb_RQ_Decline()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "���� �Ϸ�Ǿ����ϴ�.";
        Destroy(snackbar, DestroyTime);
    }
}
