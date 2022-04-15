using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]

public class List
{
    public List(string _KeywordName, string _Description, string _KeywordRatio)
    { KeywordName = _KeywordName; Description = _Description; KeywordRatio = _KeywordRatio; }

    public string KeywordName, Description, KeywordRatio;
    public bool Badge_SameKeword;
}

public class ListProfileKewordManager : MonoBehaviour
{
    //txt파일을 인스펙터에서 참조하도록 생성
    public TextAsset KeywordData;

    //리스트 배열 생성
    public List<List> KeywordList;

    //리스트를 생성할 부모 오브젝트를 참조
    public GameObject Content;

    public List<float> ratioList;

    void Awake()
    {
        LoadList();

    }

    public void LoadList()
    {

        ratioList.Clear();

        //리스트 불러오기
        string[] line = KeywordData.text.Substring(0, KeywordData.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            KeywordList.Add(new List(row[0], row[1], row[2]));

            //리스트 프리팹을 Content 부모 밑에 자식으로 생성
            GameObject ListContent = Instantiate(Resources.Load("Prefabs/Keyword")) as GameObject;
            ListContent.transform.SetParent(Content.transform, false);
            GameObject[] KeywordObj = GameObject.FindGameObjectsWithTag("Keyword");
            KeywordObj[i].GetComponent<Text>().text = row[0];

            GameObject[] DetailObj = GameObject.FindGameObjectsWithTag("Detail");
            DetailObj[i].GetComponent<Text>().text = row[1];

            GameObject[] RatioObj = GameObject.FindGameObjectsWithTag("Ratio");
            RatioObj[i].GetComponent<Text>().text = row[2];

            ratioList.Add(Convert.ToSingle(row[2]));

            Debug.Log(row[0]);
            Debug.Log(row[1]);
            Debug.Log(row[2]);
        }
    }

    public void ToggleIsOn(bool isOn)
    {
        if (isOn)
        {

        }

    }


}
