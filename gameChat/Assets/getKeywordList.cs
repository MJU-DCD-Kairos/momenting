using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getKeywordList : MonoBehaviour
{
    //저장한 키워드, 삭제할 키워드 리스트 선언
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
            Debug.Log("전체 선택된 아이들" + a + "  " + saveKWlist[a]);
        }

        for (int a = 0; a < delKWlist.Count; a++)
        {
            Debug.Log("삭제할 아이들" + a + "  " + delKWlist[a]);
        }
    }

    //토글값이 true인 키워드와 false인 키워드 리스트를 저장
    public void getKWlist()
    {
        if (KWcheckCount < 5)
        {
            if (this.GetComponent<Toggle>().isOn == true)
            {
                KWcheckCount++;
                Debug.Log("추가함 현재 개수 : " + KWcheckCount);

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
                Debug.Log("삭제함 현재 개수 : " + KWcheckCount);
                
                for (int i = 0; i < saveKWlist.Count; i++)
                {
                    if (this.transform.GetChild(0).gameObject.GetComponent<Text>().text == saveKWlist[i])
                    {
                        //같은 키워드있으면 save 리스트에서 삭제
                        saveKWlist.Remove(this.transform.GetChild(0).gameObject.GetComponent<Text>().text);

                    }
                }
            }
        }
        else if (KWcheckCount >= 5 && this.GetComponent<Toggle>().isOn == true)
        {
            Debug.Log("다섯개이상 선택할 수 없습니다.");
            notCount = true;
            this.GetComponent<Toggle>().isOn = false;
        }
        else if (KWcheckCount >= 5 && this.GetComponent<Toggle>().isOn == false && notCount == true)
        {
            notCount = false;
            Debug.Log("선택안됨 현재 개수 : " + KWcheckCount);
        }
        else if(KWcheckCount >= 5 && this.GetComponent<Toggle>().isOn == false)
        {
            KWcheckCount--;
            Debug.Log("다섯개 이상에서 취소함 현재 개수 : " + KWcheckCount);
            for (int i = 0; i < saveKWlist.Count; i++)
            {
                if (this.transform.GetChild(0).gameObject.GetComponent<Text>().text == saveKWlist[i])
                {
                    //같은 키워드있으면 save 리스트에서 삭제
                    saveKWlist.Remove(this.transform.GetChild(0).gameObject.GetComponent<Text>().text);
                }
            }
        }

    }
}
