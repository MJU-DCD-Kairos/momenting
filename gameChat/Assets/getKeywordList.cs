using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getKeywordList : MonoBehaviour
{
    //������ Ű����, ������ Ű���� ����Ʈ ����
    public static List<string> saveKWlist = new List<string>();
    public List<string> delKWlist = new List<string>();
    public static int KWcheckCount = 0;
    public bool notCount;
    public bool notAdd;
    public GameObject overKWsnackBar;
    public float snackbartime = 4.3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void checkList()
    {
        Debug.Log(saveKWlist.Count + "//////" + delKWlist.Count);
        for (int a = 0; a < saveKWlist.Count; a++)
        {
            Debug.Log("��ü ���õ� ���̵�" + a + "  " + saveKWlist[a]);
        }

        for (int a = 0; a < delKWlist.Count; a++)
        {
            Debug.Log("������ ���̵�" + a + "  " + delKWlist[a]);
        }
    }

    //��۰��� true�� Ű����� false�� Ű���� ����Ʈ�� ����
    public void getKWlist()
    {
        if (KWcheckCount < 5)
        {
            if (this.GetComponent<Toggle>().isOn == true)
            {
                KWcheckCount++;
                Debug.Log("�߰��� ���� ���� : " + KWcheckCount);

                notAdd = false;
                for (int i = 0; i < saveKWlist.Count; i++)
                {
                    if (this.transform.GetChild(0).gameObject.GetComponent<Text>().text == saveKWlist[i])
                    {
                        notAdd = true;
                    }
                }
                if (notAdd == false)
                {
                    saveKWlist.Add(this.transform.GetChild(0).gameObject.GetComponent<Text>().text);
                }
                
            }
            else
            {
                KWcheckCount--;
                Debug.Log("������ ���� ���� : " + KWcheckCount);
                
                for (int i = 0; i < saveKWlist.Count; i++)
                {
                    if (this.transform.GetChild(0).gameObject.GetComponent<Text>().text == saveKWlist[i])
                    {
                        //���� Ű���������� save ����Ʈ���� ����
                        saveKWlist.Remove(this.transform.GetChild(0).gameObject.GetComponent<Text>().text);

                    }
                }
            }
        }
        else if (KWcheckCount >= 5 && this.GetComponent<Toggle>().isOn == true)
        {
            Debug.Log("�ټ����̻� ������ �� �����ϴ�.");
            notCount = true;
            this.GetComponent<Toggle>().isOn = false;
        }
        else if (KWcheckCount >= 5 && this.GetComponent<Toggle>().isOn == false && notCount == true)
        {
            notCount = false;
            Debug.Log("���þȵ� ���� ���� : " + KWcheckCount);
        }
        else if(KWcheckCount >= 5 && this.GetComponent<Toggle>().isOn == false)
        {
            KWcheckCount--;
            Debug.Log("�ټ��� �̻󿡼� ����� ���� ���� : " + KWcheckCount);
            for (int i = 0; i < saveKWlist.Count; i++)
            {
                if (this.transform.GetChild(0).gameObject.GetComponent<Text>().text == saveKWlist[i])
                {
                    //���� Ű���������� save ����Ʈ���� ����
                    saveKWlist.Remove(this.transform.GetChild(0).gameObject.GetComponent<Text>().text);
                }
            }
        }

    }
}