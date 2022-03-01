using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class csvReaderChatRoomName : MonoBehaviour
{

    //csv파일을 외부에서 인스펙터에서 직접 참조할 수 있도록 생성
    public TextAsset csvfile;

    //오늘의 질문을 넣어줄 UI텍스트 오브젝트를 인스펙터로 참조받기위한 선언
    public Text ChatRoomNameText;

    //CSV파일의 행 개수를 인스펙터상에서 입력하기 위한 퍼블릭 변수 선언
    public int tableSize;
    string randomCRN;
    
    //각 값을 보유할 클래스 생성
    [System.Serializable]
    public class chatRoomName
    {
        public string Adjective;
        public string Noun;
    }

    //리스트를 보유할 클래스 생성
    [System.Serializable]
    public class CRNList
    {
        public chatRoomName[] CRN;
    }

    //각 클래스를 기반으로 배열 변수 생성
    public CRNList ChatRoomNameList = new CRNList();

    
    // Start is called before the first frame update
    void Start()
    {
        ReadCSV();
        makeChatRoomName();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //본격적으로 CSV파일을 파싱해서 배열정보로 생성하는 함수 작성
    void ReadCSV()
    {
        //참조한 CSV파일을 ,와 엔터단위로 파싱
        string[] CSVdata = csvfile.text.Split(new string[] {",", "\n"}, StringSplitOptions.None);

        ChatRoomNameList.CRN = new chatRoomName[tableSize];
        
        for(int i = 0; i<tableSize-1; i++)
        {
            ChatRoomNameList.CRN[i] = new chatRoomName();
            ChatRoomNameList.CRN[i].Adjective = (CSVdata[2*(i+1)]);
            ChatRoomNameList.CRN[i].Noun = (CSVdata[2*(i+1)+1]);
        }
        
    }

    void makeChatRoomName()
    {
        int AdjNum = UnityEngine.Random.Range(1,tableSize);
        int NounNum = UnityEngine.Random.Range(1,tableSize);
        
        string adj = ChatRoomNameList.CRN[AdjNum].Adjective;
        string noun = ChatRoomNameList.CRN[NounNum].Noun;

        int adjNum = adj.Length;
        int nounNum = noun.Length;
        Debug.Log(adjNum+nounNum+1);
        Debug.Log(adjNum);
        Debug.Log(nounNum);

        if(adjNum+nounNum+1 < 8){
            Debug.Log(adj + " " + noun);
        }
        else{
            makeChatRoomName();
        }

    }
}
