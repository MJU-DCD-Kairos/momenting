using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnackBarManager : MonoBehaviour
{
    public static float DestroyTime = 4.3f;
    public static GameObject Canvas;

    public static void Sb_DeleteList()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "ä�ù��� �����ƽ��ϴ�.";
        Destroy(snackbar, DestroyTime);
    }
    public static void Sb_Report()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "�Ű� �����ƽ��ϴ�.";
        Destroy(snackbar, DestroyTime);
    }

    public static void KW_Sb_DeleteList()
    {
        GameObject snackbar = Instantiate(Resources.Load("Prefabs/SnackBar")) as GameObject;
        snackbar.transform.SetParent(Canvas.transform, false);
        snackbar.GetComponentInChildren<Text>().text = "Ű����� �ִ� 5������ ���� �����մϴ�.";
        Destroy(snackbar, DestroyTime);
    }
}
