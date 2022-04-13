using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum People
{
    noone,notwo,nothree,nofour,nofive
        //보이는 순서에 따라 번호 부여
}
public class ChoMgr : MonoBehaviour
{
    public static ChoMgr instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) return;
        DontDestroyOnLoad(gameObject);
        //씬 전환이 돼도 사라지지 않도록
    }
    public People currentChoice;

}
