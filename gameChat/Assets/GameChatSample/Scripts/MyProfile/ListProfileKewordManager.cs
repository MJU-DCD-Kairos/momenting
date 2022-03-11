using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]

public class List
{
    public List(string _KeywordName, string _Description)
    { KeywordName = _KeywordName; Description = _Description; }

    public string KeywordName, Description;
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


    void Start()
    {
        LoadList();

    }

    public void LoadList()
    {
        //����Ʈ �ҷ�����
        string[] line = KeywordData.text.Substring(0, KeywordData.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            KeywordList.Add(new List(row[0], row[1]));

            //����Ʈ �������� Content �θ� �ؿ� �ڽ����� ����
            GameObject ListContent = Instantiate(Resources.Load("Prefabs/Keyword")) as GameObject;
            ListContent.transform.SetParent(Content.transform, false);
            GameObject[] KeywordObj = GameObject.FindGameObjectsWithTag("Keyword");
            KeywordObj[i].GetComponent<Text>().text = row[0];

            GameObject[] DetailObj = GameObject.FindGameObjectsWithTag("Detail");
            DetailObj[i].GetComponent<Text>().text = row[1];

            Debug.Log(row[0]);
            Debug.Log(row[1]);
        }
    }

    public void ToggleIsOn(bool isOn)
    {
        if (isOn)
        {

        }

    }


}
