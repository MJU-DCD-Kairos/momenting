using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text;


public class SelectPer : MonoBehaviour
{
    public People people;
    Animator anim;
    SpriteRenderer sr;
    public SelectPer[] chars;
    public Text SenderId;
    public GameObject UserSlot;
    public GameObject Profile;


    void Start()
    {
        OnDeSelect();
        RectTransform rt = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if (ChoMgr.instance.currentChoice == people) OnSelect();
        else OnDeSelect();
    }
    private void OnMouseUpAsButton()
    {
        ChoMgr.instance.currentChoice = people;
        OnSelect();
        for(int i=0; i < chars.Length; i++)
        {
            if (chars[i] != this) chars[i].OnDeSelect();

        }

       
    }

    void OnDeSelect()
    {
        var RectTransform = transform as RectTransform;
        //RectTransform.sizeDelta = new Vector2(512, 512);
        transform.localScale = new Vector3(1f, 1f, 1f);

        // anim.SetBool("selected",true);
        // sr.color = new Color(0.5f, 0.5f, 0.5f);
        SenderId.text = "<color=#929292>" + "�����ٶ󸶹ٻ�" + "</color>";


    }
    void OnSelect()
    {
        var RectTransform = transform as RectTransform;
        //RectTransform.sizeDelta = new Vector2(592, 592);
        transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);

        // anim.SetBool("selected",true);
        //sr.color = new Color(1f, 1f, 1f);
        SenderId.text = "<color=#f56537>" + "�����ٶ󸶹ٻ�" + "</color>";
    }
}