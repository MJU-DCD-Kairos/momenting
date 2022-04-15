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
    //txt������ �ν����Ϳ��� �����ϵ��� ����
    public TextAsset KeywordData;

    //����Ʈ �迭 ����
    public List<List> KeywordList;

    //����Ʈ�� ������ �θ� ������Ʈ�� ����
    public GameObject Content;

    public List<float> ratioList;

    void Awake()
    {
        LoadList();

    }

    public void LoadList()
    {

        ratioList.Clear();

        //����Ʈ �ҷ�����
        string[] line = KeywordData.text.Substring(0, KeywordData.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            KeywordList.Add(new List(row[0], row[1], row[2]));

            //����Ʈ �������� Content �θ� �ؿ� �ڽ����� ����
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
