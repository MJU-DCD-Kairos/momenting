using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum People
{
    noone,notwo,nothree,nofour,nofive
        //���̴� ������ ���� ��ȣ �ο�
}
public class ChoMgr : MonoBehaviour
{
    public static ChoMgr instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) return;
        DontDestroyOnLoad(gameObject);
        //�� ��ȯ�� �ŵ� ������� �ʵ���
    }
    public People currentChoice;

}
